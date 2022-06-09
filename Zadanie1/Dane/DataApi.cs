using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

[assembly: InternalsVisibleTo("DataTest")]

namespace Dane
{
    internal class DataApi : DataAbstractAPI
    {
        private ObservableCollection<Ball> _ballList;
        public override ObservableCollection<Ball> CreateBalls(int numberOfBalls)
        {
            _ballList = new ObservableCollection<Ball>();

            double x, y, r, ro, vx, vy, v;
            Random random = new Random();
            for (int i = 0; i < numberOfBalls; i++)
            {
                v = random.Next(1, 6);
                r = random.Next(20, 40);
                x = random.Next(0, (int)(1040 - r)) + random.NextDouble();
                y = random.Next(0, (int)(540 - r)) + random.NextDouble();
                vx = random.NextDouble() * v * (random.Next(0, 1) == 0 ? -1 : 1);
                vy = Math.Sqrt(v * v - vx * vx) * (random.Next(0, 1) == 0 ? -1 : 1);
                ro = random.Next(1, 5);
                _ballList.Add(new Ball(x, y, r, ro, vx, vy));
            }
            return _ballList;
        }
    }
}