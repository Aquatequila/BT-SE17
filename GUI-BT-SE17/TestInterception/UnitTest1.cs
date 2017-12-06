using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;

namespace TestInterception
{
    [TestClass]
    public class UnitTest1
    {
        private static double ToDegrees(double radianValue)
        {
            return radianValue * 180 / Math.PI;
        }
        private static double GetAngle (PointF p1, PointF p2)
        {
            float xAbs = Max(p1.X, p2.X) - Min(p1.X, p2.X);
            float yAbs = Max(p1.Y, p2.Y) - Min(p1.Y, p2.Y);
            float hypotenuse = (float) Math.Sqrt(Math.Pow(xAbs, 2) + Math.Pow(yAbs, 2));

            return ToDegrees (Math.Asin(yAbs / hypotenuse));
        }

        private static float CalculateCoordinate(double hypotenuseA, double xOrYvalue, double hypotenuseB)
        {
            return (float) ((xOrYvalue * hypotenuseB) / hypotenuseA);
        }

        private static Tuple<PointF, PointF> PythagoreanTheorem(PointF point1, PointF point2, double deltaDistanceA, double deltaDistanceB, int sector)
        {
            if ((sector == 3) || (sector == 4)) {
                return PythagoreanTheorem(point1, point2, deltaDistanceB, deltaDistanceA);
            }

            return PythagoreanTheorem(point1, point2, deltaDistanceA, deltaDistanceB);
        }

        private static Tuple<PointF, PointF> PythagoreanTheorem(PointF point1, PointF point2, double deltaDistanceA, double deltaDistanceB)
        {
            var before = new PointF(float.NaN, float.NaN);
            var after = new PointF(float.NaN, float.NaN);

            var x0 = Math.Abs(point1.X - point2.X);
            var y0 = Math.Abs(point1.Y - point2.Y);
            var v0 = Math.Sqrt(Math.Pow(x0, 2) + Math.Pow(y0, 2));

            var distA = v0 - deltaDistanceA;
            var distB = v0 + deltaDistanceB;

            before.X = CalculateCoordinate(v0, x0, distA);
            before.Y = CalculateCoordinate(v0, y0, distA);

            after.X = CalculateCoordinate(v0, x0, distB);
            after.Y = CalculateCoordinate(v0, y0, distB);

            return new Tuple<PointF, PointF> (before, after);
        }

        private static int GetSector (PointF point1, PointF point2)
        {
            var x = point1.X - point2.X;
            var y = point1.Y - point2.Y;

            if (x > 0)
            {
                if (y > 0) return 2;

                else return 3;
            }
            else
            {
                if (y > 0) return 1;

                else return 4;
            }
        }

        private static bool TryGetPointOfIntersection(PointF A, PointF B, PointF C, PointF D, out PointF intersection, out Double angle, out int sector)
        {
            intersection = new PointF(float.NaN, float.NaN);
            angle = 0;
            sector = GetSector(C, D);

            float a1 = B.Y - A.Y;
            float b1 = A.X - B.X;
            float c1 = a1 * A.X + b1 * A.Y;

            float a2 = D.Y - C.Y;
            float b2 = C.X - D.X;
            float c2 = a2 * C.X + b2 * C.Y;

            float det = a1 * b2 - a2 * b1;

            if (det.Equals(0.0))
                return false;

            float x = (b2 * c1 - b1 * c2) / det;
            float y = (a1 * c2 - a2 * c1) / det;

            PointF point = new PointF { X = x, Y = y };

            if (!IsInsideOfBounds(C, D, point))
                return false;

            angle = GetAngle(C, D) + (sector - 1) * 90;
            intersection = point;
            return true;
        }

        private static bool TryGetPointOfIntersection(PointF A, PointF B, PointF C, PointF D, out PointF intersection, out Double angle)
        {
            intersection = new PointF(float.NaN, float.NaN);
            angle = 0;

            float a1 = B.Y - A.Y;
            float b1 = A.X - B.X;
            float c1 = a1 * A.X + b1 * A.Y;

            float a2 = D.Y - C.Y;
            float b2 = C.X - D.X;
            float c2 = a2 * C.X + b2 * C.Y;

            float det = a1 * b2 - a2 * b1;

            if (det.Equals(0.0))
                return false;

            float x = (b2 * c1 - b1 * c2) / det;
            float y = (a1 * c2 - a2 * c1) / det;

            PointF point = new PointF { X = x, Y = y };

            if (!IsInsideOfBounds(C, D, point))
                return false;

            angle = GetAngle(C, D);
            intersection = point;
            return true;
        }
        private static bool TryGetPointOfIntersection(PointF A, PointF B, PointF C, PointF D, out PointF intersection)
        {
            intersection = new PointF(float.NaN, float.NaN);

            float a1 = B.Y - A.Y;
            float b1 = A.X - B.X;
            float c1 = a1 * A.X + b1 * A.Y;

            float a2 = D.Y - C.Y;
            float b2 = C.X - D.X;
            float c2 = a2 * C.X + b2 * C.Y;

            float det = a1 * b2 - a2 * b1;

            if (det.Equals(0.0))
                return false;

            float x = (b2 * c1 - b1 * c2) / det;
            float y = (a1 * c2 - a2 * c1) / det;

            PointF point = new PointF { X = x, Y = y };

            if (!IsInsideOfBounds(C, D, point))
                return false;

            intersection = point;
            return true;
        }

