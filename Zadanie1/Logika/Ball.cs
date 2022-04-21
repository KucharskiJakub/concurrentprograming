using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace Logic
{
   public class Ball : INotifyPropertyChanged
    {
        private double _x, _y;


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Ball(double x, double y)
        {
            _x = x;
            _y = y;
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

    }
} 
