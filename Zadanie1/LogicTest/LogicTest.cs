using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace LogicTest
{
    [TestClass]
    public class LogicTest
    {
        
        [TestMethod]
        public void CreatingBallsTest()
        {
            ObservableCollection<Ball> ballList = new ObservableCollection<Ball>();
            double x, y;
            Random random = new Random();
            for (int i = 0; i < 50; i++)                       
            {
                x = random.Next(0, 1010);
                y = random.Next(0, 510);
                ballList.Add(new Ball(x, y));
            }

            foreach (var ball in ballList)
            {
                Assert.IsTrue(ball.X >= 0 && ball.X <= 1010);
                Assert.IsTrue(ball.Y >= 0 && ball.Y <= 510);
            }
            Assert.IsTrue(ballList.Count == 50);
        }

        [TestMethod]
        public void BallConstuctorTest()
        {
            Ball ball = new Ball(2,5);
            Assert.AreEqual(ball.X, 2);
            Assert.AreEqual(ball.Y, 5);
        }

        [TestMethod]
        public void SteperTest()
        {
            ObservableCollection<Ball> balls = new ObservableCollection<Ball>();
            BallCreator creator = new BallCreator();
            balls = creator.CreateBalls(1);
            Assert.IsNotNull(balls[0]);
            Ball ball = new Ball(balls[0].X, balls[0].Y);
            Assert.IsNotNull(ball);

            creator.Steper(ball, 100,200);
            Thread.Sleep(100);
            
            Assert.AreNotEqual(ball.X, balls[0].X);
            Assert.AreNotEqual(ball.Y, balls[0].Y);
            creator.tokenSource.Cancel();
        }
        
        [TestMethod]
        public void PythagorasTest()
        {
             BallCreator creator = new BallCreator();
             Assert.AreEqual(creator.Pythagoras(5, 2, 6, 2), 5);
        }
    }
}