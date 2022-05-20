using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;



namespace Logic
{
    public abstract class LogicAbstractAPI
    {
        public static LogicAbstractAPI CreateBallApi() => new BallCreator();

        public abstract IList CreateBalls(int numberOfBalls);

        public abstract void Start(IList balls);
        
        public abstract void Exit();
    }
}