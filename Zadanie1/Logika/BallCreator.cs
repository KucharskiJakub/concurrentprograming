using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Dane;

[assembly: InternalsVisibleTo("LogicTest")]

namespace Logic
{
    public class BallCreator : LogicAbstractAPI
    {
        public List<Task> _targets;
        
        public CancellationToken _token;
        public CancellationTokenSource _tokenSource;
        
        private readonly object locker = new object();
        private readonly DataAbstractAPI _data;
        
        public BallCreator() : this(DataAbstractAPI.CreateBallData()) { }
        public BallCreator(DataAbstractAPI data) { _data = data; }
        
        public override IList CreateBalls(int numberOfBalls)
        {
            _targets = new List<Task>();
            _tokenSource = new CancellationTokenSource();
            _token = _tokenSource.Token;
            
            return _data.CreateBalls(numberOfBalls);
        }
        public override void Start(IList balls)
        {
            int token;
            Random random = new Random();
            foreach (Ball ball in balls)
            {
                token = random.Next(0, 2);
                ball.PX = random.Next(0, 101);
                ball.PY = 100-ball.PX;
                if (token == 1)
                {
                    ball.PX = -ball.PX;
                }
                token = random.Next(0, 2);
                if (token == 1)
                {
                    ball.PY = -ball.PY;
                }
                Thread.Sleep(1);
                _targets.Add(Task.Run(() => MainMovement(balls, ball)));
            }
        }
        
        public async void MainMovement(IList balls, Ball ball)
        {
            double w = CalculateVelocity(ball.V, ball.PX, ball.PY);
            ball.VX = ball.PX * w;
            ball.VY = ball.PY * w;
            
            while (true)
            {
                await Task.Delay(20);
                Move(ball ,ball.R, ball.X, ball.Y, ball.VX, ball.VY);

                
                
                try { _token.ThrowIfCancellationRequested(); }
                catch (OperationCanceledException) { break; }
                lock (locker)
                {
                    CantMove(balls, ball);
                }
            }
        }
        
        

        public void CantMove(IList balls, Ball ball)
        {
            
            foreach (Ball b in balls)
            {
                double  r, r1, r2;
                if (b == ball)
                    continue;
                r1 = b.R;
                r2 = ball.R;
                r1 = r1 / 2;
                r2 = r2 / 2;
                r = r1 + r2;
                
                if (Math.Abs(ball.X-b.X)<=r && Math.Abs(ball.Y-b.Y)<=r)
                {
                    if ((b.Y<ball.Y && b.VY<0 && ball.VY >0) || (b.Y>ball.Y && b.VY>0 && ball.VY <0) || (b.X<ball.X && b.VX<0 && ball.VX >0) || (b.Y<ball.Y && b.VY<0 && ball.VY >0))
                    {
                        return;
                    }
                    if (b.VX*ball.VX<0 || b.VY*ball.VY<0)
                    {
                        
                        double x1, x2, y1, y2;
                        x1 = b.X;
                        x2 = ball.X;
                        y1 = b.Y;
                        y2 = ball.Y;
                        
                        double dx, dy, mx, my;
                        dx = Math.Abs(b.X - ball.X);
                        dy = Math.Abs(b.Y - ball.Y);
                        int a = 1;
                        int c = 1;
                        while (dx>r)
                        {
                            x1 += b.VX * a * 0.01;
                            x2 += ball.VX * a * 0.01;
                            dx = Math.Abs(x1 - x2);
                            a++;
                        }
                        while (dy>r)
                        {
                            y1 += b.VX * a * 0.01;
                            y2 += ball.VX * a * 0.01;
                            dy = Math.Abs(y1 - y2);
                            c++;
                        }

                        if (dx < r) a--;
                        if (dy < r) c--;

                        b.X += b.VX*a*0.01;
                        b.Y += b.VY*c*0.01;
                        ball.X += ball.VX*a*0.01;
                        ball.Y += ball.VY*c*0.01;
                        if (Math.Abs(b.X - ball.X) < Math.Abs(b.Y - ball.Y))
                        {
                            my = b.VY;
                            b.VY = (-b.VY+ball.VY)/2;
                            ball.VY = (-ball.VY+my)/2;
                        }
                        else
                        {
                            mx = b.VX;
                            b.VX = (-b.VX+ball.VX)/2;
                            ball.VX = (-ball.VX+mx)/2;
                            
                        }
                        
                        BTB(b, ball);
                    }
                    
                }

            }
        }


        public async void BTB(Ball b, Ball ball)
        {
            await Task.Delay(20);
            double v1, v2;
            v1 = ((b.V*(b.Mass-ball.Mass))+(2*ball.Mass*ball.V))/(b.Mass+ball.Mass);
            v2 = ((ball.V*(ball.Mass-b.Mass))+(2*b.Mass*b.V))/(ball.Mass+b.Mass);
            if (v2 > 10) v2 = 10;
            if (v1 > 10) v1 = 10;
            b.V = v2;
            ball.V = v1;
            double w1 = CalculateVelocity(b.V, b.PX, b.PY);
            b.VX = b.PX * w1;
            b.VY = b.PY * w1;
            double w2 = CalculateVelocity(ball.V, ball.PX, ball.PY);
            ball.VX = ball.PX * w2;
            ball.VY = ball.PY * w2;
            b.X += b.VX;
            b.Y += b.VY;
            ball.X += ball.VX;
            ball.Y += ball.VY;
            await Task.Delay(20);
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
        
        public override void Exit()
        {
            if (!_tokenSource.IsCancellationRequested)
            {
                _tokenSource.Cancel();
            }
        }
        

    }
}
