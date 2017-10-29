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

        public MainWindow()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();

            ApplicationData.canvas = (Canvas)this.FindName("MainCanvas");
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
        }

        public Color UnwrapListItemColor(string Name)
        {
            var comboBox = this.FindName("FillBox") as ComboBox;
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
            ApplicationData.mouseClick      = mouseClick;
            CheckBox strokeBox              = (CheckBox)this.FindName("StrokeCheckbox");
            CheckBox fillBox                = (CheckBox)this.FindName("FillCheckbox");
            strokeBox.IsChecked = true;
            fillBox.IsChecked   = true;
            bool strokeEnabled              = UnwrapCheckboxCheckedState(strokeBox.IsChecked);
            bool fillEnabled                = UnwrapCheckboxCheckedState(fillBox.IsChecked);

            Color fill                      = UnwrapListItemColor("FillBox");
            Color stroke                    = UnwrapListItemColor("StrokeBox");
            ApplicationData.selectedShape   = ShapeFactory.InitShape(stroke, fill, mouseClick, shape, strokeEnabled, fillEnabled);

            ApplicationData.canvas.Children.Add(ApplicationData.selectedShape);

            if (shape == ForgeShape.Path)
            {
                StartPath(mouseClick);
            }
        }
        private static void StartPath(Point mouseClick)
        {
            ApplicationData.pathString  = String.Format("M{0} {1}", mouseClick.X, mouseClick.Y);
            Path path                   = (Path)ApplicationData.selectedShape;
            path.Data                   = Geometry.Parse(ApplicationData.pathString);
        }

        private static void UpdatePath(Point mouseClick)
        {
            Path path = (Path)ApplicationData.selectedShape;
            path.Data = Geometry.Parse(String.Format("{0} L {1} {2}", 
                ApplicationData.pathString, mouseClick.X, mouseClick.Y));
        }

        private static void SetPathPoint(Point mouseClick)
        {
            ApplicationData.pathString += String.Format(" L {0} {1}", mouseClick.X, mouseClick.Y);
            Path path = (Path)ApplicationData.selectedShape;
            path.Data = Geometry.Parse(ApplicationData.pathString);
        }

        private void EndPath(bool doClose = false)
        {
            if (doClose)
            {
                ApplicationData.pathString += "Z";
            }
            Path path                       = (Path)ApplicationData.selectedShape;
            path.Data                       = Geometry.Parse(ApplicationData.pathString);
            ApplicationData.selectedShape   = null;
            ApplicationData.pathString      = "";
        }

        private static void UpdateShape(Point currentMousePosition)
        {
            Shape shape     = ApplicationData.selectedShape;
            Point position  = ApplicationData.mouseClick;

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
            ApplicationData.selectedShape = null;
        }

        private void MainCanvas_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            EndPath();
        }

        private void MainCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point position = e.GetPosition(ApplicationData.canvas);

            switch (ApplicationData.selectedMenuItem)
            {
                case MenuItem.Ellipse: StartShape(position, ForgeShape.Ellipse); break;
                case MenuItem.Path:
                    if (ApplicationData.selectedShape == null)
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
            Point position = e.GetPosition(ApplicationData.canvas);
            if (ApplicationData.selectedShape != null)
            {
                switch (ApplicationData.selectedMenuItem)
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
            if (ApplicationData.selectedMenuItem != MenuItem.None && ApplicationData.selectedMenuItem != MenuItem.Path)
            {
                EndShape();
            }
        }


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            ApplicationData.lastKeyPressed = e.Key;

            switch (e.Key)
            {
                case Key.P:         ApplicationData.selectedMenuItem = MenuItem.Path;      break;
                case Key.R:         ApplicationData.selectedMenuItem = MenuItem.Rectangle; break;
                case Key.E:         ApplicationData.selectedMenuItem = MenuItem.Ellipse;   break;
                case Key.L:         break;
                case Key.Z:         EndPath(true); break;
                case Key.M:         ApplicationData.selectedShape = null; ApplicationData.selectedMenuItem = MenuItem.None; break; 
                case Key.Escape:    ApplicationData.selectedShape = null; ApplicationData.selectedMenuItem = MenuItem.None; break;
                case Key.LeftCtrl:  Console.WriteLine(e.Key); break;
                default:
                    Console.WriteLine("no valid key pressed");
                    break;
            }
        }

        public static void shape_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ApplicationData.mouseClick = e.GetPosition(ApplicationData.canvas);

            if (ApplicationData.selectedMenuItem == MenuItem.Path)
            {
                SetPathPoint(e.GetPosition(ApplicationData.canvas));
                e.Handled = true;
            }
            else if (ApplicationData.lastKeyPressed == Key.M)
            {
                
                if (ApplicationData.selectedShape != null && ApplicationData.selectedShape.Equals(sender))
                {
                    ApplicationData.selectedShape = null;
                }
                else
                {
                    ApplicationData.selectedShape = (Shape) sender;
                }
                e.Handled = true;
            }
        }
        public static void shape_MouseMove(object sender, MouseEventArgs e)
        {
            if (ApplicationData.selectedShape != null && ApplicationData.lastKeyPressed == Key.M)
            {
                if (ApplicationData.selectedShape.Equals(sender))
                {
                    var shape       = (Shape)sender;
                    var mousePos    = e.GetPosition(ApplicationData.canvas);

                    double x        = mousePos.X - ApplicationData.mouseClick.X;
                    double y        = mousePos.Y - ApplicationData.mouseClick.Y;

                    double left     = Canvas.GetLeft(shape) + x;
                    double top      = Canvas.GetTop(shape)  + y;
                    
                    Canvas.SetLeft(shape, left);
                    Canvas.SetTop(shape, top);

                    ApplicationData.mouseClick = mousePos;
                    e.Handled = true;
                }
            }
            
        }

        internal static void shape_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (ApplicationData.selectedMenuItem != MenuItem.Path)
            {
                ApplicationData.selectedShape = null;
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
                SerializeToXML(this, ApplicationData.canvas, saveFileDialog.FileName);
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
                bmp.Render(ApplicationData.canvas);

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
            if (ApplicationData.selectedShape != null)
            {
                Color fill = UnwrapListItemColor("FillBox");
                ApplicationData.selectedShape.Fill = new SolidColorBrush(fill);
            }
        }

        private void FillCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (ApplicationData.selectedShape != null)
            {
                ApplicationData.selectedShape.Fill = null;
            }
        }

        private void StrokeCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (ApplicationData.selectedShape != null)
            {
                Color stroke = UnwrapListItemColor("StrokeBox");
                ApplicationData.selectedShape.Stroke = new SolidColorBrush(stroke);
            }
        }

        private void StrokeCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (ApplicationData.selectedShape != null)
            {
                ApplicationData.selectedShape.Stroke = null;
            }
        }
    }
}
