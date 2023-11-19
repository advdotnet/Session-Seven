using Microsoft.Xna.Framework;
using System;

namespace SessionSeven.Components
{
	[Serializable]
	public struct Circle
	{
		private float _radius;
		private float _radiusSquared;
		private Vector2 _center;

		public Vector2 Center
		{
			get => _center;
			set => _center = value;
		}

		public float Radius
		{
			get => _radius;
			set
			{
				_radius = value;
				_radiusSquared = value * value;
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
			return (point - Center).LengthSquared() <= _radiusSquared;
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

			var p1 = a - Center;
			var p2 = b - Center;

			var diff = b - a;
			var d = (p1.X * p2.Y) - (p2.X * p1.Y);

			var di = (_radiusSquared * diff.LengthSquared()) - (d * d);

			return (di >= 0) && InBetween(Center.X, a.X, b.X) && InBetween(Center.Y, a.Y, b.Y);
		}

		private static bool InBetween(float val, float a, float b)
		{
			return Math.Min(a, b) <= val &&
				val <= Math.Max(a, b);
		}

		public override bool Equals(object obj) => obj is Circle circle && this == circle;

		public bool Equals(Circle obj) => this == obj;

		public override int GetHashCode() => Center.GetHashCode() ^ Radius.GetHashCode();

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
