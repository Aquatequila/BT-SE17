using BT.ViewModel;
using GUI_BT_SE17.Enums;
using GUI_BT_SE17.Shapes;
using GUI_BT_SE17.ViewModels;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GUI_BT_SE17
{
    internal class CanvasLogic
    {
        private DrawingModel drawModel = DrawingModel.GetInstance();

        public ViewModel ViewModel
        {
            get;
        }

        public CanvasLogic(CheckBox stroke, CheckBox fill, ComboBox colorStroke, ComboBox colorFill)
        {
            ViewModel = ViewModel.GetInstance(stroke,fill,colorStroke,colorFill, InitCanvas());
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

        private AnnotationDrawer anno;
        private List<Path> annotatioShapes; 

        private void MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (ViewModel.SelectedShape != null)
            {
                CreateShapeLogic.EndPath();
            }
        }

        private ShapeMoveCommand moveCommand;
        private void MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point position = e.GetPosition(ViewModel.Canvas);

            switch (ViewModel.SelectedMenuItem)
            {
                case Operation.Path:
                    {
                        if (ViewModel.SelectedShape == null) CreateShapeLogic.StartShape(position, ViewModel);
                        else CreateShapeLogic.SetPathPoint(position);
                        break;
                    }
                case Operation.Annotation:
                    {
                        anno = new AnnotationDrawer(ViewModel.SelectedAnnotation);
                        anno.Start(position);
                        break;
                    }
                case Operation.Move:
                    {
                        if (ViewModel.SelectedShape != null)
                        {
                            moveCommand = new ShapeMoveCommand(drawModel.SvgForShape(ViewModel.SelectedShape), position);
                        }
                        break;
                    }
                default: break;
            }
        }

        private void MouseMove(object sender, MouseEventArgs e)
        {
            Point position = e.GetPosition(ViewModel.Canvas);

            if (ViewModel.SelectedShape != null || ViewModel.SelectedMenuItem == Operation.Annotation)
            {
                switch (ViewModel.SelectedMenuItem)
                {
                    case Operation.Path:
                        {
                            CreateShapeLogic.UpdatePath(position);
                            break;
                        }
                    case Operation.None:
                        {
                            break;
                        }
                    case Operation.Annotation:
                        {
                            if (anno != null && anno.Updateable)
                            {
                                if (annotatioShapes != null)
                                {
                                    foreach (var shape in annotatioShapes)
                                        ViewModel.Canvas.Children.Remove(shape);
                                }
                                annotatioShapes = anno.Update(position);
                                foreach (var shape in annotatioShapes)
                                {
                                    ViewModel.Canvas.Children.Add(shape);
                                }
                            }
                            break;
                        }
                    case Operation.Move:
                        {
                            if (ViewModel.SelectedShape != null && moveCommand != null)
                            {
                                drawModel.UpdateSelected(moveCommand.MoveTo(position, out var svg), svg);
                            }
                            break;
                        }
                    default: break;
                }
            }
        }

        private void MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Point position = e.GetPosition(ViewModel.Canvas);

            if (ViewModel.SelectedMenuItem == Operation.Move)
            {
                if (ViewModel.SelectedShape != null && moveCommand != null)
                {
                    drawModel.UpdateSelected(moveCommand.MoveTo(position, out var svg), svg);
                    drawModel.Selected = null;
                    ViewModel.SelectedMenuItem = Operation.None;
                    moveCommand = null;
                }
            }
            else if (ViewModel.SelectedMenuItem == Operation.Annotation)
            {
                if (anno != null && anno.Updateable)
                {
                    if (annotatioShapes != null)
                    {
                        foreach (var shape in annotatioShapes)
                            ViewModel.Canvas.Children.Remove(shape);
                    }
                    annotatioShapes = anno.Update(position, out var svgs);
                    foreach (var shape in annotatioShapes)
                    {
                        ViewModel.Canvas.Children.Add(shape);
                    }

                    drawModel.AddElements(annotatioShapes, svgs);

                    anno = null;
                    annotatioShapes = null;

                }
            }
            else if (ViewModel.SelectedMenuItem != Operation.Edit && ViewModel.SelectedMenuItem != Operation.None && ViewModel.SelectedMenuItem != Operation.Path)
            {
                CreateShapeLogic.EndShape(ViewModel);
            }
        }
        #endregion
    }
}
