using Svg.Wrapper;
using System;
using System.Drawing;

namespace Svg.Path.Operations
{
    public static class Interceptor
    {
        public struct CollisionData
        {
            public Double beforeX;
            public Double beforeY;
            public Double centerX;
            public Double centerY;
            public Double afterX;
            public Double afterY;
            public Double alpha;

            public override String ToString()
            {
                return $"x:{centerX}, y:{centerY}, angle:{alpha}";
            }

        }

        public static bool LineIsHorizontal(SvgCommand point1, SvgCommand point2)
        {
            return point1.x == point2.x;
        }

        public static bool LineIsVertical(SvgCommand point1, SvgCommand point2)
        {
            return point1.y == point2.y;
        }

        private static double ToDegrees(this double radianValue)
        {
            return radianValue * 180 / Math.PI;
        }

        public static int GetSectorOf(SvgCommand point1, SvgCommand point2)
        {
            var x = point1.x - point2.x;
            var y = point1.y - point2.y;

            if (x > 0)
            {
                if (y > 0)
                {
                    return 1;
                }
                else
                {
                    return 4;
                }
            }
            else
            {
                if (y > 0)
                {
                    return 2;
                }
                else
                {
                    return 3;
                }
            }
        }

        private static double CalculateCoordinate(double hypotenuseA, double xOrYvalue, double hypotenuseB)
        {
            return (xOrYvalue * hypotenuseB) / hypotenuseA;
        }

        private static CollisionData ApplyInterceptionTheorem(SvgCommand point1, SvgCommand point2, double deltaDistanceA, double deltaDistanceB)
        {
            CollisionData data = new CollisionData();

            var x0 = Math.Abs(point1.x - point2.x);
            var y0 = Math.Abs(point1.y - point2.y);
            var v0 = Math.Sqrt(Math.Pow(x0, 2) + Math.Pow(y0, 2));

            var distA = v0 - deltaDistanceA;
            var distB = v0 + deltaDistanceB;

            data.beforeX = CalculateCoordinate(v0, x0, distA);
            data.beforeY = CalculateCoordinate(v0, y0, distA);

            data.afterX = CalculateCoordinate(v0, x0, distB);
            data.afterY = CalculateCoordinate(v0, y0, distB);

            data.alpha = Math.Asin(y0 / v0).ToDegrees();

            return data;
        }

        struct MyPoint
        {
            public Double first;
            public Double second;
        }

        public static bool TryGetLineIntersectionPointV2(SvgCommand line1Start, SvgCommand line1End,
                                          SvgCommand line2Start, SvgCommand line2End, out CollisionData data)
        {
            var A = new MyPoint { first = line1Start.x, second = line1Start.y };
            var B = new MyPoint { first = line1End.x, second = line1End.y };
            var C = new MyPoint { first = line2Start.x, second = line2Start.y };
            var D = new MyPoint { first = line2End.x, second = line2End.y };

            data = new CollisionData();

            var a1 = B.second - A.second;
            var b1 = A.first - B.first;
            var c1 = a1 * (A.first) + b1 * (A.second);

            var a2 = D.second - C.second;
            var b2 = C.first - C.first;
            var c2 = a1 * (C.first) + b2 * (C.second);

            var determinant = a1 * b2 - a2 * b1;

            if (determinant.Equals(0.0))
            {
                return false;
            }

            var x = (b2 * c1 - b1 * c2) / determinant;
            var y = (a1 * c2 - a2 * c1) / determinant;

            if (Double.IsInfinity(x) || Double.IsInfinity(y))
                return false;

            //var newLineEnd = new SvgCommand { x = x, y = y };
            //data = ApplyInterceptionTheorem(line1Start, newLineEnd, deltaDistanceA, DeltaDistanceB);

            data.centerX = x;
            data.centerY = y;

            return true;
        }

        public static bool TryGetLineIntersectionPoint(SvgCommand line1Start, SvgCommand line1End,
                                          SvgCommand line2Start, SvgCommand line2End, double deltaDistanceA, double DeltaDistanceB, out CollisionData data) 
        {
            data = new CollisionData();

            double A1 = line1End.y - line1Start.y;
            double B1 = line1Start.x - line1End.x;
            double C1 = A1 * line1Start.x + B1 * line1Start.y;

            double A2 = line2End.y - line2Start.y;
            double B2 = line2Start.x - line2End.x;
            double C2 = A1 * line2Start.x + B1 * line2Start.y;

            double determinant = A1 * B2 - A2 * B1;

            if (!(determinant < 0.0) && !(determinant > 0.0))
            {
                return false;
            }

            double x = (B2 * C1 - B1 * C2) / determinant;
            double y = (A1 * C2 - A2 * C1) / determinant;

            var newLineEnd = new SvgCommand { x = x, y = y };
            data = ApplyInterceptionTheorem(line1Start, newLineEnd, deltaDistanceA, DeltaDistanceB);

            data.centerX = x;
            data.centerY = y;

            return true;
        }
    }
}
