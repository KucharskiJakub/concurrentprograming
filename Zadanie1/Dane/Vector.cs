using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Data
{
    public class Vector : INotifyPropertyChanged
    {
        private double _x, _y;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

        public Vector(double x, double y)
        {
            _x = x;
            _y = y;
        }

        public Vector(Vector v)
        {
            if (v != null)
            {
                _x = v.X;
                _y = v.Y;
            }
        }

        public Vector Add(Vector other)
        {
            return new Vector(this.X + other.X, this.Y + other.Y);
        }

        public Vector MultiplyByScalar(double scalar)
        {
            return new Vector(scalar * this.X, scalar * this.Y);
        }

        // Iloczyn skalarny
        public static double DotProduct(Vector v1, Vector v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y;
        }

        // Długość wektora
        public double MagnitudeSquared()
        {
            return this.X * this.X + this.Y * this.Y;
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            return v1.Add(v2);
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            return v1.Add(v2.MultiplyByScalar(-1));
        }

        public static Vector operator *(double scalar, Vector v)
        {
            return v.MultiplyByScalar(scalar);
        }
        public static Vector operator *(Vector v, double scalar)
        {
            return scalar * v;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj is Vector other)
            {
                if (this.X == other.X && this.Y == other.Y)
                    return true;
                return false;
            }
            else
            {
                return false;
            }
        }
    }
}
