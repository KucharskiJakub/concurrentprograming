using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic;
using Dane;
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
        public void Test1()
        {
            BallCreator ballCreator = new BallCreator();
            ObservableCollection<Ball> ballList = new ObservableCollection<Ball>();
            double x, y, r, ro;
            Random random = new Random();
            for (int i = 0; i < 50; i++)                       
            {
                x = random.Next(0, 1010);
                y = random.Next(0, 510);
                r = random.Next(20, 40);
                ro = random.Next(1, 5);
                ballList.Add(new Ball(x, y, r, ro));
            }

            foreach (var ball in ballList)
            {
                Assert.IsTrue(ball.X >= 0 && ball.X <= 1010);
                Assert.IsTrue(ball.Y >= 0 && ball.Y <= 510);
            }
            Assert.IsTrue(ballList.Count == 50);
        }


        
    }
}