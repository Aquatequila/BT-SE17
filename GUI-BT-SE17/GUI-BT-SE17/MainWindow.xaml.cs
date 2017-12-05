using BT.ViewModel;
using GUI_BT_SE17.Enums;
using GUI_BT_SE17.Shapes.Commands;
using GUI_BT_SE17.ViewModels;
using Microsoft.Win32;
using Svg.Path.Operations;
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

        public static ViewModel GetViewModel ()
        {
            return viewModel;
        }

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

            Loaded += (s, e) =>
            {
                DataContext = viewModel;
            };
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
        }

        #endregion

        private DrawingModel drawModel = DrawingModel.GetInstance();

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            viewModel.PressedKey = e.Key;

            switch (e.Key)
            {
                case Key.Delete:
                    {
                        drawModel.RemoveSelectedShape();
                        break;
                    }
                case Key.P:
                    {
                        viewModel.SelectedMenuItem = Operation.Path;
                        break;
                    }
                case Key.Z:
                    {
                        if (viewModel.SelectedShape != null)
                            CreateShapeLogic.EndPath(true);
                        break;
                    }
                case Key.M:
                    {
                        viewModel.SelectedMenuItem = Operation.Move;
                        break;
                    }
                case Key.V:
                    {
                        if (drawModel.Selected != null)
                        {
                            drawModel.MirrorSelectedVertical();
                            drawModel.Selected = null;
                            viewModel.SelectedMenuItem = Operation.None;
                        }
                        break;
                    }
                case Key.H:
                    {
                        if (viewModel.SelectedShape != null)
                        {
                            drawModel.MirrorSelectedHorizontal();
                            drawModel.Selected = null;
                            viewModel.SelectedMenuItem = Operation.None;
                        }
                        break;
                    }
                case Key.A:
                    {
                        viewModel.SelectedMenuItem = Operation.Annotation;
                        break;
                    }
                default:
                    Console.WriteLine("no valid key pressed");
                    break;
            }
        }

        #region menu buttons
        private void SaveFileButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = @"C:\Users\ntecm\desktop\temp";
            if (saveFileDialog.ShowDialog() == true)
            {
                drawModel.WriteToFile(saveFileDialog.FileName + ".svg");
            }
        }

        private void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = @"C:\Users\ntecm\desktop\temp";
            if (openFileDialog.ShowDialog() == true)
            {
                drawModel.LoadFromFile(openFileDialog.FileName);   
            }
        }

        private void ToPngButton_Click(object sender, RoutedEventArgs e)
        {
            buttonHandler.ToPng(viewModel.Canvas, (int)this.Width, (int)this.Height);
            e.Handled = true;
        }

        private void Increment_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Pixel++;
        }

        private void Decrement_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Pixel--;
        }
        #endregion


    }
}
