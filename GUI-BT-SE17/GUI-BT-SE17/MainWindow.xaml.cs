using Microsoft.Win32;
using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GUI_BT_SE17
{
    using FileStream = System.IO.FileStream;
    using File = System.IO.File;
    using StreamWriter = System.IO.StreamWriter;
    using FileMode = System.IO.FileMode;
    using FileAccess = System.IO.FileAccess;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static ApplicationData app = new ApplicationData();
        public MainWindow()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
            app.canvas = (Canvas)this.FindName("MainCanvas");
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
        }
        public Color UnwrapListItemColor(string Name)
        {
            var comboBox = this.FindName(Name) as ComboBox;
            return (Color) ColorConverter.ConvertFromString(comboBox.Text);
        }
        private static bool UnwrapCheckboxCheckedState(bool? state)
        {
            switch(state)
            {
                case true:  return true;
                case false: return false;
                default:    return false;
            }
        }

        private void StartShape(Point mouseClick, ForgeShape shape)
        {
            app.mouseClick      = mouseClick;
            CheckBox strokeBox              = (CheckBox)this.FindName("StrokeCheckbox");
            CheckBox fillBox                = (CheckBox)this.FindName("FillCheckbox");
            strokeBox.IsChecked = true;
            fillBox.IsChecked   = true;
            bool strokeEnabled              = UnwrapCheckboxCheckedState(strokeBox.IsChecked);
            bool fillEnabled                = UnwrapCheckboxCheckedState(fillBox.IsChecked);

            Color fill                      = UnwrapListItemColor("FillBox");
            Color stroke                    = UnwrapListItemColor("StrokeBox");
            app.selectedShape   = ShapeFactory.InitShape(stroke, fill, mouseClick, shape, strokeEnabled, fillEnabled);

            app.canvas.Children.Add(app.selectedShape);

            if (shape == ForgeShape.Path)
            {
                StartPath(mouseClick);
            }
        }
        private static void StartPath(Point mouseClick)
        {
            app.pathString  = String.Format("M{0} {1}", mouseClick.X, mouseClick.Y);
            Path path                   = (Path)app.selectedShape;
            path.Data                   = Geometry.Parse(app.pathString);
        }

        private static void UpdatePath(Point mouseClick)
        {
            Path path = (Path)app.selectedShape;
            path.Data = Geometry.Parse(String.Format("{0} L {1} {2}", 
                app.pathString, mouseClick.X, mouseClick.Y));
        }

        private static void SetPathPoint(Point mouseClick)
        {
            app.pathString += String.Format(" L {0} {1}", mouseClick.X, mouseClick.Y);
            Path path = (Path)app.selectedShape;
            path.Data = Geometry.Parse(app.pathString);
        }

        private void EndPath(bool doClose = false)
        {
            if (doClose)
            {
                app.pathString += "Z";
            }
            Path path                       = (Path)app.selectedShape;
            path.Data                       = Geometry.Parse(app.pathString);
            app.selectedShape   = null;
            app.pathString      = "";
        }

        private static void UpdateShape(Point currentMousePosition)
        {
            Shape shape     = app.selectedShape;
            Point position  = app.mouseClick;

            shape.Height    = Math.Abs(position.Y - currentMousePosition.Y);
            shape.Width     = Math.Abs(position.X - currentMousePosition.X);

            if (position.X > currentMousePosition.X) // width invert
            {
                Canvas.SetLeft(shape, position.X - shape.Width);
            }
            if (position.Y > currentMousePosition.Y) // height invert
            {
                Canvas.SetTop(shape, position.Y - shape.Height);
            }
        }

        private static void EndShape()
        {
            app.selectedShape = null;
        }

        private void MainCanvas_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            EndPath();
        }

        private void MainCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point position = e.GetPosition(app.canvas);

            switch (app.selectedMenuItem)
            {
                case MenuItem.Ellipse: StartShape(position, ForgeShape.Ellipse); break;
                case MenuItem.Path:
                    if (app.selectedShape == null)
                    {
                        StartShape(position, ForgeShape.Path);
                    }
                    else
                    {
                        SetPathPoint(position);
                    }
                    break;
                case MenuItem.Rectangle: StartShape(position, ForgeShape.Rectangle); break;
                case MenuItem.None: break;
                default: break;
            }
        }

        private void MainCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            Point position = e.GetPosition(app.canvas);
            if (app.selectedShape != null)
            {
                switch (app.selectedMenuItem)
                {
                    case MenuItem.Ellipse:   UpdateShape(position); break;
                    case MenuItem.Path:      UpdatePath(position);  break;
                    case MenuItem.Rectangle: UpdateShape(position); break;
                    case MenuItem.None: break;
                    default: break;
                }
            }
        }

        private void MainCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (app.selectedMenuItem != MenuItem.None && app.selectedMenuItem != MenuItem.Path)
            {
                EndShape();
            }
        }


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            app.lastKeyPressed = e.Key;

            switch (e.Key)
            {
                case Key.P:         app.selectedMenuItem = MenuItem.Path;      break;
                case Key.R:         app.selectedMenuItem = MenuItem.Rectangle; break;
                case Key.E:         app.selectedMenuItem = MenuItem.Ellipse;   break;
                case Key.L:         break;
                case Key.Z:         EndPath(true); break;
                case Key.M:         app.selectedShape = null; app.selectedMenuItem = MenuItem.None; break; 
                case Key.Escape:    app.selectedShape = null; app.selectedMenuItem = MenuItem.None; break;
                case Key.LeftCtrl:  Console.WriteLine(e.Key); break;
                default:
                    Console.WriteLine("no valid key pressed");
                    break;
            }
        }

        public static void shape_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            app.mouseClick = e.GetPosition(app.canvas);

            if (app.selectedMenuItem == MenuItem.Path)
            {
                SetPathPoint(e.GetPosition(app.canvas));
                e.Handled = true;
            }
            else if (app.lastKeyPressed == Key.M)
            {
                
                if (app.selectedShape != null && app.selectedShape.Equals(sender))
                {
                    app.selectedShape = null;
                }
                else
                {
                    app.selectedShape = (Shape) sender;
                }
                e.Handled = true;
            }
        }
        public static void shape_MouseMove(object sender, MouseEventArgs e)
        {
            if (app.selectedShape != null && app.lastKeyPressed == Key.M)
            {
                if (app.selectedShape.Equals(sender))
                {
                    var shape       = (Shape)sender;
                    var mousePos    = e.GetPosition(app.canvas);

                    double x        = mousePos.X - app.mouseClick.X;
                    double y        = mousePos.Y - app.mouseClick.Y;

                    double left     = Canvas.GetLeft(shape) + x;
                    double top      = Canvas.GetTop(shape)  + y;
                    
                    Canvas.SetLeft(shape, left);
                    Canvas.SetTop(shape, top);

                    app.mouseClick = mousePos;
                    e.Handled = true;
                }
            }
            
        }

        internal static void shape_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (app.selectedMenuItem != MenuItem.Path)
            {
                app.selectedShape = null;
                e.Handled = true;
            }
        }

        private void SerializeToXML(MainWindow window, Canvas canvas, string filename)
        {
            string mystrXAML = XamlWriter.Save(canvas);
            FileStream filestream = File.Create(filename);
            StreamWriter streamwriter = new StreamWriter(filestream);
            streamwriter.Write(mystrXAML);
            streamwriter.Close();
            filestream.Close();
        }

        private void SaveFileButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            //saveFileDialog.InitialDirectory = @"c:\";
            if (saveFileDialog.ShowDialog() == true)
            {
                SerializeToXML(this, app.canvas, saveFileDialog.FileName);
            }
        }

        private void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                Console.WriteLine($"file to load = {openFileDialog.FileName}");
                var fileStream = File.Open(openFileDialog.FileName, FileMode.Open, FileAccess.Read);
                var canvasLoaded = XamlReader.Load(fileStream) as Canvas;
                fileStream.Close();


            }
        }

        private void ToPngButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            //saveFileDialog.InitialDirectory = @"c:\";
            if (saveFileDialog.ShowDialog() == true)
            {
                RenderTargetBitmap bmp = new RenderTargetBitmap((int)this.Width, (int)this.Height, 96, 96, PixelFormats.Default);
                //app.canvas.Background = new SolidColorBrush(Colors.Transparent);
                bmp.Render(app.canvas);

                var png = new PngBitmapEncoder();
                png.Frames.Add(BitmapFrame.Create(bmp));
                using (var stm = File.Create(saveFileDialog.FileName))
                {
                    png.Save(stm);
                }
            }
        }

        private void FillCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            if (app.selectedShape != null)
            {
                Color fill = UnwrapListItemColor("FillBox");
                app.selectedShape.Fill = new SolidColorBrush(fill);
            }
        }

        private void FillCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (app.selectedShape != null)
            {
                app.selectedShape.Fill = null;
            }
        }

        private void StrokeCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (app.selectedShape != null)
            {
                Color stroke = UnwrapListItemColor("StrokeBox");
                app.selectedShape.Stroke = new SolidColorBrush(stroke);
            }
        }

        private void StrokeCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (app.selectedShape != null)
            {
                app.selectedShape.Stroke = null;
            }
        }

        private void FillBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (app.selectedShape != null)
            {
                CheckBox box = (CheckBox)FindName("FillCheckbox");
                box.IsChecked = false;
            }
        }

        private void StrokeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (app.selectedShape != null)
            {
                CheckBox box = (CheckBox)FindName("StrokeCheckbox");
                box.IsChecked = false;
            }
        }
    }
}
