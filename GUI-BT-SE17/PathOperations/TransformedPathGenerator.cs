﻿using Svg.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Svg.Path.Operations
{
    public static class TransformedPathGenerator
    {
        public static SvgElement RotatePathByDegrees(SvgElement svg, double degrees, SvgCommand center)
        {
            var returnValue = svg;
            for (var i = 0; i < returnValue.Path.Count; i++)
            {
                returnValue.Path[i] = NormalizePoint(returnValue.Path[i], center);

                returnValue.Path[i] = RotatePointToDegrees(returnValue.Path[i], degrees, center);

                returnValue.Path[i] = DenormalizePoint(returnValue.Path[i], center);
            }
            return returnValue;
        }

        private static System.Windows.Point GetCenterPoint(BoundingRectangle box)
        {
            double x = (box.maxX - box.minX) / 2 + box.minX;
            double y = (box.maxY - box.minY) / 2 + box.minY;

            return new System.Windows.Point(x, y);

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

        public static BoundingRectangle CalculateBoundingRectangle(IList<SvgElement> svgs)
        {
            var boundingRectangle = new BoundingRectangle();
            var notInitialized = true;

            foreach (var svg in svgs)
            {
                foreach (var Point in svg.Path)
                {
                    if (Point.type != PointType.Z && Point.type != PointType.z)
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
                }
            }

            return boundingRectangle;
        }

        public static BoundingRectangle CalculateBoundingRectangle(SvgElement svg)
        {
            var boundingRectangle = new BoundingRectangle();
            var notInitialized = true;

            foreach (var Point in svg.Path)
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
            point.x = point.x - center.x;
            point.y = point.y - center.y;
            point.x1 = point.x1 - center.x;
            point.y1 = point.y1 - center.y;
            point.rx = point.rx - center.x;
            point.ry = point.ry - center.y;

            return point;
        }

        private static SvgCommand DenormalizePoint(SvgCommand point, SvgCommand center)
        {
            point.x = point.x + center.x;
            point.y = point.y + center.y;
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
            point.x = Math.Round(point.x * Math.Cos(degrees), 2) - Math.Round(point.y * Math.Sin(degrees), 2);
            point.y = Math.Round(oldVal * Math.Sin(degrees), 2) + Math.Round(point.y * Math.Cos(degrees), 2);

            //Console.WriteLine(point);

            point.x1 = point.x1 * Math.Cos(degrees) - point.y1 * Math.Sin(degrees + 180);
            point.y1 = point.x1 * Math.Sin(degrees) + point.y1 * Math.Cos(degrees + 180);
            point.rx = point.rx * Math.Cos(degrees) - point.ry * Math.Sin(degrees + 180);
            point.ry = point.rx * Math.Sin(degrees) + point.ry * Math.Cos(degrees + 180);



            return point;
        }

        private static SvgCommand ScalePoint(SvgCommand point, double amount)
        {
            point.x *= amount;
            point.y *= amount;
            point.rx *= amount;
            point.ry *= amount;
            point.x1 *= amount;
            point.y1 *= amount;

            return point;
        }

        private static SvgCommand ScalePoint(SvgCommand point, double xFactor, double yFactor)
        {
            point.x *= xFactor;
            point.y *= yFactor;
            point.rx *= xFactor;
            point.ry *= yFactor;
            point.x1 *= xFactor;
            point.y1 *= yFactor;

            return point;
        }
        public static SvgElement ScalePath(SvgElement svg, double xFactor, double yFactor)
        {
            SvgElement returnValue = svg;
            for (var i = 0; i < returnValue.Path.Count; i++)
            {
                returnValue.Path[i] = ScalePoint(returnValue.Path[i], xFactor, yFactor);
            }
            return returnValue;
        }
        // ref problem => scale not visible to outside
        public static SvgElement ScalePath(SvgElement svg, double amount)
        {
            SvgElement returnValue = svg;
            for (var i = 0; i < returnValue.Path.Count; i++)
            {
                returnValue.Path[i] = ScalePoint(returnValue.Path[i], amount);
            }
            return returnValue;
        }

        public static List<SvgElement> ScaleBy(List<SvgElement> svgs, double xFactor, double yFactor)
        {
            var returnValue = new List<SvgElement>();

            foreach (var svg in svgs)
            {
                returnValue.Add(ScalePath(svg, xFactor, yFactor));
            }
            return returnValue;
        }

        public static List<SvgElement> ScaleBy(List<SvgElement> svgs, double amount)
        {
            var returnValue = new List<SvgElement>();

            foreach(var svg in svgs)
            {
                returnValue.Add(ScalePath(svg, amount));
            }
            return returnValue;
        }

        private static SvgCommand TranslatePoint(SvgCommand point, double deltaX, double deltaY)
        {
            point.x += deltaX;
            point.y += deltaY;
            point.rx += deltaX;
            point.ry += deltaY;
            point.x1 += deltaX;
            point.y1 += deltaY;

            return point;
        }

        

        public static SvgElement TranslatePath(SvgElement svg, double deltaX, double deltaY)
        {
            var returnValue = svg;
            for (var i = 0; i < returnValue.Path.Count; i++)
            {
                returnValue.Path[i] = TranslatePoint(returnValue.Path[i], deltaX, deltaY);
            }
            return returnValue;
        }

        public static List<SvgElement> TranslateBy(List<SvgElement> svgs, double deltaX, double deltaY)
        {
            var result = new List<SvgElement>();
            foreach (var svg in svgs)
            {
                result.Add(TranslatePath(svg, deltaX, deltaY));
            }
            return result;
        }
    } 
}
