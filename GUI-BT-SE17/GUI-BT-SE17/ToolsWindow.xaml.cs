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
    /// Interaction logic for ToolsWindow.xaml
    /// </summary>
    public partial class ToolsWindow : Window
    {
        private bool closeable = false;
        
        public ToolsWindow()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();

            shapes.Items.Add("rectangle");
            shapes.Items.Add("path");
            shapes.Items.Add("ellipse");

        }
        public void MakeClosable()
        {
            closeable = true;
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = !closeable;

            base.OnClosing(e);
        }

        private void shapes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selected = (string) e.AddedItems[0];
            GUIData.selectedMenuItem = selected;

        }
    }
}
