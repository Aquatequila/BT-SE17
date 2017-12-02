using BT.ViewModel;
using GUI_BT_SE17.Enums;
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
                case Operation.Path:
                    {
                        if (ViewModel.SelectedShape == null) CreateShapeLogic.StartShape(position, ViewModel);
                        else CreateShapeLogic.SetPathPoint(position, ViewModel);
                        break;
                    }
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
                    case Operation.Path: CreateShapeLogic.UpdatePath(position, ViewModel); break;
                    case Operation.None: break;
                    default: break;
                }
            }
        }

        private void MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (ViewModel.SelectedMenuItem != Operation.None && ViewModel.SelectedMenuItem != Operation.Path)
            {
                CreateShapeLogic.EndShape(ViewModel);
            }
        }
        #endregion
    }
}
