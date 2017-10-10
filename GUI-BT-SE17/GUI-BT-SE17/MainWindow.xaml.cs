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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GUI_BT_SE17
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
            
            ApplicationData.toolsWindow     = new ToolsWindow();
            ApplicationData.toolsWindow.Show();
            ApplicationData.propertyWindow  = new PropertyWindow();
            ApplicationData.propertyWindow.Show();


            ApplicationData.propertyWindow.Left += this.Width / 2 + ApplicationData.propertyWindow.Width / 2;

            ApplicationData.canvas = (Canvas)this.FindName("MainCanvas");

            ApplicationData.toolsWindow.Left -= this.Width / 2 + ApplicationData.toolsWindow.Width/2;
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            ApplicationData.toolsWindow.MakeClosable();
            ApplicationData.toolsWindow.Close();
            ApplicationData.propertyWindow.Close();

            base.OnClosing(e);
        }

        

        public static Color ExtractARGB(string postfix = "")
        {
            Color color = new Color();

            TextBox a = (TextBox)ApplicationData.propertyWindow.FindName("aBox" + postfix);
            TextBox r = (TextBox)ApplicationData.propertyWindow.FindName("rBox" + postfix);
            TextBox g = (TextBox)ApplicationData.propertyWindow.FindName("gBox" + postfix);
            TextBox b = (TextBox)ApplicationData.propertyWindow.FindName("bBox" + postfix);

            byte aVal;
            byte.TryParse(a.Text, out aVal);

            byte rVal;
            byte.TryParse(r.Text, out rVal);

            byte gVal;
            byte.TryParse(g.Text, out gVal);

            byte bVal;
            byte.TryParse(b.Text, out bVal);

            color.A = aVal;
            color.R = rVal;
            color.G = gVal;
            color.B = bVal;

            return color;
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

        private static void StartShape(Point mouseClick, ForgeShape shape)
        {
            ApplicationData.mouseClick      = mouseClick;
            CheckBox strokeBox              = (CheckBox)ApplicationData.propertyWindow.FindName("StrokeEnabled");
            CheckBox fillBox                = (CheckBox)ApplicationData.propertyWindow.FindName("FillEnabled");
            strokeBox.IsChecked = true;
            fillBox.IsChecked   = true;
            bool strokeEnabled              = UnwrapCheckboxCheckedState(strokeBox.IsChecked);
            bool fillEnabled                = UnwrapCheckboxCheckedState(fillBox.IsChecked);

            Color fill                      = ExtractARGB();
            Color stroke                    = ExtractARGB("Stroke");
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
    }
}
