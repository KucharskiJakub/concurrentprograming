using Data;
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
        public abstract bool NewSession { get; set; }

        public abstract void AppendObjectToJSONFile(string filename, string newJsonObject);

        public abstract CollisionInfo CollisionInfoObject(Vector initial_vel_1, Vector initial_vel_2, Ball ball_1, Ball ball_2);
    }
}