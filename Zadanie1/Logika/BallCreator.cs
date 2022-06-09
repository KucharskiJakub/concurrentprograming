using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
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
        private readonly object _fileLocker = new object();
        private readonly DataAbstractAPI _data;

        private string _logPath = "ball_log.json";

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
            LogBalls(balls);
            foreach (Ball ball in balls)
            {
                _targets.Add(Task.Run(() => Move(balls, ball)));
            }
        }
        
        public async void Move(IList balls, Ball ball)
        {
            while (true)
            {
                await Task.Delay(20);
                if (ball.Destination.X + ball.Velocity.X > 1040 - ball.R)
                {
                    ball.Destination.X = 1040 - ball.R;
                    ball.Velocity.X *= -1;
                }
                else if (ball.Destination.X + ball.Velocity.X < 0)
                {
                    ball.Destination.X = 0;
                    ball.Velocity.X *= -1;
                }
                else
                {
                    ball.Destination.X += ball.Velocity.X;
                }

                if (ball.Destination.Y + ball.Velocity.Y > 540 - ball.R)
                {
                    ball.Destination.Y = 540 - ball.R;
                    ball.Velocity.Y *= -1;
                }
                else if (ball.Destination.Y + ball.Velocity.Y < 0)
                {
                    ball.Destination.Y = 0;
                    ball.Velocity.Y *= -1;
                }
                else
                {
                    ball.Destination.Y += ball.Velocity.Y;
                }
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
                Vector relativePosition = ball.Destination - ball1.Destination;
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
            Vector relativePos = ball2.Destination - ball1.Destination;
            if (Vector.DotProduct(relativePos, relativeVelocity) > 0)
            {
                return;
            }
            Vector newV1 = ball1.Velocity - 2 * ball2.Mass / (ball1.Mass + ball2.Mass) * Vector.DotProduct(ball1.Velocity - ball2.Velocity, ball1.Destination - ball2.Destination) / (ball1.Destination - ball2.Destination).MagnitudeSquared() * (ball1.Destination - ball2.Destination);
            Vector newV2 = ball2.Velocity - 2 * ball1.Mass / (ball1.Mass + ball2.Mass) * Vector.DotProduct(ball2.Velocity - ball1.Velocity, ball2.Destination - ball1.Destination) / (ball2.Destination - ball1.Destination).MagnitudeSquared() * (ball2.Destination - ball1.Destination);
            if (double.IsNaN(ball1.Velocity.X) || double.IsNaN(ball2.Velocity.X) || double.IsNaN(ball1.Velocity.Y) || double.IsNaN(ball2.Velocity.Y))
            {
                return;
            }
            Vector initVel1 = ball1.Velocity;
            Vector initVel2 = ball2.Velocity;

            JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
            string jsonCollisionInfo;
            string now;
            string newJsonObject;

            lock (_locker) lock (_fileLocker)
                {
                    ball1.Velocity = newV1;
                    ball2.Velocity = newV2;

                    jsonCollisionInfo = JsonSerializer.Serialize(_data.CollisionInfoObject(initVel1, initVel2, ball1, ball2), options);
                    now = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff");
                    newJsonObject = "{" + String.Format("\n\t\"datetime\": \"{0}\",\n\t\"collision\":{1}\n", now, jsonCollisionInfo) + "}";
                    _data.AppendObjectToJSONFile(_logPath, newJsonObject);

                }

        }
        public void CallLogger(int interval, IList balls)
        {
            while (true)
            {
                Thread.Sleep(interval);
                // Zatrzymaj log jeśli zatrzymano kule
                try { _token.ThrowIfCancellationRequested(); }
                catch (OperationCanceledException) { break; }
                LogBalls(balls);
            }
        }

        public void LogBalls(IList balls)
        {
            JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
            string jsonBalls = JsonSerializer.Serialize(balls, options);
            string now = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff");

            string newJsonObject = "{" + String.Format("\n\t\"datetime\": \"{0}\",\n\t\"balls\":{1}\n", now, jsonBalls) + "}";
            lock (_fileLocker)
            {
                _data.AppendObjectToJSONFile(_logPath, newJsonObject);
            }
        }




        public override void Exit(IList balls)
        {
            if (!_tokenSource.IsCancellationRequested)
            {
                _tokenSource.Cancel();
                LogBalls(balls);
            }
        }
        

    }
}
