using BT.ViewModel;
using GUI_BT_SE17.Shapes;
using GUI_BT_SE17.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GUI_BT_SE17.Functionality
{
    internal static class CreateTemplateLogic
    {
        private static DrawingModel drawModel = DrawingModel.GetInstance();

        public static void StartShape(Point mouseClick, ViewModel model)
        {
            model.MouseClick = mouseClick;

            model.StrokeEnabled = true;
            model.FillEnabled = false;

            PathBuilder.StartPath(mouseClick);

            drawModel.SetStrokeColor();
            drawModel.SetStrokeWidth();
        }

        public static void UpdatePath(Point mouseClick) // updates visual while the end point is not confirmed
        {
            PathBuilder.UpdatePath(mouseClick);
        }

        public static void SetPathPoint(Point mouseClick) // inserts the path point
        {
            PathBuilder.SetPathPoint(mouseClick);
        }

        public static void EndPath(bool doClose = false) // ends the path
        {
            PathBuilder.EndPath(doClose);
            drawModel.Selected = null;
        }

        public static void EndShape(ViewModel model) // shape gets deselected
        {
            drawModel.Selected = null;
        }
    }
}
