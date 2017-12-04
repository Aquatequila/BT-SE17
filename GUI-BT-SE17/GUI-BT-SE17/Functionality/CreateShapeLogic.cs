using BT.ViewModel;
using GUI_BT_SE17.ViewModels;
using System.Windows;
using System.Windows.Shapes;

namespace GUI_BT_SE17
{
    internal static class CreateShapeLogic
    {
        private static DrawingModel drawModel = DrawingModel.GetInstance();
        #region shape functions
        public static void StartShape(Point mouseClick, ViewModel model)
        {
            model.MouseClick = mouseClick;

            model.StrokeEnabled = true;
            model.FillEnabled = false;

            PathBuilder.StartPath(mouseClick);

            drawModel.SetStrokeColor();
            drawModel.SetStrokeWidth();
        }

        public static void UpdatePath(Point mouseClick)
        {
            PathBuilder.UpdatePath(mouseClick);
        }

        public static void SetPathPoint(Point mouseClick)
        {
            PathBuilder.SetPathPoint(mouseClick);
        }

        public static void EndPath(bool doClose = false)
        {
            PathBuilder.EndPath(doClose);
            drawModel.Selected = null;
        }

        public static void EndShape(ViewModel model)
        {
            drawModel.Selected = null;
        }
        #endregion
    }
}
