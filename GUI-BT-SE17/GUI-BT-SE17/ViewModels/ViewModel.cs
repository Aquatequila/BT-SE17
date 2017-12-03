﻿using GUI_BT_SE17;
using GUI_BT_SE17.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BT.ViewModel
{
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private CheckBox strokeBox;
        private CheckBox fillBox;
        private ComboBox strokeComboBox;
        private ComboBox fillComboBox;

        public ObservableCollection<TemplateItem> Annotations
        {
            get
            {
                return templates;
            }
            set
            {
                templates = value;
            }
        }

        public TemplateItem SelectedAnnotation
        {
            get
            {
                return Annotations[AnnotationIndex];
            }
        }

        private int annotationIndex;
        public int AnnotationIndex
        {
            get
            {
                return annotationIndex;
            }
            set
            {
                if (value != annotationIndex)
                {
                    annotationIndex = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AnnotationIndex)));
                }
            }
        }

        private ObservableCollection<TemplateItem> templates;

        private String templatePath = @"../../../Templates";
        private String patternPath = @"../../../Patterns";
        private TemplateLoader patterns;
        private TemplateLoader templateLoader;

        public ViewModel(CheckBox stroke, CheckBox fill, ComboBox colorStroke, ComboBox colorFill, Canvas canvas)
        {
            strokeBox = stroke;
            fillBox = fill;
            strokeComboBox = colorStroke;
            fillComboBox = colorFill;
            Canvas = canvas;

            templateLoader = new TemplateLoader(templatePath);
            patterns = new TemplateLoader(patternPath);

            Annotations = templateLoader.GetItems();
            Annotations[0].PngPath = @"/../../Pictures/test.png";
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

                    if (selectedShape != null)
                        SelectedShape.StrokeThickness = pixel;

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

                    if (selectedShape != null)
                    {
                        if (fillEnabled == true)
                            selectedShape.Fill = new SolidColorBrush(FillColor);
                        else
                            selectedShape.Fill = null;
                    }
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

                    if (selectedShape != null)
                    {
                        if (strokeEnabled == true)
                            selectedShape.Stroke = new SolidColorBrush(StrokeColor);
                        else
                            selectedShape.Stroke = null;
                    }
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

                    FillEnabled = true;
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

                    StrokeEnabled = true;

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

        private Operation selectedMenuItem;
        public Operation SelectedMenuItem
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