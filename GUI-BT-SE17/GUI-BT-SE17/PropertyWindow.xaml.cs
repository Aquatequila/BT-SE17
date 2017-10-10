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
using System.Windows.Shapes;

namespace GUI_BT_SE17
{
    /// <summary>
    /// Interaction logic for PropertyWindow.xaml
    /// </summary>
    public partial class PropertyWindow : Window
    {
        public PropertyWindow()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        private void FillEnabled_Checked(object sender, RoutedEventArgs e)
        {
            if (ApplicationData.selectedShape != null)
            {
                Color fill = MainWindow.ExtractARGB();
                ApplicationData.selectedShape.Fill = new SolidColorBrush(fill);
            }
        }

        private void FillEnabled_Unchecked(object sender, RoutedEventArgs e)
        {
            if (ApplicationData.selectedShape != null)
            {
                ApplicationData.selectedShape.Fill = null;
            }
        }

        private void StrokeEnabled_Checked(object sender, RoutedEventArgs e)
        {
            if (ApplicationData.selectedShape != null)
            {
                Color stroke = MainWindow.ExtractARGB("Stroke");
                ApplicationData.selectedShape.Stroke = new SolidColorBrush(stroke);
            }
        }

        private void StrokeEnabled_Unchecked(object sender, RoutedEventArgs e)
        {
            if (ApplicationData.selectedShape != null)
            {
                ApplicationData.selectedShape.Stroke = null;
            }
        }
    }
}
