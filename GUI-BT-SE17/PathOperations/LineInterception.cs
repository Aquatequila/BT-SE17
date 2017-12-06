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

        private static float CalculateCoordinate(double hypotenuseA, double xOrYvalue, double hypotenuseB)
        {
            return (float)((xOrYvalue * hypotenuseB) / hypotenuseA);
        }

        public static Tuple<PointF, PointF> PythagoreanTheorem(PointF point1, PointF point2, double deltaDistanceA, double deltaDistanceB, int sector)
        {
            if ((sector == 3) || (sector == 4))
            {
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

            return new Tuple<PointF, PointF>(before, after);
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

        public static bool TryGetPointOfIntersection(PointF A, PointF B, PointF C, PointF D, out PointF intersection, out Double angle, out int sector)
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

        private static bool IsInsideOfBounds(PointF C, PointF D, PointF intersection)
        {
            var isInsideXBounds = intersection.X >= Min(C.X, D.X) && intersection.X <= Max(C.X, D.X);
            var isInsideYBounds = intersection.Y >= Min(C.Y, D.Y) && intersection.Y <= Max(C.Y, D.Y);

            return isInsideXBounds && isInsideYBounds;
        }
    }
}
