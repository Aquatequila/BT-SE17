using Svg.Wrapper;
using System;

namespace Svg.Path.Operations
{
    public struct BoundingRectangle
    {
        public double minX;
        public double maxX;
        public double minY;
        public double maxY;

        public void Initialize(double x, double y)
        {
            maxX = x;
            minX = x;
            minY = y;
            maxY = y;
        }
        public void Set(double x, double y)
        {
            maxX = maxX < x ? x : maxX;
            minX = minX < x ? minX : x;

            maxY = maxY < y ? y : maxY;
            minY = minY < y ? minY : y;
        }
    }

    public static class PathMatrixOperation
    {
        public static void RotatePathToDegrees(ref SvgElement svg, double degrees, SvgCommand center)
        {
            for (var i = 0; i < svg.Path.Count; i++)
            {
                svg.Path[i] = NormalizePoint(svg.Path[i], center);

                svg.Path[i] = RotatePointToDegrees(svg.Path[i], degrees, center);

                svg.Path[i] = DenormalizePoint(svg.Path[i], center);
            }
        }

        private static System.Windows.Point GetCenterPoint(BoundingRectangle box)
        {
            double x = (box.maxX - box.minX) / 2 + box.minX;
            double y = (box.maxY - box.minY) / 2 + box.minY;

            return new System.Windows.Point(x,y);

        }

        private static double CalculateAngleBetween(SvgCommand point, SvgCommand center)
        {
            point.x -= center.x;
            point.y -= center.y;

            int xSector = point.x < 0 ? -1 : 1;
            int ySector = point.y < 0 ? -1 : 1;

            point.x = Math.Abs(point.x);
            point.y = Math.Abs(point.y);

            double hypotenuse = Math.Sqrt(Math.Pow(point.x, 2) + Math.Pow(point.y, 2));

            double angle = (180 / Math.PI) * Math.Asin(point.y / hypotenuse);

            if (xSector < 1)
            {
                if (ySector < 1) // Sector 2
                {
                    angle += 180;
                }
                else // Sector 2
                {
                    angle += 90;
                }
            }
            else
            {
                if (ySector < 1) // Sector 4
                {
                    angle += 270;
                }
            }
            Console.WriteLine($"Angle : {angle}");
            return angle;
        }

        public static BoundingRectangle CalculateBoundingRectangle(SvgElement svg)
        {
            var boundingRectangle = new BoundingRectangle();
            var notInitialized = true;

            foreach(var Point in svg.Path)
            {
                if (notInitialized)
                {
                    notInitialized = false;
                    boundingRectangle.Initialize(Point.x, Point.y);
                }
                else
                {
                    boundingRectangle.Set(Point.x, Point.y);
                }
            }

            return boundingRectangle;
        }

        private static SvgCommand NormalizePoint(SvgCommand point, SvgCommand center)
        {
            point.x  = point.x  - center.x;
            point.y  = point.y  - center.y;
            point.x1 = point.x1 - center.x;
            point.y1 = point.y1 - center.y;
            point.rx = point.rx - center.x;
            point.ry = point.ry - center.y;

            return point;
        }

        private static SvgCommand DenormalizePoint(SvgCommand point, SvgCommand center)
        {
            point.x  = point.x  + center.x;
            point.y  = point.y  + center.y;
            point.x1 = point.x1 + center.x;
            point.y1 = point.y1 + center.y;
            point.rx = point.rx + center.x;
            point.ry = point.ry + center.y;

            return point;
        }

        private static SvgCommand RotatePointToDegrees(SvgCommand point, double degrees, SvgCommand center)
        {
            degrees = degrees * Math.PI / 180;
            double oldVal;

            oldVal = point.x;
            point.x  = Math.Round(point.x * Math.Cos(degrees), 2)  - Math.Round(point.y * Math.Sin(degrees), 2);
            point.y  = Math.Round(oldVal * Math.Sin(degrees), 2)  + Math.Round(point.y * Math.Cos(degrees), 2);

            //Console.WriteLine(point);

            point.x1 = point.x1 * Math.Cos(degrees) - point.y1 * Math.Sin(degrees + 180);
            point.y1 = point.x1 * Math.Sin(degrees) + point.y1 * Math.Cos(degrees + 180);
            point.rx = point.rx * Math.Cos(degrees) - point.ry * Math.Sin(degrees + 180);
            point.ry = point.rx * Math.Sin(degrees) + point.ry * Math.Cos(degrees + 180);

            

            return point;
        }

        private static SvgCommand ScalePoint(SvgCommand point, double amount)
        {
            point.x  *= amount;
            point.y  *= amount;
            point.rx *= amount;
            point.ry *= amount;
            point.x1 *= amount;
            point.y1 *= amount;

            return point;
        }
        // ref problem => scale not visible to outside
        public static void ScalePath(ref SvgElement svg, double amount)
        {
            for (var i = 0; i < svg.Path.Count; i++)
            {
                svg.Path[i] = ScalePoint(svg.Path[i], amount);
            }
        }

        private static SvgCommand TranslatePoint(SvgCommand point, double deltaX, double deltaY)
        {
            point.x  += deltaX;
            point.y  += deltaY;
            point.rx += deltaX;
            point.ry += deltaY;
            point.x1 += deltaX;
            point.y1 += deltaY;

            return point;
        }

        public static void TranslatePath(ref SvgElement svg, double deltaX, double deltaY)
        {
            for (var i = 0; i < svg.Path.Count; i++)
            {
                svg.Path[i] = TranslatePoint(svg.Path[i], deltaX, deltaY);
            }
        }
    }
}
