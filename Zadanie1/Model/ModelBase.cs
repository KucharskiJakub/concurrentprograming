using System.Collections;
using System.Collections.ObjectModel;
using Logic;

namespace Model
{
    public abstract class ModelBase
    {
        public abstract IList Balls(int numberOfBalls);
        
        public abstract void Animation(IList balls);
        
        public abstract void StopAnimation(IList balls);
        public static ModelBase CreateApi()
        {
            return new ModelAPI();
        }
    }
}
