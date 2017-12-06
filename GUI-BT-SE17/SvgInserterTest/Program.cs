using Svg.Path.Operations;
using Svg.Wrapper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvgInserterTest
{
    class Program
    {
        static PointF FindIntersection(PointF A, PointF B, PointF C, PointF D)
        {
            float a1 = B.Y - A.Y;
            float b1 = A.X - B.X;
            float c1 = a1 * A.X + b1 * A.Y;

            float a2 = D.Y - C.Y;
            float b2 = C.X - D.X;
            float c2 = a2 * C.X + b2 * C.Y;

            float det = a1 * b2 - a2 * b1;
            //If lines are parallel, the result will be (NaN, NaN).
            return det == 0 ? new PointF(float.NaN, float.NaN)
                : new PointF((b2 * c1 - b1 * c2) / det, (a1 * c2 - a2 * c1) / det);
        }

        static void Main(string[] args)
        {
            var p1 = new PointF { X = 0, Y = 0 };
            var p2 = new PointF { X = 20, Y = 20 };
            var p3 = new PointF { X = 0, Y = 20 };
            var p4 = new PointF { X = 20, Y = 0 };

            Console.WriteLine(FindIntersection(p1,p2,p3,p4));
        }
    }
}
