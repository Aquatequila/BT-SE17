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

        private PointF GetPoint(PointF reference, int sector, int distance, DeltaUnit unit, bool calculatePointBefore = false)
        {
            var deltaX = (float) unit.X * distance;
            var deltaY = (float) unit.Y * distance;

            if (calculatePointBefore)
            {
                deltaX *= -1;
                deltaY *= -1;
            }

            var result = new PointF();

            switch (sector)
            {
                case 1:
                    {
                        result.X = reference.X + deltaX;
                        result.Y = reference.Y - deltaY;
                        break;
                    }
                case 2:
                    {
                        result.X = reference.X - deltaX;
                        result.Y = reference.Y - deltaY;
                        break;
                    }
                case 3:
                    {
                        result.X = reference.X - deltaX;
                        result.Y = reference.Y + deltaY;
                        break;
                    }
                case 4:
                    {
                        result.X = reference.X + deltaX;
                        result.Y = reference.Y + deltaY;
                        break;
                    }
            }
            return result;
        }
        private struct DeltaUnit
        {
            public Double X;
            public Double Y;
        }
        private static Double CalculateDistance(PointF start, PointF end)
        {
            return Math.Sqrt(Math.Pow(end.X - start.X, 2) + Math.Pow(end.Y - start.Y,2));
        }
        private static DeltaUnit GetDeltaUnit (PointF start, PointF end)
        {
            var result = new DeltaUnit();
            var distance = CalculateDistance(start, end);

            result.X = Math.Abs((end.X - start.X)) / distance;
            result.Y = Math.Abs((end.Y - start.Y)) / distance;

            return result;
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
        private static bool IsInsideOfBounds(PointF C, PointF D, PointF toCheck)
        {
            var isInsideXBounds = toCheck.X >= Min(C.X, D.X) && toCheck.X <= Max(C.X, D.X);
            var isInsideYBounds = toCheck.Y >= Min(C.Y, D.Y) && toCheck.Y <= Max(C.Y, D.Y);

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

            //var points = PythagoreanTheorem(p3, result, 5, 10); // before, after
            //Assert.AreEqual(6.46, points.Item1.X, 0.1);
            //Assert.AreEqual(6.46, points.Item1.Y, 0.1);
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

            //var points = PythagoreanTheorem(p3, result, 5, 10); // before, after
            //Assert.AreEqual(6.46, points.Item1.X, 0.1);
            //Assert.AreEqual(6.46, points.Item1.Y, 0.1);
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

            //var points = PythagoreanTheorem(p3, result, 5, 10); // before, after

            //Assert.AreEqual(6.46, points.Item1.X, 0.1);
            //Assert.AreEqual(6.46, points.Item1.Y, 0.1);

            //Assert.AreEqual(17.07, points.Item2.X, 0.1);
            //Assert.AreEqual(17.07, points.Item2.Y, 0.1);
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

            //Assert.IsTrue(PythagoreanTheorem(p3, result, 5, 10, sector, out var points)); // before, after

            //Assert.AreEqual(2.92, points.Item1.X, 0.1);
            //Assert.AreEqual(2.92, points.Item1.Y, 0.1);

            //Assert.AreEqual(13.54, points.Item2.X, 0.1);
            //Assert.AreEqual(13.54, points.Item2.Y, 0.1);
        }

        [TestMethod]
        public void Test19() // Pythagorean Theorem test
        {
            var p1 = new PointF { X = 0, Y = 0 };
            var p2 = new PointF { X = 100, Y = 100 };
            var p3 = new PointF { X = 0, Y = 100 };
            var p4 = new PointF { X = 100, Y = 0 };

            Assert.IsTrue(TryGetPointOfIntersection(p1, p2, p3, p4, out var result, out var angle, out var sector));
            Assert.AreEqual(50, result.X, 0.1);
            Assert.AreEqual(50, result.Y, 0.1);
            Assert.AreEqual(45, angle, 0.01);
            Assert.AreEqual(1, sector);


            var unit = GetDeltaUnit(p3, p4);

            var before = GetPoint(result, sector, 10, unit, calculatePointBefore: true);
            Assert.AreEqual(42.9, before.X, 0.1);
            Assert.AreEqual(57, before.Y, 0.1);
            Assert.IsTrue(IsInsideOfBounds(p3, p4, before));

            var after = GetPoint(result, sector, 10, unit, calculatePointBefore: false);
            Assert.AreEqual(57, after.X, 0.1);
            Assert.AreEqual(42.9, after.Y, 0.1);
            Assert.IsTrue(IsInsideOfBounds(p3, p4, after));

            after = GetPoint(result, sector, 200, unit, calculatePointBefore: false);
            Assert.IsFalse(IsInsideOfBounds(p3, p4, after));

            before = GetPoint(result, sector, 200, unit, calculatePointBefore: true);
            Assert.IsFalse(IsInsideOfBounds(p3, p4, before));
        }

        [TestMethod]
        public void Test20() // Pythagorean Theorem test
        {
            var p1 = new PointF { X = 30, Y = 20 };
            var p2 = new PointF { X = 60, Y = 40 };
            var p3 = new PointF { X = 20, Y = 80 };
            var p4 = new PointF { X = 50, Y = 20 };

            Assert.IsTrue(TryGetPointOfIntersection(p1, p2, p3, p4, out var result, out var angle, out var sector));
            Assert.AreEqual(45, result.X, 0.1);
            Assert.AreEqual(30, result.Y, 0.1);
            Assert.AreEqual(63.43, angle, 0.01);
            Assert.AreEqual(1, sector);
            
            var unitA = GetDeltaUnit(p1, p2);
            var unitB = GetDeltaUnit(p3, p2);

            var before = GetPoint(result, 4, 10, unitA, calculatePointBefore: true);
            Assert.IsTrue(IsInsideOfBounds(p1, p2, before));

            var after = GetPoint(result, 4, 10, unitA, calculatePointBefore: false);
            Assert.IsTrue(IsInsideOfBounds(p1, p2, after));
        }
        [TestMethod]
        public void Test30() // Pythagorean Theorem test
        {
            var p1 = new PointF { X = 20, Y = 20 };
            var p2 = new PointF { X = 120, Y = 20 };
            var result = new PointF { X = 80, Y = 20 };

            var unitA = GetDeltaUnit(p1, p2);

            var before = GetPoint(result, 1, 10, unitA, calculatePointBefore: true);
            Assert.IsTrue(IsInsideOfBounds(p1, p2, before));

            var after = GetPoint(result, 1, 10, unitA, calculatePointBefore: false);
            Assert.IsTrue(IsInsideOfBounds(p1, p2, after));
        }

        [TestMethod]
        public void Test31() // Pythagorean Theorem test
        {
            var p1 = new PointF { X = 20, Y = 20 };
            var p2 = new PointF { X = 20, Y = 120 };
            var result = new PointF { X = 20, Y = 60 };

            var unitA = GetDeltaUnit(p1, p2);

            var before = GetPoint(result, 4, 10, unitA, calculatePointBefore: true);
            Assert.IsTrue(IsInsideOfBounds(p1, p2, before));

            var after = GetPoint(result, 4, 10, unitA, calculatePointBefore: false);
            Assert.IsTrue(IsInsideOfBounds(p1, p2, after));
        }

        [TestMethod]
        public void Test32() // Pythagorean Theorem test
        {
            var p1 = new PointF { X = 20, Y = 120 };
            var p2 = new PointF { X = 20, Y = 20 };
            var p3 = new PointF { X = 100, Y = 60 };
            var p4 = new PointF { X = 0, Y = 60 };

            Assert.IsTrue(TryGetPointOfIntersection(p3, p4, p1, p2, out var result, out var angle, out var sector));

            var unitA = GetDeltaUnit(p1, p2);

            var before = GetPoint(result, 4, 10, unitA, calculatePointBefore: true);
            Assert.IsTrue(IsInsideOfBounds(p1, p2, before));

            var after = GetPoint(result, 4, 10, unitA, calculatePointBefore: false);
            Assert.IsTrue(IsInsideOfBounds(p1, p2, after));
        }



    }
}