        private static float Min (float left, float right)
        {
            return left < right ? left : right;
        }
        private static float Max(float left, float right)
        {
            return left > right ? left : right;
        }

        private static bool IsInsideOfBounds(PointF C, PointF D, PointF intersection)
        {
            var isInsideXBounds = intersection.X >= Min(C.X, D.X) && intersection.X <= Max(C.X, D.X);
            var isInsideYBounds = intersection.Y >= Min(C.Y, D.Y) && intersection.Y <= Max(C.Y, D.Y);

            return isInsideXBounds && isInsideYBounds;
        }

        [TestMethod]
        public void Test1() // angled at beginning
        {
            var p1 = new PointF { X = 0, Y = 0 };
            var p2 = new PointF { X = 20, Y = 20 };
            var p3 = new PointF { X = 0, Y = 20 };
            var p4 = new PointF { X = 20, Y = 0 };

            Assert.IsTrue(TryGetPointOfIntersection(p1, p2, p3, p4, out var result, out var angle));
            Assert.AreEqual(10, result.X, 0.1);
            Assert.AreEqual(10, result.Y, 0.1);
            Assert.AreEqual(45, angle, 0.01);
        }

        [TestMethod]
        public void Test2() // angled not zero realted
        {
            var p1 = new PointF { X = 20, Y = 20 };
            var p2 = new PointF { X = 40, Y = 40 };
            var p3 = new PointF { X = 20, Y = 40 };
            var p4 = new PointF { X = 40, Y = 20 };

            Assert.IsTrue(TryGetPointOfIntersection(p1, p2, p3, p4, out var result, out var angle));
            Assert.AreEqual(30, result.X, 0.1);
            Assert.AreEqual(30, result.Y, 0.1);
            Assert.AreEqual(45, angle, 0.01);
        }

        [TestMethod]
        public void Test3() // angled and horizontal
        {
            var p1 = new PointF { X = 30, Y = 30 };
            var p2 = new PointF { X = 40, Y = 30 };
            var p3 = new PointF { X = 20, Y = 40 };
            var p4 = new PointF { X = 40, Y = 20 };

            Assert.IsTrue(TryGetPointOfIntersection(p1, p2, p3, p4, out var result, out var angle));
            Assert.AreEqual(30, result.X, 0.1);
            Assert.AreEqual(30, result.Y, 0.1);
            Assert.AreEqual(45, angle, 0.01);
        }


        [TestMethod]
        public void Test4() // angled and vertical
        {
            var p1 = new PointF { X = 20, Y = 20 };
            var p2 = new PointF { X = 20, Y = 50 };
            var p3 = new PointF { X = 30, Y = 20 };
            var p4 = new PointF { X = 10, Y = 50 };

            Assert.IsTrue(TryGetPointOfIntersection(p1, p2, p3, p4, out var result, out var angle));
            Assert.AreEqual(20, result.X, 0.1);
            Assert.AreEqual(35, result.Y, 0.1);
            Assert.AreEqual(56.3, angle, 0.01);
        }

        [TestMethod]
        public void Test5() // horizontal and vertical
        {
            var p1 = new PointF { X = 20, Y = 20 };
            var p2 = new PointF { X = 40, Y = 20 };
            var p3 = new PointF { X = 30, Y = 10 };
            var p4 = new PointF { X = 30, Y = 30 };

            Assert.IsTrue(TryGetPointOfIntersection(p1, p2, p3, p4, out var result));
            Assert.AreEqual(30, result.X, 0.1);
            Assert.AreEqual(20, result.Y, 0.1);
        }

