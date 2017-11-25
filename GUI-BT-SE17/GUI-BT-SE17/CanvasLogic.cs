using BT.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GUI_BT_SE17
{
    internal class CanvasLogic
    {
        public ViewModel ViewModel
        {
            get;
        }

        public CanvasLogic(CheckBox stroke, CheckBox fill, ComboBox colorStroke, ComboBox colorFill)
        {
            ViewModel = new ViewModel(stroke,fill,colorStroke,colorFill, InitCanvas());
        }

        public Canvas InitCanvas()
        {
            var canvas = new Canvas();
            canvas.Background = new SolidColorBrush(Colors.White);
            canvas.MouseLeftButtonDown += MouseLeftButtonDown;
            canvas.MouseMove += MouseMove;
            canvas.MouseLeftButtonUp += MouseLeftButtonUp;
            canvas.MouseRightButtonUp += MouseRightButtonUp;

            return canvas;
        }

        #region Canvas Mouse Functions
        private void MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            CreateShapeLogic.EndPath(ViewModel);
        }

        private void MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point position = e.GetPosition(ViewModel.Canvas);

            switch (ViewModel.SelectedMenuItem)
            {
                case MenuCommand.Ellipse: CreateShapeLogic.StartShape(position, ForgeShape.Ellipse, ViewModel); break;
                case MenuCommand.Path:
                    if (ViewModel.SelectedShape == null)
                    {
                        CreateShapeLogic.StartShape(position, ForgeShape.Path, ViewModel);
                    }
                    else
                    {
                        CreateShapeLogic.SetPathPoint(position, ViewModel);
                    }
                    break;
                case MenuCommand.Rectangle: CreateShapeLogic.StartShape(position, ForgeShape.Rectangle, ViewModel); break;
                case MenuCommand.None: break;
                default: break;
            }
        }

        private void MouseMove(object sender, MouseEventArgs e)
        {
            Point position = e.GetPosition(ViewModel.Canvas);
            if (ViewModel.SelectedShape != null)
            {
                switch (ViewModel.SelectedMenuItem)
                {
                    case MenuCommand.Path: CreateShapeLogic.UpdatePath(position, ViewModel); break;
                    case MenuCommand.None: break;
                    default: break;
                }
            }
        }

        private void MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (ViewModel.SelectedMenuItem != MenuCommand.None && ViewModel.SelectedMenuItem != MenuCommand.Path)
            {
                CreateShapeLogic.EndShape(ViewModel);
            }
        }
        #endregion
    }
}
