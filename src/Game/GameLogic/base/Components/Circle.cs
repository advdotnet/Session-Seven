using Microsoft.Xna.Framework;
using System;

namespace SessionSeven.Components
{
    [Serializable]
    public struct Circle
    {
        private float _Radius;
        private float _RadiusSquared;
        private Vector2 _Center;

        public Vector2 Center
        {
            get { return _Center; }
            set { _Center = value; }
        }

        public float Radius
        {
            get { return _Radius; }
            set
            {
                _Radius = value;
                _RadiusSquared = value * value;
            }
        }

        public Circle(Vector2 center, float radius) : this()
        {
            Center = center;
            Radius = radius;
        }

        /// <summary>
        /// Returns true if the given point is inside the circle
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool Contains(Vector2 point)
        {
            return ((point - Center).LengthSquared() <= _RadiusSquared);
        }

        /// <summary>
        /// Returns true, if this circle is intersected by a line segment given by vectors a and b.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public bool Intersects(Vector2 a, Vector2 b)
        {
            if (0 >= Radius)
            {
                return false;
            }

            var P1 = a - Center;
            var P2 = b - Center;

            var Diff = b - a;
            var D = P1.X * P2.Y - P2.X * P1.Y;

            var di = _RadiusSquared * Diff.LengthSquared() - (D * D);

            return (di >= 0) && InBetween(Center.X, a.X, b.X) && InBetween(Center.Y, a.Y, b.Y);
        }

        private static bool InBetween(float val, float a, float b)
        {
            return Math.Min(a, b) <= val &&
                val <= Math.Max(a, b);
        }

        public override bool Equals(Object obj)
        {
            return obj is Circle &&
                this == (Circle)obj;
        }

        public bool Equals(Circle obj)
        {
            return this == obj;
        }

        public override int GetHashCode()
        {
            return Center.GetHashCode() ^ Radius.GetHashCode();
        }

        public static bool operator ==(Circle x, Circle y)
        {
            return x.Center == y.Center && x.Radius == y.Radius;
        }

        public static bool operator !=(Circle x, Circle y)
        {
            return !(x == y);
        }
    }
}
