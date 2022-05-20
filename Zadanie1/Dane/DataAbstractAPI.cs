using System.Collections;
using System.Collections.ObjectModel;

namespace Dane
{
    public abstract class DataAbstractAPI
    {
        public static DataAbstractAPI CreateBallData()
        {
            return new DataApi();
        }
        public abstract ObservableCollection<Ball> CreateBalls(int numberOfBalls);
    }
}