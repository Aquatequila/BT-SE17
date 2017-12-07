using System;
using System.Drawing;

namespace Svg.Path.Operations
{
    public static class LineInterception
    {
        private static double ToDegrees(double radianValue)
        {
            return radianValue * 180 / Math.PI;
        }
        private static double GetAngle(PointF p1, PointF p2)
        {
            float xAbs = Max(p1.X, p2.X) - Min(p1.X, p2.X);
            float yAbs = Max(p1.Y, p2.Y) - Min(p1.Y, p2.Y);
            float hypotenuse = (float)Math.Sqrt(Math.Pow(xAbs, 2) + Math.Pow(yAbs, 2));

            return ToDegrees(Math.Asin(yAbs / hypotenuse));
        }

        private static PointF GetPoint(PointF reference, int sector, int distance, DeltaUnit unit, bool calculatePointBefore = false)
        {
            var deltaX = (float)unit.X * distance;
            var deltaY = (float)unit.Y * distance;

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
            return Math.Sqrt(Math.Pow(end.X - start.X, 2) + Math.Pow(end.Y - start.Y, 2));
        }
        private static DeltaUnit GetDeltaUnit(PointF start, PointF end)
        {
            var result = new DeltaUnit();
            var distance = CalculateDistance(start, end);

            result.X = Math.Abs((end.X - start.X)) / distance;
            result.Y = Math.Abs((end.Y - start.Y)) / distance;

            return result;
        }

        private static int GetSector(PointF point1, PointF point2)
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

        private static float Min(float left, float right)
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
    }
}
