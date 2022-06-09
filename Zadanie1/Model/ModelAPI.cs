using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Logic;

namespace Model
{
    internal class ModelAPI : ModelBase
    {
        private readonly LogicAbstractAPI _logic;
        
        public override IList Balls(int numberOfBalls)
            => _logic.CreateBalls(numberOfBalls);
        
        public override void Animation(IList balls)
            => _logic.Start(balls);
        
        public override void StopAnimation(IList balls) => _logic.Exit(balls);
        public ModelAPI() : this(LogicAbstractAPI.CreateBallApi()) { }
        public ModelAPI(LogicAbstractAPI logic) { _logic = logic; }
    }
}