using Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Data
{
    public class Ball : INotifyPropertyChanged
    {
        private double _r, _ro, _m, _v;
        private Vector _position, _velocity;


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Ball(double x, double y, double r, double ro)
        {
            _position = new Vector(x, y);
            _velocity = new Vector(0, 0);
            _r = r;
            _ro = ro;
            _m = BallMass(_r, _ro);
        }

        public Ball(double x, double y, double r, double ro, double vx, double vy)
        {
            _position = new Vector(x, y);
            _velocity = new Vector(vx, vy);
            _r = r;
            _ro = ro;
            _m = BallMass(_r, _ro);
        }

        public Vector Position
        {
            get => _position;
            set
            {
                if (value.Equals(_position))
                    return;
                _position = value;
                RaisePropertyChanged(nameof(Position));
            }
        }

        public Vector Velocity
        {
            get => _velocity;
            set
            {
                if (value.Equals(_velocity))
                    return;
                _velocity = value;
                RaisePropertyChanged(nameof(Velocity));
            }
        }
        private double BallMass(double r, double ro)
        {
            double m = 0, v = 0;
            v = (4 / 3) * Math.PI * r * r * r;
            m = v * ro;
            return m;
        }

        public double R
        { 
            get => _r;
            set
            {
                if (value.Equals(_r))
                    return;
                _r = value;
                RaisePropertyChanged(nameof(R));
            }
        }
        public double Ro
        { 
            get => _ro;
            set
            {
                if (value.Equals(_ro))
                    return;
                _ro = value;
                RaisePropertyChanged(nameof(Ro));
            }
        }
        public double V
        { 
            get => _v;
            set
            {
                if (value.Equals(_v))
                    return;
                
                _v = value;
                RaisePropertyChanged(nameof(V));
            }
        }
  
        
        public double Mass => _m;

    }
}
