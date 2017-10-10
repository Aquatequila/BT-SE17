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
        public Point RotatePointAroundCenter(Point center, Point point, double degrees)
        {
            int sector;
            double radius = TheoremOfPythagoras(center, point, out sector);

            return new Point();
        }
        public double TheoremOfPythagoras(Point center, Point point, out int sector)
        {
            double x = point.X  - center.X;
            double y = center.Y - point.Y;

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
            return 1;
        }
        private double SquareAndSum(double x, double y)
        {
            return Math.Pow(x, 2) + Math.Pow(y, 2);
        }
    }
}
