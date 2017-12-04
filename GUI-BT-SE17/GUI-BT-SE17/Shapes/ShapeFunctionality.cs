using GUI_BT_SE17.ViewModels;
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
        private static DrawingModel drawingModel = DrawingModel.GetInstance();

        public static void Click(object sender, MouseEventArgs args)
        {
            var model = MainWindow.GetViewModel();

            drawingModel.Selected = (Path)sender;
            model.SelectedMenuItem = Enums.Operation.Edit;

            args.Handled = true;
        }

        private static bool hovered = false;

        public static void Hover(object sender, MouseEventArgs args)
        {
            if (!hovered)
                (sender as Shape).StrokeThickness += 3;
            hovered = true;
        }

        public static void EndHover(object sender, MouseEventArgs args)
        {
            if (hovered)
                (sender as Shape).StrokeThickness -= 3;
            hovered = false;
        }
    }
}
