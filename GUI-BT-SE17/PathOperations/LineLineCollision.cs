using System;

namespace Svg.Path.Operations
{
    public static class LineLineCollision
    {
        public struct collisionData
        {
            public Double beforeX;
            public Double beforeY;
            public Double centerX;
            public Double centerY;
            public Double afterX;
            public Double afterY;
            public Double alpha;

        }

        private static double calculateCoordinate(double hypotenuseA, double xOrYvalue, double hypotenuseB)
        {
            return (xOrYvalue * hypotenuseB) / hypotenuseA;
        }

        private static double convertRadianToDegree(double radianValue)
        {
            return radianValue * 180 / Math.PI;
        }

        private static collisionData applyInterceptionTheorem(ref Tuple<double, double> lineStart, ref Tuple<double, double> lineEnd, double deltaDistanceA, double deltaDistanceB)
        {
            collisionData points = new collisionData();

            double x0 = Math.Abs(lineStart.Item1 - lineEnd.Item1);
            double y0 = Math.Abs(lineStart.Item2 - lineEnd.Item2);
            double v0 = Math.Sqrt(Math.Pow(x0, 2) + Math.Pow(y0, 2));

            double distA = v0 - deltaDistanceA;
            double distB = v0 + deltaDistanceB;


            points.beforeX = calculateCoordinate(v0, x0, distA);
            points.beforeY = calculateCoordinate(v0, y0, distA);

            points.afterX = calculateCoordinate(v0, x0, distB);
            points.afterY = calculateCoordinate(v0, y0, distB);

            points.alpha = convertRadianToDegree(Math.Asin(y0 / v0));

            return points;
        }

        public static bool LineIsHorizontal(ref Tuple<double, double> lineStart, ref Tuple<double, double> lineEnd)
        {
            return lineStart.Item1 == lineEnd.Item1;
        }

        public static bool LineIsVertical(ref Tuple<double, double> lineStart, ref Tuple<double, double> lineEnd)
        {
            return lineStart.Item2 == lineEnd.Item2;
        }

        public static int GetSectorOfLine(ref Tuple<double, double> lineStart, ref Tuple<double, double> lineEnd)
        {
            double x = lineStart.Item1 - lineEnd.Item1;
            double y = lineStart.Item2 - lineEnd.Item2;

            if ( x > 0)
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



        public static collisionData getLineIntersectionPoint(ref Tuple<double, double> line1Start, ref Tuple<double, double> line1End,
                                          ref Tuple<double, double> line2Start, ref Tuple<double, double> line2End, double deltaDistanceA, double DeltaDistanceB)
        {
            collisionData points = new collisionData();

            double A1 = line1End.Item2 - line1Start.Item2;
            double B1 = line1Start.Item1 - line1End.Item1;
            double C1 = A1 * line1Start.Item1 + B1 * line1Start.Item2;

            double A2 = line2End.Item2 - line2Start.Item2;
            double B2 = line2Start.Item1 - line2End.Item1;
            double C2 = A1 * line2Start.Item1 + B1 * line2Start.Item2;

            double det = A1 * B2 - A2 * B1;

            if (!(det < 0.0) && !(det > 0.0))
            {
                //return null;
            }

            double x = (B2 * C1 - B1 * C2) / det;
            double y = (A1 * C2 - A2 * C1) / det;

            Tuple<double, double> newLineEnd = new Tuple<double, double>(x, y);

            points = applyInterceptionTheorem(ref line1Start, ref newLineEnd, deltaDistanceA, DeltaDistanceB);

            points.centerX = x;
            points.centerY = y;

            return points;
        }
    }
}
