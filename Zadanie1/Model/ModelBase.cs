using System.Collections;
using System.Collections.ObjectModel;
using Logic;

namespace Model
{
    public abstract class ModelBase
    {
        public abstract ObservableCollection<Ball> Balls(int numberOfBalls);
        
        public abstract void Animation(IList balls);
        
        public abstract void StopAnimation();
        public static ModelBase CreateApi()
        {
            return new ModelAPI();
        }
    }
    internal class ModelAPI : ModelBase
    {
        private readonly BallCreator creator = new BallCreator();
        public override ObservableCollection<Ball> Balls(int numberOfBalls)
        => creator.CreateBalls(numberOfBalls);
        
        public override void Animation(IList balls)
        => creator.Target((ObservableCollection<Ball>)balls);
        
        public override void StopAnimation() => creator.Exit();
    }
}
