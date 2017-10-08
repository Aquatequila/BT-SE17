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

        public MainWindow()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
            tools       = new ToolsWindow();
            property = new PropertyWindow();

            property.Show();
            property.Left += this.Width / 2;

            tools.Show();
            tools.Left -= this.Width / 2 + tools.Width/2;
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            tools.MakeClosable();
            tools.Close();

            base.OnClosing(e);
        }

        private void startRectangle(Point mouseClick)
        {
            GUIData.clickPosition = mouseClick;
            rect = new Rectangle();
            rect.Stroke = new SolidColorBrush(Colors.Black);
            rect.Fill = new SolidColorBrush(Colors.Red);
            Canvas.SetLeft(rect, GUIData.clickPosition.X);
            Canvas.SetTop(rect, GUIData.clickPosition.Y);
            MainCanvas.Children.Add(rect);

        }

        private void stopRectangle()
        {
            rect = null;
        }

        private void updateRectangle(Point mousePosition)
        {
            rect.Height = Math.Abs(GUIData.clickPosition.Y - mousePosition.Y);
            rect.Width  = Math.Abs(GUIData.clickPosition.X - mousePosition.X);

            if (GUIData.clickPosition.X > mousePosition.X) // width invert
            {
                Canvas.SetLeft(rect, GUIData.clickPosition.X - rect.Width);
            }
            if (GUIData.clickPosition.Y > mousePosition.Y) // height invert
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
        }

        private void MainCanvas_MouseMove(object sender, MouseEventArgs e)
        {

            if (rect != null && GUIData.selectedMenuItem == "rectangle")
            {
                updateRectangle(e.GetPosition(this));
            }
            
        }

        private void MainCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (GUIData.selectedMenuItem == "rectangle")
            {
                stopRectangle();
            }
        }
    }
}
