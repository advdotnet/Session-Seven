using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using SessionSeven.Components;
using System;

namespace SessionSeven.Functional.Test
{
	[TestClass]
	public class CircleTest
	{
		[TestMethod]
		public void EqualityTestPositive()
		{
			var circleA = new Circle(new Vector2(10, 10), 5);
			var circleB = new Circle(new Vector2(10, 10), 5);

			Assert.IsTrue(circleA.Equals(circleB));
			Assert.IsTrue(circleA == circleB);
		}

		[TestMethod]
		public void EqualityTestNegative()
		{
			var circleA = new Circle(new Vector2(10, 10), 5);
			var circleB = new Circle(new Vector2(10, 10), 5.2f);

			Assert.IsFalse(circleA.Equals(circleB));
			Assert.IsFalse(circleA == circleB);
		}

		[TestMethod]
		public void ContainsTestPositive()
		{
			var circle = new Circle(Vector2.Zero, 1 * 2);

			Assert.IsTrue(circle.Contains(Vector2.UnitX * 2));
			Assert.IsTrue(circle.Contains(Vector2.UnitY * 2));
			Assert.IsTrue(circle.Contains(-Vector2.UnitX * 2));
			Assert.IsTrue(circle.Contains(-Vector2.UnitY * 2));
			Assert.IsTrue(circle.Contains((Vector2.UnitX + Vector2.UnitY) * 2 / (float)Math.Sqrt(2)));
		}

		[TestMethod]
		public void ContainsTestNegative()
		{
			var circle = new Circle(Vector2.Zero, 1);

			Assert.IsFalse(circle.Contains(1.01f * Vector2.UnitX));
			Assert.IsFalse(circle.Contains(1.01f * Vector2.UnitY));
			Assert.IsFalse(circle.Contains(1.01f * -Vector2.UnitX));
			Assert.IsFalse(circle.Contains(1.01f * -Vector2.UnitY));
			Assert.IsFalse(circle.Contains(Vector2.UnitX + Vector2.UnitY));
		}

		[TestMethod]
		public void IntersectsTestPositive()
		{
			var circle = new Circle(Vector2.Zero, 1 * 2);

			Assert.IsTrue(circle.Intersects(Vector2.UnitX, Vector2.UnitY));
			Assert.IsTrue(circle.Intersects(Vector2.UnitY, Vector2.UnitX));
			Assert.IsTrue(circle.Intersects(Vector2.Zero, Vector2.UnitX));
		}

		[TestMethod]
		public void IntersectsTestNegative()
		{
			var circle = new Circle(Vector2.Zero, 1);

			Assert.IsFalse(circle.Intersects(Vector2.UnitX, new Vector2(2, 0)));
			Assert.IsFalse(circle.Intersects(new Vector2(1, 2), new Vector2(1, -2)));
			Assert.IsFalse(circle.Intersects(new Vector2(2, 2), new Vector2(2, -2)));
		}
	}
}
