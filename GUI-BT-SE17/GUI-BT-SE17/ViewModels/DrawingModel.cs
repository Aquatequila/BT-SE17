using Svg.Wrapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace GUI_BT_SE17.ViewModels
{
    internal class DrawingModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private DrawingModel() { }

        private static DrawingModel self = null;
        public static DrawingModel GetInstance()
        {
            if (self == null)
            {
                self = new DrawingModel();
            }
            return self;
        }
        
        private Canvas canvas;
        public Canvas Canvas
        {
            get => canvas;
            set
            {
                canvas = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Canvas)));
            }
        }
        
        private List<SvgElement> svgs = new List<SvgElement>();
        public List<SvgElement> Svgs
        {
            get { return svgs; }
            set
            {
                svgs = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Svgs)));
            }
        }
        private List<Path> shapes = new List<Path>();
        public List<Path> Shapes
        {
            get { return shapes; }
            set
            {
                shapes = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Shapes)));
            }
        }
    }
}
