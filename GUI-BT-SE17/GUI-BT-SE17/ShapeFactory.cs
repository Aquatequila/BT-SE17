using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GUI_BT_SE17
{
    class ShapeFactory
    {
        public ShapeFactory() { }

        public static Shape InitShape(Color stroke, Color fill, Point position, Shape shape)
        {
            shape.Stroke = new SolidColorBrush(stroke);
            shape.Fill = new SolidColorBrush(fill);
            Canvas.SetLeft(shape, position.X);
            Canvas.SetTop(shape, position.Y);

            return shape;
        }

        public static Path NewPath(Color stroke, Color fill, Point position)
        {
            Path path = new Path();
        

            path.Stroke = new SolidColorBrush(stroke);
            //path.Fill   = new SolidColorBrush(fill);
            

            return path;
        }

        public static Rectangle NewRectangle(Color stroke, Color fill, Point position)
        {
            Rectangle rectangle = new Rectangle();
            rectangle.Stroke    = new SolidColorBrush(stroke);
            rectangle.Fill      = new SolidColorBrush(fill);
            Canvas.SetLeft(rectangle, position.X);
            Canvas.SetTop (rectangle, position.Y);

            return rectangle;
        }

        public static Ellipse NewEllipse(Color stroke, Color fill, Point position)
        {
            Ellipse circle = new Ellipse();
            circle.Stroke = new SolidColorBrush(stroke);
            circle.Fill = new SolidColorBrush(fill);
            Canvas.SetLeft(circle, position.X);
            Canvas.SetTop(circle, position.Y);

            return circle;
        } 

    }
}
