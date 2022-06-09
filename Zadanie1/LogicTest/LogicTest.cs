using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic;
using Dane;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Data;

namespace LogicTest
{
    [TestClass]
    public class LogicTest
    {
        
        [TestMethod]
        public void Test1()
        {
            BallCreator ballCreator = new BallCreator();
            ObservableCollection<Ball> ballList = new ObservableCollection<Ball>();
         
            double x, y, r, ro, vx, vy, v;
            Random random = new Random();
            for (int i = 0; i < 50; i++)                       
            {
                v = random.Next(1, 6);
                r = random.Next(20, 40);
                x = random.Next(0, (int)(1040 - r)) + random.NextDouble();
                y = random.Next(0, (int)(540 - r)) + random.NextDouble();
                vx = random.NextDouble() * v * (random.Next(0, 1) == 0 ? -1 : 1);
                vy = Math.Sqrt(v * v - vx * vx) * (random.Next(0, 1) == 0 ? -1 : 1);
                ro = random.Next(1, 5);
                ballList.Add(new Ball(x, y, r, ro, vx, vy));
            }

            foreach (var ball in ballList)
            {
                Assert.IsTrue(ball.Destination.X >= 0 && ball.Destination.X <= 1010);
                Assert.IsTrue(ball.Destination.Y >= 0 && ball.Destination.Y <= 510);
            }
            Assert.IsTrue(ballList.Count == 50);
        }


        
    }
}