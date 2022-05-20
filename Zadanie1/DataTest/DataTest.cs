using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dane;
using System.Collections.ObjectModel;

namespace DataTest
{
    [TestClass]
    public class DataTest
    {
        [TestMethod]
        public void CreateBallsTest()
        {
            DataApi _dataAPI = new DataApi();
            int xlim = 100;
            int ylim = 100;
            ObservableCollection<Ball> ballList = (ObservableCollection<Ball>)_dataAPI.CreateBalls(10);
            foreach (Ball ball in ballList)
            {
                Assert.IsTrue(ball.X < 1040);
                Assert.IsTrue(ball.Y < 540);
                // Stroke == 10
                Assert.IsTrue(ball.X >= 0);
                Assert.IsTrue(ball.Y >= 0);
            }
        }
        [TestMethod]
        public void BallTest()
        {
            Ball ball = new Ball(2, 5, 20, 2);
            Assert.AreEqual(ball.X, 2);
            Assert.AreEqual(ball.Y, 5);
            Assert.AreEqual(ball.R, 20);
            Assert.AreEqual(ball.Ro, 2);
            Assert.AreEqual(ball.Mass, 20 / 2 * 2 * 0.1);
        }
    }
}
