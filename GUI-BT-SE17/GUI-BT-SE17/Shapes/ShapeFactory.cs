using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GUI_BT_SE17
{
    public enum ForgeShape
    {
        Ellipse,
        Rectangle,
        Path
    }
    class ShapeFactory
    {
        public ShapeFactory() { }

        public static Shape InitShape(Color stroke, Color fill, Point position, ForgeShape elem, bool strokeSelected, bool fillSelected)
        {
            Shape shape = null;

            switch(elem)
            {
                case ForgeShape.Ellipse:    shape = new Ellipse();   break;
                case ForgeShape.Rectangle:  shape = new Rectangle(); break;
                case ForgeShape.Path:       shape = new Path();      break;
            }
            if (strokeSelected)
            {
                shape.Stroke = new SolidColorBrush(stroke);
            }
            if (fillSelected)
            {
                shape.Fill =  new SolidColorBrush(fill);
                // this makes the shape clickable
                //shape.Fill = new SolidColorBrush(Colors.Transparent);
            }

            if (elem != ForgeShape.Path)
            {
                Canvas.SetLeft(shape, position.X);
                Canvas.SetTop(shape, position.Y);
            }
            

            //shape.MouseDown           += MainWindow.shape_MouseLeftButtonDown;
            //shape.MouseMove           += MainWindow.shape_MouseMove;
            //shape.MouseLeftButtonUp += MainWindow.shape_MouseLeftButtonUp;

            shape.MouseEnter += (sender, args) => {
                Shape a = sender as Shape; 
                a.StrokeThickness *= 3;
                //a.Fill = new SolidColorBrush(Colors.Beige);
            };
            shape.MouseLeave += (sender, args) => {
                Shape a = sender as Shape;
                a.StrokeThickness /= 3;
            };

            return shape;
        }

    }
}
