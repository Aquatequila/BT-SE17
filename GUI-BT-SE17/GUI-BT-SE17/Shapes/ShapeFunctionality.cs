using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Shapes;

namespace GUI_BT_SE17.Shapes
{
    internal static class ShapeFunctionality
    {
        public static void Click(object sender, MouseEventArgs args)
        {
            var model = MainWindow.GetViewModel();

            model.SelectedShape = (Path)sender;
            model.SelectedMenuItem = Enums.Operation.Edit;

            args.Handled = true;
        }

        public static void Hover(object sender, MouseEventArgs args)
        {
            (sender as Shape).StrokeThickness += 3;
        }

        public static void EndHover(object sender, MouseEventArgs args)
        {
            (sender as Shape).StrokeThickness -= 3;
        }
    }
}
