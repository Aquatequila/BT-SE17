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
        private ToolsWindow tools = null;
        private PropertyWindow property = null;
        private Rectangle rect;
        private Ellipse ellipse;
        private Path path;
        private string pathString;

        public MainWindow()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
            tools       = new ToolsWindow();
            property    = new PropertyWindow();

            property.Show();
            property.Left += this.Width / 2 + property.Width / 2;

            GUIData.property = property;

            tools.Show();
            tools.Left -= this.Width / 2 + tools.Width/2;
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            tools.MakeClosable();
            tools.Close();

            base.OnClosing(e);
        }

        private Color ExtractARGB(string postfix = "")
        {
            Color color = new Color();

            TextBox a = (TextBox)property.FindName("aBox" + postfix);
            TextBox r = (TextBox)property.FindName("rBox" + postfix);
            TextBox g = (TextBox)property.FindName("gBox" + postfix);
            TextBox b = (TextBox)property.FindName("bBox" + postfix);

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

        private void startEllipse(Point mouseClick)
        {
            GUIData.clickPosition = mouseClick;

            Color fill = ExtractARGB();
            Color stroke = ExtractARGB("Stroke");

            ellipse = ShapeFactory.NewEllipse(stroke, fill, mouseClick);

            MainCanvas.Children.Add(ellipse);
        }

        private void startPath(Point mouseClick)
        {
            GUIData.clickPosition = mouseClick;

            Color fill = ExtractARGB();
            Color stroke = ExtractARGB("Stroke");

            path = ShapeFactory.NewPath(stroke, fill, mouseClick);
            pathString = String.Format("M{0} {1}", mouseClick.X, mouseClick.Y);
            path.Data = Geometry.Parse(pathString);
            MainCanvas.Children.Add(path);
        }

        private void updatePath(Point mouseClick)
        {
            path.Data = Geometry.Parse(String.Format("{0} L {1} {2}", pathString,
                mouseClick.X, mouseClick.Y));
        }

        private void setPathPoint(Point mouseClick)
        {
            pathString += String.Format(" L {0} {1}", mouseClick.X, mouseClick.Y);
        }

        private void EndPath(bool doClose = false)
        {
            if (doClose)
            {
                pathString += "Z";
            }
            path.Data = Geometry.Parse(pathString);
            path = null;
        }

        private void updateEllipse(Point currentMousePosition)
        {
            ellipse.Height = Math.Abs(GUIData.clickPosition.Y - currentMousePosition.Y);
            ellipse.Width  = Math.Abs(GUIData.clickPosition.X - currentMousePosition.X);

            if (GUIData.clickPosition.X > currentMousePosition.X) // width invert
            {
                Canvas.SetLeft(ellipse, GUIData.clickPosition.X - ellipse.Width);
            }
            if (GUIData.clickPosition.Y > currentMousePosition.Y) // height invert
            {
                Canvas.SetTop(ellipse, GUIData.clickPosition.Y - ellipse.Height);
            }
        }
        private void stopEllipse()
        {
            ellipse = null;
        }

        private void startRectangle(Point mouseClick)
        {
            GUIData.clickPosition = mouseClick;

            Color fill = ExtractARGB();
            Color stroke = ExtractARGB("Stroke");

            rect = ShapeFactory.NewRectangle(stroke, fill, mouseClick); 

            MainCanvas.Children.Add(rect);
        }

        private void stopRectangle()
        {
            rect = null;
        }

        private void updateRectangle(Point currentMousePosition)
        {
            rect.Height = Math.Abs(GUIData.clickPosition.Y - currentMousePosition.Y);
            rect.Width  = Math.Abs(GUIData.clickPosition.X - currentMousePosition.X);

            if (GUIData.clickPosition.X > currentMousePosition.X) // width invert
            {
                Canvas.SetLeft(rect, GUIData.clickPosition.X - rect.Width);
            }
            if (GUIData.clickPosition.Y > currentMousePosition.Y) // height invert
            {
                Canvas.SetTop(rect, GUIData.clickPosition.Y - rect.Height);
            }
        }

        private void MainCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (GUIData.selectedMenuItem == "rectangle")
            {
                startRectangle(e.GetPosition(this));
            }
            else if (GUIData.selectedMenuItem == "ellipse")
            {
                startEllipse(e.GetPosition(this));
            }
            else if (GUIData.selectedMenuItem == "path")
            {
                if (path == null)
                {
                    startPath(e.GetPosition(this));
                }
                else
                {
                    setPathPoint(e.GetPosition(this));
                }
                
            }
        }

        private void MainCanvas_MouseMove(object sender, MouseEventArgs e)
        {

            if (rect != null && GUIData.selectedMenuItem == "rectangle")
            {
                updateRectangle(e.GetPosition(this));
            }
            else if (ellipse != null && GUIData.selectedMenuItem == "ellipse")
            {
                updateEllipse(e.GetPosition(this));
            }
            else if (path != null && GUIData.selectedMenuItem == "path")
            {
                updatePath(e.GetPosition(this));
            }

        }

        private void MainCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (GUIData.selectedMenuItem == "rectangle")
            {
                stopRectangle();
            }
            else if (GUIData.selectedMenuItem == "ellipse")
            {
                stopEllipse();
            }
        }


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.P: GUIData.selectedMenuItem = "path"; Console.WriteLine(e.Key); break;
                case Key.C: GUIData.selectedMenuItem = "ellipse"; break;
                case Key.R: GUIData.selectedMenuItem = "rectangle";  Console.WriteLine(e.Key); break;
                case Key.E: GUIData.selectedMenuItem = "ellipse"; Console.WriteLine(e.Key); break;
                case Key.L: Console.WriteLine(e.Key); break;
                case Key.Z: EndPath(true); break;
                case Key.LeftCtrl: Console.WriteLine(e.Key); break;
                default:
                    Console.WriteLine("no valid key pressed");
                    break;
            }
        }

        private void MainCanvas_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            EndPath();
        }
    }
}
