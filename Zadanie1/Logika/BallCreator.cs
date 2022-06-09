using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Dane;
using Data;

[assembly: InternalsVisibleTo("LogicTest")]

namespace Logic
{
    public class BallCreator : LogicAbstractAPI
    {
        public List<Task> _targets;
        
        public CancellationToken _token;
        public CancellationTokenSource _tokenSource;
        
        private readonly object _locker = new object();
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
                _targets.Add(Task.Run(() => Move(balls, ball)));
            }
        }
        
        public async void Move(IList balls, Ball ball)
        {
            while (true)
            {
                await Task.Delay(15);
                // Odbicie od prawej ściany
                if (ball.Position.X + ball.Velocity.X > 1040 - ball.R)
                {
                    ball.Position.X = 1040 - ball.R;
                    ball.Velocity.X *= -1;
                }
                // Odbicie od lewej ściany
                else if (ball.Position.X + ball.Velocity.X < 0)
                {
                    ball.Position.X = 0;
                    ball.Velocity.X *= -1;
                }
                else
                {
                    ball.Position.X += ball.Velocity.X;
                }

                // Odbicie od dolnej ściany
                if (ball.Position.Y + ball.Velocity.Y > 540 - ball.R)
                {
                    ball.Position.Y = 540 - ball.R;
                    ball.Velocity.Y *= -1;
                }
                // Odbicie od górnej ściany
                else if (ball.Position.Y + ball.Velocity.Y < 0)
                {
                    ball.Position.Y = 0;
                    ball.Velocity.Y *= -1;
                }
                else
                {
                    ball.Position.Y += ball.Velocity.Y;
                }
                // Sprawdzenie, czy nalezy zatrzymac kule
                try { _token.ThrowIfCancellationRequested(); }
                catch (OperationCanceledException) { break; }
                IsBallsClash(balls, ball);
            }
        }
        public void IsBallsClash(IList balls, Ball ball)
        {
            foreach (Ball ball1 in balls)
            {
                if (ball1 == ball)
                    continue;
                Vector relativePosition = ball.Position - ball1.Position;
                double distance = Math.Sqrt(relativePosition.MagnitudeSquared());
                if (distance * 2 <= ball.R + ball1.R)
                {
                    B2B(ball, ball1);
                }
            }
        }
        public void B2B(Ball ball1, Ball ball2)
        {
            Vector relativeVelocity = ball2.Velocity - ball1.Velocity;
            Vector relativePos = ball2.Position - ball1.Position;
            // Jeśli nie lecą na siebie
            if (Vector.DotProduct(relativePos, relativeVelocity) > 0)
            {
                return;
            }
            Vector newV1 = ball1.Velocity - 2 * ball2.Mass / (ball1.Mass + ball2.Mass) * Vector.DotProduct(ball1.Velocity - ball2.Velocity, ball1.Position - ball2.Position) / (ball1.Position - ball2.Position).MagnitudeSquared() * (ball1.Position - ball2.Position);
            Vector newV2 = ball2.Velocity - 2 * ball1.Mass / (ball1.Mass + ball2.Mass) * Vector.DotProduct(ball2.Velocity - ball1.Velocity, ball2.Position - ball1.Position) / (ball2.Position - ball1.Position).MagnitudeSquared() * (ball2.Position - ball1.Position);
            if (double.IsNaN(ball1.Velocity.X) || double.IsNaN(ball2.Velocity.X) || double.IsNaN(ball1.Velocity.Y) || double.IsNaN(ball2.Velocity.Y))
            {
                return;
            }
            Vector initVel1 = ball1.Velocity;
            Vector initVel2 = ball2.Velocity;
            
            // Sekcja krytyczna
            lock (_locker)
                {
                    ball1.Velocity = newV1;
                    ball2.Velocity = newV2;


                }

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
