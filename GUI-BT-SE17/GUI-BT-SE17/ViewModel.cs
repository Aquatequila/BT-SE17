using GUI_BT_SE17;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BT.ViewModel
{
    public enum MenuCommand
    {
        Rectangle,
        Ellipse,
        Path,
        None
    }

    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private CheckBox strokeBox;
        private CheckBox fillBox;
        private ComboBox strokeComboBox;
        private ComboBox fillComboBox;

        public ViewModel(CheckBox stroke, CheckBox fill, ComboBox colorStroke, ComboBox colorFill, Canvas canvas)
        {
            strokeBox = stroke;
            fillBox = fill;
            strokeComboBox = colorStroke;
            fillComboBox = colorFill;
            Canvas = canvas;
        }

        private Shape selectedShape;
        public Shape SelectedShape
        {
            get { return selectedShape; }
            set { selectedShape = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedShape))); }
        }

        private int pixel = 1;
        public int Pixel
        {
            get { return pixel; }
            set
            {
                if (value > 0)
                {
                    pixel = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Pixel)));
                }
            }
        }

        private bool fillEnabled;
        public bool FillEnabled
        {
            get { return fillEnabled; }
            set
            {
                if (value != fillEnabled)
                {
                    fillEnabled = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FillEnabled)));
                }
            }
        }

        private bool strokeEnabled;
        public bool StrokeEnabled
        {
            get { return strokeEnabled; }
            set
            {
                if (value != strokeEnabled)
                {
                    strokeEnabled = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StrokeEnabled)));
                }
            }
        }


        private int selectedFillColorIndex = 4;
        public int SelectedFillColorIndex
        {
            get { return selectedFillColorIndex; }
            set
            {
                if (value > -1 && value < 7 && value != selectedFillColorIndex)
                {
                    selectedFillColorIndex = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedFillColorIndex)));
                }
            }
        }

        private int selectedStrokeColorIndex = 0;
        public int SelectedStrokeColorIndex
        {
            get { return selectedStrokeColorIndex; }
            set
            {
                if (value > -1 && value < 7 && value != selectedStrokeColorIndex)
                {
                    selectedStrokeColorIndex = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedStrokeColorIndex)));
                }
            }
        }


        public String FillColorString
        {
            get
            {
                return fillComboBox.Text;
            }
        }

        public String StrokeColorString
        {
            get
            {
                return strokeComboBox.Text;
            }
        }

        public Color FillColor
        {
            get
            {
                return (Color)ColorConverter.ConvertFromString(fillComboBox.Text);
            }
        }

        public Color StrokeColor
        {
            get
            {
                return (Color)ColorConverter.ConvertFromString(strokeComboBox.Text);
            }
        }

        private Canvas canvas;
        public Canvas Canvas
        {
            get
            {
                return canvas;
            }
            set
            {
                canvas = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Canvas)));
            }
        }

        private Key pressedKey;
        public Key PressedKey
        {
            get { return pressedKey; }
            set { pressedKey = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PressedKey))); }
        }

        private Point mouseClick;
        public Point MouseClick
        {
            get { return mouseClick; }
            set { mouseClick = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MouseClick))); }
        }

        private MenuCommand selectedMenuItem;
        public MenuCommand SelectedMenuItem
        {
            get { return selectedMenuItem; }
            set { selectedMenuItem = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedMenuItem))); }
        }

        private String pathString;
        public String PathString
        {
            get { return pathString; }
            set { pathString = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PathString))); }
        }
    }
}
