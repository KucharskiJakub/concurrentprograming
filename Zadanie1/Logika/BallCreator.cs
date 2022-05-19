using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace Logic
{
    public class BallCreator
    {
        public List<Task> targets;
        
        public CancellationToken token;
        public CancellationTokenSource tokenSource;
        
        private readonly object locker = new object();
        
        public ObservableCollection<Ball> CreateBalls(int number)
        {
            ObservableCollection<Ball> ballList = new ObservableCollection<Ball>();
            tokenSource = new CancellationTokenSource();
            token = tokenSource.Token;
            targets = new List<Task>();
            double x, y, r, ro;
            Random random = new Random();
            for (int i = 0; i < number; i++)
            {
                x = random.Next(0, 1010);
                y = random.Next(0, 510);
                r = random.Next(20, 40);
                ro = random.Next(1, 5);
                ballList.Add(new Ball(x, y, r, ro));
            }
            return ballList;
        }
        public void Start(IList<Ball> balls)
        {
            double px, py;
            int token;
            Random random = new Random();
            foreach (Ball ball in balls)
            {
                token = random.Next(0, 2);
                px = random.Next(0, 101);
                py = 100-px;
                if (token == 1)
                {
                    px = -px;
                }
                token = random.Next(0, 2);
                if (token == 1)
                {
                    py = -py;
                }
                Thread.Sleep(1);
                targets.Add(Task.Run(() => MainMovement(balls, ball, px, py)));
            }
        }
        
        public async void MainMovement(IList<Ball> balls, Ball ball, double px, double py)
        {
            double w = CalculateVelocity(ball.V, px, py);
            ball.VX = px * w;
            ball.VY = py * w;
            
            while (true)
            {
                await Task.Delay(20);
                Move(ball ,ball.R, ball.X, ball.Y, ball.VX, ball.VY);

                
                
                try { token.ThrowIfCancellationRequested(); }
                catch (OperationCanceledException) { break; }
                lock (locker)
                {
                    LookForCollisionsNaive(balls, ball);

                }
            }
        }
        
        


// Iterowanie, bez drzewa
        public void LookForCollisionsNaive(IList<Ball> balls, Ball ball)
        {
            // Iteruje po wszystkich parach kul. Jeśli są wystarczająco blisko siebie, to zatrzymuje ich ruch, oblicza 
            // prędkość po zderzeniu i wznawia ruch
            foreach (Ball b in balls)
            {
                double z1, z2, r, r1, r2;
                if (b == ball)
                    continue;
                r1 = b.R;
                r2 = ball.R;
                r1 = r1 / 2;
                r2 = r2 / 2;
                r = r1 + r2;
                
                double distance = Pythagoras(b.X, ball.X, b.Y, ball.Y, r1, r2);
                z1 = Math.Abs(b.VX+r1 - ball.VX+r2);
                z2 = Math.Abs(b.VY+r1 - ball.VY+r2);
                if (z1+r >=distance && z2+r>=distance)
                {
                    if (b.VX<0) { b.X += (-z1 / 2) - r1; }
                    else if (b.VX>=0) { b.X += (z1 / 2) - r1; }
                    if (b.VY<0) { b.Y += (-z2 / 2) - r1; }
                    else if (b.VY>=0) { b.Y += (z2 / 2) - r1; }
                    if (ball.VX<0) { ball.X += (-z1 / 2) - r2; }
                    else if (ball.VX>=0) { ball.X += (z1 / 2) - r2; }
                    if (ball.VY<0) { ball.Y += (-z2 / 2) - r2; }
                    else if (ball.VY>=0) { ball.Y += (z2 / 2) - r2; }

                    
                    rrr(ball, b);
                }

            }
        }

        public async void rrr(Ball ball, Ball b)
        {
            await Task.Delay(20); 
            b.VX = -b.VX;
            b.VY = -b.VY;
            ball.VX = -ball.VX;
            ball.VY = -ball.VY;
        }

        public double Pythagoras(double x1, double x2, double y1, double y2, double r1, double r2)
        {
            double a, x, y;
            x1 += r1;
            y1 += r1;
            x2 += r2;
            y2 += r2;
            x = x1 - x2;
            y = y1 - y2;
            x = Math.Abs(x);
            y = Math.Abs(y);
            a = x * x + y * y;
            a = Math.Sqrt(a);
            return a;
        }

        public double CalculateVelocity(double v, double px, double py)
        {
            double w = Math.Sqrt((v * v) / ((px * px) + (py * py)));
            return w;
        }
        public void Move(Ball ball, double r, double x, double y, double dx, double dy)
        {
            double wx, wy;
            bool xchanged = false, ychanged = false;
            wx = x + dx;
            wy = y + dy;
            if (wy>540-r || wy < 0)
            {
                dy = -dy;
                if (wy < 0)
                {
                    ball.Y = 0;
                }
                else
                {
                    ball.Y = 540 - r;
                }
                ychanged = true;
            }
            if (wx > 1040-r || wx < 0)
            {
                dx = -dx;
                if (wx < 0)
                {
                    ball.X = 0;
                }
                else
                {
                    ball.X = 1040 - r;
                }
                xchanged = true;
            }

            if (!xchanged)
            {
                ball.X += dx;
            }
            if (!ychanged)
            {
                ball.Y += dy;
            }

            ball.VX = dx;
            ball.VY = dy;
        }
        
        public void Exit()
        {
            if (!tokenSource.IsCancellationRequested)
            {
                tokenSource.Cancel();
            }
        }
        

    }
}
