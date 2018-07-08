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
            var CircleA = new Circle(new Vector2(10, 10), 5);
            var CircleB = new Circle(new Vector2(10, 10), 5);

            Assert.IsTrue(CircleA.Equals(CircleB));
            Assert.IsTrue(CircleA == CircleB);
        }

        [TestMethod]
        public void EqualityTestNegative()
        {
            var CircleA = new Circle(new Vector2(10, 10), 5);
            var CircleB = new Circle(new Vector2(10, 10), 5.2f);

            Assert.IsFalse(CircleA.Equals(CircleB));
            Assert.IsFalse(CircleA == CircleB);
        }

        [TestMethod]
        public void ContainsTestPositive()
        {
            var Circle = new Circle(Vector2.Zero, 1 * 2);

            Assert.IsTrue(Circle.Contains(Vector2.UnitX * 2));
            Assert.IsTrue(Circle.Contains(Vector2.UnitY * 2));
            Assert.IsTrue(Circle.Contains(-Vector2.UnitX * 2));
            Assert.IsTrue(Circle.Contains(-Vector2.UnitY * 2));
            Assert.IsTrue(Circle.Contains((Vector2.UnitX + Vector2.UnitY) * 2 / (float)Math.Sqrt(2)));
        }

        [TestMethod]
        public void ContainsTestNegative()
        {
            var Circle = new Circle(Vector2.Zero, 1);

            Assert.IsFalse(Circle.Contains(1.01f * Vector2.UnitX));
            Assert.IsFalse(Circle.Contains(1.01f * Vector2.UnitY));
            Assert.IsFalse(Circle.Contains(1.01f * -Vector2.UnitX));
            Assert.IsFalse(Circle.Contains(1.01f * -Vector2.UnitY));
            Assert.IsFalse(Circle.Contains(Vector2.UnitX + Vector2.UnitY));
        }

        [TestMethod]
        public void IntersectsTestPositive()
        {
            var Circle = new Circle(Vector2.Zero, 1 * 2);

            Assert.IsTrue(Circle.Intersects(Vector2.UnitX, Vector2.UnitY));
            Assert.IsTrue(Circle.Intersects(Vector2.UnitY, Vector2.UnitX));
            Assert.IsTrue(Circle.Intersects(Vector2.Zero, Vector2.UnitX));
        }

        [TestMethod]
        public void IntersectsTestNegative()
        {
            var Circle = new Circle(Vector2.Zero, 1);

            Assert.IsFalse(Circle.Intersects(Vector2.UnitX, new Vector2(2, 0)));
            Assert.IsFalse(Circle.Intersects(new Vector2(1, 2), new Vector2(1, -2)));
            Assert.IsFalse(Circle.Intersects(new Vector2(2, 2), new Vector2(2, -2)));
        }
    }
}
