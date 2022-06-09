using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class CollisionInfo
    {
        Vector _initial_vel_1;
        Vector _initial_vel_2;
        Ball _ball_1;
        Ball _ball_2;

        public CollisionInfo(Vector initial_vel_1,
                             Vector initial_vel_2,
                             Ball ball_1,
                             Ball ball_2)
        {
            this._initial_vel_1 = initial_vel_1;
            this._initial_vel_2 = initial_vel_2;
            this._ball_1 = ball_1;
            this._ball_2 = ball_2;
        }
        public Vector InitialVel1
        {
            get => _initial_vel_1;
        }

        public Vector InitialVel2
        {
            get => _initial_vel_2;
        }

        public Ball Ball_1
        {
            get => _ball_1;
        }

        public Ball Ball_2
        {
            get => _ball_2;
        }
    }
}
