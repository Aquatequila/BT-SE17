using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GUI_BT_SE17
{
    class PointOperation
    {
        private static double ConvertRadianToDegree(double radianValue)
        {
            return radianValue * 180 / Math.PI;
        }

        private double GetAlpha(double radius, double x)
        {
            return ConvertRadianToDegree(Math.Cos(x / radius));
        }

        public Point RotatePointAroundCenter(Point center, Point point, double degrees)
        {
            int sector;
            double x, y;
            double radius = TheoremOfPythagoras(center, point, out sector, out x, out y);
            x = Math.Abs(x);
            y = Math.Abs(y);
            double alpha = GetAlpha(radius, x);
            alpha += degrees;
            int sectorChange =(int) (alpha / 90);
            alpha %= 90;

            double xNew = Math.Sin(alpha) * x;
            double yNew = Math.Cos(alpha) * y;

            // adjust to sector


            

            return new Point();
        }
        public double TheoremOfPythagoras(Point center, Point point, out int sector, out double x, out double y)
        {
            x = point.X  - center.X;
            y = center.Y - point.Y;

            sector = GetSectorOfPoint(x,y);

            return Math.Sqrt(SquareAndSum(x,y));
        }
        private int GetSectorOfPoint(double x, double y)
        {
            if (x < 0)
            {
                if (y < 0)
                {
                    return 2;
                }
                else
                {
                    return 3;
                }
            }
            else
            {
                if (y < 0)
                {
                    return 4;
                }
                else
                {
                    return 1;
                }
            }
        }
        private double SquareAndSum(double x, double y)
        {
            return Math.Pow(x, 2) + Math.Pow(y, 2);
        }
    }
}
