using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace GUI_BT_SE17
{
    enum MenuItem
    {
        Rectangle,
        Ellipse,
        Path,
        None
    }

    class ApplicationData
    {
        public static MenuItem         selectedMenuItem { get; set; }
        public static Point            mouseClick       { get; set; }
        public static Shape            selectedShape    { get; set; }
        public static Key              lastKeyPressed   { get; set; }
        public static Canvas           canvas           { get; set; }
        public static String           pathString       { get; set; }
    }
}
