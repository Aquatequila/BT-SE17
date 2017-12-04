using BT.ViewModel;
using GUI_BT_SE17.Shapes;
using GUI_BT_SE17.Shapes.Commands;
using Svg.IO;
using Svg.Path.Operations;
using Svg.Wrapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GUI_BT_SE17.ViewModels
{
    using SVG = TransformedPathGenerator;

    internal class DrawingModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private static ViewModel model;

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

        public void SetViewModel()
        {
            model = ViewModel.GetViewModel();
        }

        public void LoadFromFile(string path)
        {
            var parser = new SvgDocumentParser(path);
            var temp = parser.Parse().GetSvgElements();

            foreach (var svg in temp)
            {
                var i = model.Canvas.Children.Add(Helper.TransformSvgToXamlPath(svg));
                Svgs.Insert(i, svg);

                AddEventsFor(i);
            }
        }

        internal void Add(String data)
        {
            var item = new Path();
            item.Data = Geometry.Parse(data);

            var i = model.Canvas.Children.Add(item);
            Svgs.Insert(i, new SvgElement());

            Selected = model.Canvas.Children[i] as Path;
        }

        internal void AddLineToPath(String data)
        {
            (model.Canvas.Children[SelectedIndex] as Path).Data = Geometry.Parse(data);
        }

        private List<SvgCommand> AquireCopy (IList<SvgCommand> source)
        {
            var result = new List<SvgCommand>();

            foreach (var cmd in source)
            {
                var c = cmd;
                result.Add(c);
            }
            return result;
        }

        internal void EndPath(string data, List<SvgCommand> svgCommands)
        {
            var i = SelectedIndex;
            (model.Canvas.Children[i] as Path).Data = Geometry.Parse(data);
            AddEventsFor(i);
            Svgs[i].Path = AquireCopy(svgCommands);
        }

        public void WriteToFile(string path)
        {
            SvgDocumentWriter writer = new SvgDocumentWriter();
            SvgWrapper wrapper = new SvgWrapper();

            for (var i = 0; i < Svgs.Count; i++)
            {
                wrapper.SetChild($"element {i++}", Svgs[i]);
            }
            writer.WriteToFile(path, wrapper);
        }

        public void MirrorSelectedVertical()
        {
            if (Selected == null)
                throw new ArgumentNullException("DrawingModel: Selected is null in MirrorSelectedVertical()");

            var i = GetIndexOf(Selected);

            var svg = Svgs[i];
            var bounds = SVG.CalculateBoundingRectangle(svg);
            var width = bounds.maxX - bounds.minX;
            var height = bounds.maxY - bounds.minY;
            var center = new Point(width / 2 + bounds.minX, height / 2 + bounds.minY);

            (model.Canvas.Children[i] as Path).Data = SimpleShapeCommand.MirrorVertical(svg, center, out var result).Data;
            AddEventsFor(i);
            Svgs[i] = result;
        }

        public void MirrorSelectedHorizontal()
        {
            if (Selected == null)
                throw new ArgumentNullException("DrawingModel: Selected is null in MirrorSelectedHorizontal()");

            var i = GetIndexOf(Selected);

            var svg = Svgs[i];
            var bounds = SVG.CalculateBoundingRectangle(svg);
            var width = bounds.maxX - bounds.minX;
            var height = bounds.maxY - bounds.minY;
            var center = new Point(width / 2 + bounds.minX, height / 2 + bounds.minY);

            (model.Canvas.Children[i] as Path).Data = SimpleShapeCommand.MirrorHorizontal(svg, center, out var result).Data;
            AddEventsFor(i);
            Svgs[i] = result;
        }


        private String GetSvgFillColorStringFor (int index)
        {
            return (model.Canvas.Children[index] as Path).Fill.ToString().Remove(1, 2);
        }

        private String GetSvgStrokeColorStringFor(int index)
        {
            return (model.Canvas.Children[index] as Path).Stroke.ToString().Remove(1, 2);
        }

        private double GetSvgStrokeWidthFor(int index)
        {
            return (model.Canvas.Children[index] as Path).StrokeThickness;
        }

        public void SetFillColor()
        {
            if (Selected == null)
                throw new ArgumentNullException("DrawingModel: Selected is null in SetFill(color)");
            var i = GetIndexOf(Selected);

            (model.Canvas.Children[i] as Path).Fill = new SolidColorBrush(model.FillColor);
            Svgs[i].SetAttribute("fill", GetSvgFillColorStringFor(i));
        }

        internal void UpdateSelected(Path newPath, SvgElement newSvg)
        {
            var i = SelectedIndex;

            (model.Canvas.Children[i] as Path).Data = newPath.Data;
            Svgs[i].Path = newSvg.Path;
        }

        public void SetStrokeColor()
        {
            if (Selected == null)
                throw new ArgumentNullException("DrawingModel: Selected is null in SetStrokeColor(color)");
            var i = GetIndexOf(Selected);

            (model.Canvas.Children[i] as Path).Stroke = new SolidColorBrush(model.StrokeColor);
            Svgs[i].SetAttribute("stroke", GetSvgStrokeColorStringFor(i));
        }

        public void SetStrokeWidth ()
        {
            if (Selected == null)
                throw new ArgumentNullException("DrawingModel: Selected is null in SetStrokeColor(color)");
            var i = GetIndexOf(Selected);

            (model.Canvas.Children[i] as Path).StrokeThickness = model.Pixel;
            Svgs[i].SetAttribute("stroke-width", $"{GetSvgStrokeWidthFor(i)}");
        }

        private Path selected;
        public Path Selected
        {
            get { return selected; }
            set
            {
                selected = value;
                model.SelectedShape = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Selected)));
            }
        }

        public int SelectedIndex
        {
            get
            {
                if (Selected == null)
                    throw new ArgumentNullException("DrawingModel: Selected is null in SelectedIndex property");

                return GetIndexOf(Selected);
            }
        }

        private int GetIndexOf(Path path)
        {
            var i = model.Canvas.Children.IndexOf(path);
            if (i < 0)
                throw new IndexOutOfRangeException("GetIndexFor: indx out of range in GetIndexFor(path)");

            return i;
        }

        public SvgElement SvgForShape (Path path)
        {
            return Svgs[GetIndexOf(path)];
        }

        private void AddEventsFor(int index)
        {
            model.Canvas.Children[index].MouseEnter += ShapeFunctionality.Hover;
            model.Canvas.Children[index].MouseLeave += ShapeFunctionality.EndHover;
            model.Canvas.Children[index].MouseLeftButtonDown += ShapeFunctionality.Click;
        }

        public void UpdateSvg(Path path, SvgElement updated)
        {
            Svgs[GetIndexOf(path)] = updated;
        }

        public void AddElements (List<Path> paths, List<SvgElement> svgElements)
        {
            if (paths.Count != svgElements.Count)
                throw new ArgumentException("DrawingModel: coutn of paths and svgElements differ");

            for (var i = 0; i < paths.Count; i++)
            {
                var j = model.Canvas.Children.Add(paths[i]);
                Svgs.Insert(j, svgElements[i]);
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
    }
}
