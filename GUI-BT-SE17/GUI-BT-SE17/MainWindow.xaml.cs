using BT.ViewModel;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GUI_BT_SE17
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static ViewModel viewModel;
        private ButtonHandler buttonHandler = new ButtonHandler();
        private static CanvasLogic canvasLogic;

        #region general

        private void InitViewModel()
        {
            CheckBox strokeBox = (CheckBox)this.FindName("StrokeCheckbox");
            CheckBox fillBox = (CheckBox)this.FindName("FillCheckbox");
            var fillColorBox = this.FindName("FillBox") as ComboBox;
            var strokeColorBox = this.FindName("StrokeBox") as ComboBox;

            canvasLogic = new CanvasLogic(strokeBox, fillBox, strokeColorBox, fillColorBox);
            viewModel = canvasLogic.ViewModel;
        }

        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            InitViewModel();

            this.Loaded += (s, e) =>
            {
                this.DataContext = viewModel;
            };
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
        }


        #endregion
   
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            viewModel.PressedKey = e.Key;

            switch (e.Key)
            {
                case Key.P: viewModel.SelectedMenuItem = MenuCommand.Path;      break;
                case Key.R: viewModel.SelectedMenuItem = MenuCommand.Rectangle; break;
                case Key.E: viewModel.SelectedMenuItem = MenuCommand.Ellipse;   break;
                case Key.L:         break;
                case Key.Z:        CreateShapeLogic.EndPath(viewModel, true); break;
                case Key.M: viewModel.SelectedShape = null; viewModel.SelectedMenuItem = MenuCommand.None; break; 
                case Key.Escape: viewModel.SelectedShape = null; viewModel.SelectedMenuItem = MenuCommand.None; break;
                case Key.LeftCtrl:  Console.WriteLine(e.Key); break;
                default:
                    Console.WriteLine("no valid key pressed");
                    break;
            }
        }

        #region shape functions
        //public static void shape_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    viewModel.MouseClick = e.GetPosition(viewModel.Canvas);

        //    if (viewModel.SelectedMenuItem == MenuCommand.Path)
        //    {
        //        CreateShapeLogic.SetPathPoint(e.GetPosition(viewModel.Canvas), viewModel);
        //        e.Handled = true;
        //    }
        //    else if (viewModel.PressedKey == Key.M)
        //    {
        //        if (viewModel.SelectedShape != null && viewModel.SelectedShape.Equals(sender))
        //        {
        //            viewModel.SelectedShape = null;
        //        }
        //        else
        //        {
        //            viewModel.SelectedShape = (Shape)sender;
        //        }
        //        e.Handled = true;
        //    }
        //}
        //public static void shape_MouseMove(object sender, MouseEventArgs e)
        //{
        //    if (viewModel.SelectedShape != null && viewModel.PressedKey == Key.M)
        //    {
        //        if (viewModel.SelectedShape.Equals(sender))
        //        {
        //            var shape = (Shape)sender;
        //            var mousePos = e.GetPosition(viewModel.Canvas);

        //            double x = mousePos.X - viewModel.MouseClick.X;
        //            double y = mousePos.Y - viewModel.MouseClick.Y;

        //            double left = Canvas.GetLeft(shape) + x;
        //            double top = Canvas.GetTop(shape) + y;

        //            Canvas.SetLeft(shape, left);
        //            Canvas.SetTop(shape, top);

        //            viewModel.MouseClick = mousePos;
        //            e.Handled = true;
        //        }
        //    }

        //}
        #endregion

        #region menu buttons
        private void SaveFileButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            //saveFileDialog.InitialDirectory = @"c:\";
            if (saveFileDialog.ShowDialog() == true)
            {
                ShapeContainer.WriteToFile(saveFileDialog.FileName + ".svg");
            }
        }

        private void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                ShapeContainer.LoadFromFile(openFileDialog.FileName, viewModel.Canvas);   
            }
        }

        private void ToPngButton_Click(object sender, RoutedEventArgs e)
        {
            buttonHandler.ToPng(viewModel.Canvas, (int)this.Width, (int)this.Height);
            e.Handled = true;
        }
#endregion
        #region stroke fill  boxes
        private void FillCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            if (viewModel.SelectedShape != null)
            {
                Color fill = viewModel.FillColor;
                viewModel.SelectedShape.Fill = new SolidColorBrush(fill);
            }
        }

        private void FillCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (viewModel.SelectedShape != null)
            {
                viewModel.SelectedShape.Fill = null;
            }
        }
        private void StrokeCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (viewModel.SelectedShape != null)
            {
                Color stroke = viewModel.StrokeColor;
                viewModel.SelectedShape.Stroke = new SolidColorBrush(stroke);
            }
        }

        private void StrokeCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (viewModel.SelectedShape != null)
            {
                viewModel.SelectedShape.Stroke = null;
            }
        }

        private void FillBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (viewModel.SelectedShape != null)
            {
                viewModel.FillEnabled = true;
                viewModel.SelectedShape.Fill = new SolidColorBrush(viewModel.FillColor);
            }
        }

        private void StrokeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (viewModel.SelectedShape != null)
            {
                viewModel.SelectedShape = viewModel.SelectedShape;
                viewModel.StrokeEnabled = true;
                viewModel.SelectedShape.Stroke = new SolidColorBrush(viewModel.StrokeColor);
            }
        }
        #endregion

        private void Increment_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Pixel++;
        }

        private void Decrement_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Pixel--;
        }
    }
}