        [TestMethod]
        public void Test6() // all coordinates equal
        {
            var p1 = new PointF { X = 20, Y = 20 };
            var p2 = new PointF { X = 20, Y = 20 };
            var p3 = new PointF { X = 20, Y = 20 };
            var p4 = new PointF { X = 20, Y = 20 };

            Assert.IsFalse(TryGetPointOfIntersection(p1, p2, p3, p4, out var result));
            Assert.AreEqual(float.NaN, result.X);
            Assert.AreEqual(float.NaN, result.Y);
        }

        [TestMethod]
        public void Test7() // lines are the same
        {
            var p1 = new PointF { X = 20, Y = 20 };
            var p2 = new PointF { X = 40, Y = 40 };
            var p3 = new PointF { X = 40, Y = 40 };
            var p4 = new PointF { X = 20, Y = 20 };

            Assert.IsFalse(TryGetPointOfIntersection(p1, p2, p3, p4, out var result));
            Assert.AreEqual(float.NaN, result.X);
            Assert.AreEqual(float.NaN, result.Y);
        }

        [TestMethod]
        public void Test8() // no actual intersection
        {
            var p1 = new PointF { X = 20, Y = 20 };
            var p2 = new PointF { X = 40, Y = 40 };
            var p3 = new PointF { X = 10, Y = 50 };
            var p4 = new PointF { X = 20, Y = 40 };

            Assert.IsFalse(TryGetPointOfIntersection(p1, p2, p3, p4, out var result));
            Assert.AreEqual(float.NaN, result.X);
            Assert.AreEqual(float.NaN, result.Y);
        }

        [TestMethod]
        public void Test9() // intersection, angle 0
        {
            var p1 = new PointF { X = 20, Y = 20 };
            var p2 = new PointF { X = 40, Y = 40 };
            var p3 = new PointF { X = 30, Y = 30 };
            var p4 = new PointF { X = 40, Y = 30 };

            Assert.IsTrue(TryGetPointOfIntersection(p1, p2, p3, p4, out var result, out var angle));
            Assert.AreEqual(30, result.X);
            Assert.AreEqual(30, result.Y);
            Assert.AreEqual(0, angle);
        }

        [TestMethod]
        public void Test10() // intersection angle 90
        {
            var p1 = new PointF { X = 20, Y = 20 };
            var p2 = new PointF { X = 40, Y = 40 };
            var p3 = new PointF { X = 30, Y = 30 };
            var p4 = new PointF { X = 30, Y = 0 };

            Assert.IsTrue(TryGetPointOfIntersection(p1, p2, p3, p4, out var result, out var angle));
            Assert.AreEqual(30, result.X);
            Assert.AreEqual(30, result.Y);
            Assert.AreEqual(90, angle);
        }

        [TestMethod]
        public void Test11() // angled and horizontal and sector
        {
            var p1 = new PointF { X = 30, Y = 30 };
            var p2 = new PointF { X = 40, Y = 30 };
            var p3 = new PointF { X = 20, Y = 20 };
            var p4 = new PointF { X = 40, Y = 40 };

            Assert.IsTrue(TryGetPointOfIntersection(p1, p2, p3, p4, out var result, out var angle, out var sector));
            Assert.AreEqual(30, result.X, 0.1);
            Assert.AreEqual(30, result.Y, 0.1);
            Assert.AreEqual(315, angle, 0.01);
            Assert.AreEqual(4, sector);
        }
        [TestMethod]
        public void Test12() // angled and horizontal and sector
        {
            var p1 = new PointF { X = 30, Y = 30 };
            var p2 = new PointF { X = 40, Y = 30 };
            var p3 = new PointF { X = 40, Y = 40 };
            var p4 = new PointF { X = 20, Y = 20 };

            Assert.IsTrue(TryGetPointOfIntersection(p1, p2, p3, p4, out var result, out var angle, out var sector));
            Assert.AreEqual(30, result.X, 0.1);
            Assert.AreEqual(30, result.Y, 0.1);
            Assert.AreEqual(135, angle, 0.01);
            Assert.AreEqual(2, sector);
        }
        [TestMethod]
        public void Test13() // angled and horizontal and sector
        {
            var p1 = new PointF { X = 30, Y = 30 };
            var p2 = new PointF { X = 40, Y = 30 };
            var p3 = new PointF { X = 20, Y = 40 };
            var p4 = new PointF { X = 40, Y = 20 };

            Assert.IsTrue(TryGetPointOfIntersection(p1, p2, p3, p4, out var result, out var angle, out var sector));
            Assert.AreEqual(30, result.X, 0.1);
            Assert.AreEqual(30, result.Y, 0.1);
            Assert.AreEqual(45, angle, 0.01);
            Assert.AreEqual(1, sector);
        }
        [TestMethod]
        public void Test14() // angled and horizontal and sector
        {
            var p1 = new PointF { X = 30, Y = 30 };
            var p2 = new PointF { X = 40, Y = 30 };
            var p3 = new PointF { X = 40, Y = 20 };
            var p4 = new PointF { X = 20, Y = 40 };

            Assert.IsTrue(TryGetPointOfIntersection(p1, p2, p3, p4, out var result, out var angle, out var sector));
            Assert.AreEqual(30, result.X, 0.1);
            Assert.AreEqual(30, result.Y, 0.1);
            Assert.AreEqual(225, angle, 0.01);
            Assert.AreEqual(3, sector);
        }

