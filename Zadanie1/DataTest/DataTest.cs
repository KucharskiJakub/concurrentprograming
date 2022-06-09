using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dane;
using System.Collections.ObjectModel;
using Data;

namespace DataTest
{
    [TestClass]
    public class DataTest
    {
        [TestMethod]
        public void CreateBallsTest()
        {
            DataApi _dataAPI = new DataApi();
            ObservableCollection<Ball> ballList = (ObservableCollection<Ball>)_dataAPI.CreateBalls(10);
            foreach (Ball ball in ballList)
            {
                Assert.IsTrue(ball.Destination.X < 1040);
                Assert.IsTrue(ball.Destination.Y < 540);
                // Stroke == 10
                Assert.IsTrue(ball.Destination.X >= 0);
                Assert.IsTrue(ball.Destination.Y >= 0);
            }
        }
        [TestMethod]
        public void BallTest()
        {
            Ball ball = new Ball(2, 5, 20, 2);
            Assert.AreEqual(ball.Destination.X, 2);
            Assert.AreEqual(ball.Destination.Y, 5);
            Assert.AreEqual(ball.R, 20);
            Assert.AreEqual(ball.Ro, 2);
        }
    }
}
