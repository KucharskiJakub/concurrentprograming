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
        
        public ObservableCollection<Ball> CreateBalls(int number)
        {
            ObservableCollection<Ball> ballList = new ObservableCollection<Ball>();
            tokenSource = new CancellationTokenSource();
            token = tokenSource.Token;
            targets = new List<Task>();
            double x, y;
            Random random = new Random();
            for (int i = 0; i < number; i++)
            {
                x = random.Next(0, 1010);
                y = random.Next(0, 510);
                ballList.Add(new Ball(x, y));
            }
            return ballList;
        }
        public void Target(ObservableCollection<Ball> balls)
        {
            double x, y;
            Random random = new Random();
            foreach (Ball ball in balls)
            {
                x = random.Next(0, 1010);
                y = random.Next(0, 510);
                Thread.Sleep(5);
                targets.Add(Task.Run(() => Steper(ball, x, y)));
            }
        }
        
        public async void Steper(Ball ball, double x2, double y2)
        {
            int steps;
            double xRest, yRest, xMove, yMove;
            double distance;
            
            Random random = new Random();
            
            while (true)
            {
                distance = Pythagoras(ball.X, x2, ball.Y, y2);
                steps = (int) distance / 5;
                (xMove, yMove, xRest, yRest) = MoveLength(ball.X, x2, ball.Y, y2, steps);
                for (int i = 0; i < steps; i++)
                {
                    await Task.Delay(20);
                    ball.X += xMove;
                    ball.Y += yMove;
                }
                await Task.Delay(20);
                ball.X += xRest;
                ball.Y += yRest;
                
                x2 = random.Next(0, 1010) + random.NextDouble();
                y2 = random.Next(0, 510) + random.NextDouble();
                
                
                try { token.ThrowIfCancellationRequested(); }
                catch (OperationCanceledException) { break; }
            }
        }



        public double Pythagoras(double x1, double x2, double y1, double y2)
        {
            double a, x, y;
            x = x1 - x2;
            y = y1 - y2;
            x = Math.Abs(x);
            y = Math.Abs(y);
            a = x * x + y * y;
            a = Math.Sqrt(a);
            return a;
        }
        
        public (double, double, double, double) MoveLength(double x1, double x2, double y1, double y2, int steps)
        {
            double x, y, xr, yr;
            x = x2 - x1;
            y = y2 - y1;
            xr = x % 5;
            yr = y % 5;
            x = x - xr;
            x = x / steps;
            y = y - yr;
            y = y / steps;
            return (x, y, xr, yr);
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
