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

            double x, y, r, ro;
            Random random = new Random();
            for (int i = 0; i < numberOfBalls; i++)
            {
                x = random.Next(0, 1010);
                y = random.Next(0, 510);
                r = random.Next(20, 40);
                ro = random.Next(1, 5);
                _ballList.Add(new Ball(x, y, r, ro));
            }
            return _ballList;
        }
    }
}