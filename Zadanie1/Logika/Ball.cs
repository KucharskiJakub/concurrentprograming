using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace Logic
{
   public class Ball : INotifyPropertyChanged
    {
        private double _x, _y, _r, _ro, _m, _v, _vx, _vy;


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Ball(double x, double y, double r, double ro)
        {
            _x = x;
            _y = y;
            _r = r;
            _ro = ro;
            _m = _r * _ro;
            _v = 10;
            _vx = 0;
            _vy = 0;
        }
        public double X
        { 
            get => _x;
            set
            {
                if (value.Equals(_x))
                    return;
                _x = value;
                RaisePropertyChanged(nameof(X));
            }
        }

        public double Y
        { 
            get => _y;
            set
            {
                if (value.Equals(_y))
                    return;
                _y = value;
                RaisePropertyChanged(nameof(Y));
            }
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
        public double VX
        { 
            get => _vx;
            set
            {
                if (value.Equals(_vx))
                    return;
                _vx = value;
                RaisePropertyChanged(nameof(VX));
            }
        }
        public double VY
        { 
            get => _vy;
            set
            {
                if (value.Equals(_vy))
                    return;
                _vy = value;
                RaisePropertyChanged(nameof(VY));
            }
        }
        
        public double Mass => _m;

    }
} 
