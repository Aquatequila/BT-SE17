using BT.ViewModel;
using System.Windows;
using System.Windows.Shapes;

namespace GUI_BT_SE17
{
    internal static class CreateShapeLogic
    {
        #region shape functions
        public static void StartShape(Point mouseClick, ForgeShape shape, ViewModel model)
        {
            model.MouseClick = mouseClick;

            model.StrokeEnabled = true;
            model.FillEnabled = false;

            model.SelectedShape = ShapeContainer.StartPath(mouseClick);

            ShapeContainer.SetStrokeColor(model.StrokeColor);
            ShapeContainer.SetStrokeWidth(model.Pixel);
            ShapeContainer.SetFillColor(model.FillColor);

            model.Canvas.Children.Add(model.SelectedShape);
        }

        public static void UpdatePath(Point mouseClick, ViewModel model)
        {
            Path path = (Path)model.SelectedShape;
            path.Data = ShapeContainer.UpdatePath(mouseClick).Data;
        }

        public static void SetPathPoint(Point mouseClick, ViewModel model)
        {
            Path path = (Path)model.SelectedShape;
            path.Data = ShapeContainer.AddLineToPath(mouseClick).Data;
        }

        public static void EndPath(ViewModel model, bool doClose = false)
        {
            ShapeContainer.RemoveLastPoint();
            Path path = (Path)model.SelectedShape;
            path.Data = ShapeContainer.EndPath(doClose).Data;
            model.SelectedShape = null;
        }

        public static void EndShape(ViewModel model)
        {
            model.SelectedShape = null;
        }
        #endregion
    }
}