        [TestMethod]
        public void Test15() // Pythagorean Theorem test
        {
            var p1 = new PointF { X = 30, Y = 30 };
            var p2 = new PointF { X = 40, Y = 30 };
            var p3 = new PointF { X = 40, Y = 20 };
            var p4 = new PointF { X = 20, Y = 40 };

            Assert.IsTrue(TryGetPointOfIntersection(p1, p2, p3, p4, out var result, out var angle, out var sector));
            Assert.AreEqual(30, result.X, 0.1);
            Assert.AreEqual(30, result.Y, 0.1);
            Assert.AreEqual(225, angle, 0.01);
            Assert.AreEqual(3, sector);

            var points = PythagoreanTheorem(p3, result, 5, 10); // before, after

            Assert.AreEqual(6.46, points.Item1.X, 0.1);
            Assert.AreEqual(6.46, points.Item1.Y, 0.1);
        }

        [TestMethod]
        public void Test16() // Pythagorean Theorem test
        {
            var p1 = new PointF { X = 30, Y = 30 };
            var p2 = new PointF { X = 40, Y = 30 };
            var p3 = new PointF { X = 40, Y = 40 };
            var p4 = new PointF { X = 20, Y = 20 };

            Assert.IsTrue(TryGetPointOfIntersection(p1, p2, p3, p4, out var result, out var angle, out var sector));
            Assert.AreEqual(30, result.X, 0.1);
            Assert.AreEqual(30, result.Y, 0.1);
            Assert.AreEqual(135, angle, 0.01);
            Assert.AreEqual(2, sector);

            var points = PythagoreanTheorem(p3, result, 5, 10); // before, after

            Assert.AreEqual(6.46, points.Item1.X, 0.1);
            Assert.AreEqual(6.46, points.Item1.Y, 0.1);
        }
        [TestMethod]
        public void Test17() // Pythagorean Theorem test
        {
            var p1 = new PointF { X = 30, Y = 30 };
            var p2 = new PointF { X = 40, Y = 30 };
            var p3 = new PointF { X = 20, Y = 20 };
            var p4 = new PointF { X = 40, Y = 40 };

            Assert.IsTrue(TryGetPointOfIntersection(p1, p2, p3, p4, out var result, out var angle, out var sector));
            Assert.AreEqual(30, result.X, 0.1);
            Assert.AreEqual(30, result.Y, 0.1);
            Assert.AreEqual(315, angle, 0.01);
            Assert.AreEqual(4, sector);

            var points = PythagoreanTheorem(p3, result, 5, 10); // before, after

            Assert.AreEqual(6.46, points.Item1.X, 0.1);
            Assert.AreEqual(6.46, points.Item1.Y, 0.1);

            Assert.AreEqual(17.07, points.Item2.X, 0.1);
            Assert.AreEqual(17.07, points.Item2.Y, 0.1);
        }
        [TestMethod]
        public void Test18() // Pythagorean Theorem test
        {
            var p1 = new PointF { X = 30, Y = 30 };
            var p2 = new PointF { X = 40, Y = 30 };
            var p3 = new PointF { X = 20, Y = 20 };
            var p4 = new PointF { X = 40, Y = 40 };

            Assert.IsTrue(TryGetPointOfIntersection(p1, p2, p3, p4, out var result, out var angle, out var sector));
            Assert.AreEqual(30, result.X, 0.1);
            Assert.AreEqual(30, result.Y, 0.1);
            Assert.AreEqual(315, angle, 0.01);
            Assert.AreEqual(4, sector);

            var points = PythagoreanTheorem(p3, result, 5, 10, sector); // before, after

            Assert.AreEqual(2.92, points.Item1.X, 0.1);
            Assert.AreEqual(2.92, points.Item1.Y, 0.1);

            Assert.AreEqual(13.54, points.Item2.X, 0.1);
            Assert.AreEqual(13.54, points.Item2.Y, 0.1);
        }
    }
}
