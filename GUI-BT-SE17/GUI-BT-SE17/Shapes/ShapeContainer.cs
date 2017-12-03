using GUI_BT_SE17.Shapes;
using GUI_BT_SE17.Shapes.Commands;
using Svg.IO;
using Svg.Path.Operations;
using Svg.Wrapper;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GUI_BT_SE17
{
    using SVG = TransformedPathGenerator;
    public static class ShapeContainer
    {
        private static List<SvgElement> svgs = new List<SvgElement>();
        private static List<Path> shapes = new List<Path>();
        private static List<SvgCommand> newPath = new List<SvgCommand>();
        private static SvgCommandFactory factory = new SvgCommandFactory();
        private static List<bool> wasSet;
        private static int shapeIndex = 0;
        private static int pathIndex = 0;
        private static int strokeWidth;
        private static Color fillColor;
        private static Color strokeColor;

        public static void UpdateEntry(SvgElement updated, Shape shape)
        {
            var i = shapes.IndexOf(shape as Path);

            if (i > -1)
            {
                svgs[i] = updated;
            }
        }

        public static void MirrorVertical (Shape shape, Canvas canvas)
        {
            var path = shape as Path;
            var i = shapes.IndexOf(path);

            if (i > -1)
            {
                canvas.Children.Remove(shape);

                var svg = svgs[i];
                var bounds = SVG.CalculateBoundingRectangle(svg);
                var width = bounds.maxX - bounds.minX;
                var height = bounds.maxY - bounds.minY;
                var center = new Point(width / 2 + bounds.minX, height / 2 + bounds.minY);


                shapes[i] = SimpleShapeCommand.MirrorVertical(svg, center, out var result) as Path;
                shapes[i].MouseEnter += ShapeFunctionality.Hover;
                shapes[i].MouseLeave += ShapeFunctionality.EndHover;
                shapes[i].MouseLeftButtonDown += ShapeFunctionality.Click;
                canvas.Children.Add(shapes[i]);
                svgs[i] = result;
            }
        }

        public static void MirrorHorizontal(Shape shape, Canvas canvas)
        {
            var path = shape as Path;
            var i = shapes.IndexOf(path);

            if (i > -1)
            {
                canvas.Children.Remove(shape);

                var svg = svgs[i];
                var bounds = SVG.CalculateBoundingRectangle(svg);
                var width = bounds.maxX - bounds.minX;
                var height = bounds.maxY - bounds.minY;
                var center = new Point(width / 2 + bounds.minX, height / 2 + bounds.minY);


                shapes[i] = SimpleShapeCommand.MirrorHorizontal(svg, center, out var result) as Path;
                shapes[i].MouseEnter += ShapeFunctionality.Hover;
                shapes[i].MouseLeave += ShapeFunctionality.EndHover;
                shapes[i].MouseLeftButtonDown += ShapeFunctionality.Click;
                canvas.Children.Add(shapes[i]);
                svgs[i] = result;
            }
        }

        public static void AddElements (List<Path> graphicObjects, List<SvgElement> svgObjects)
        {
            foreach (var graphic in graphicObjects)
            {
                shapes.Add(graphic);

                shapes[shapeIndex].MouseEnter += ShapeFunctionality.Hover;
                shapes[shapeIndex].MouseLeave += ShapeFunctionality.EndHover;
                shapes[shapeIndex].MouseLeftButtonDown += ShapeFunctionality.Click;
            }
            foreach (var svg in svgObjects)
            {
                svgs.Add(svg);
            }
            shapeIndex++;
        } 

        public static Path RemoveLastPoint()
        {
            newPath.RemoveAt(pathIndex--);
            shapes[shapeIndex].Data = Geometry.Parse(GetNewPathString());
            return shapes[shapeIndex];
        }

        public static void LoadFromFile(string path, Canvas canvas)
        {
            var parser = new SvgDocumentParser(path);
            var wrapper = parser.Parse();

            svgs = wrapper.GetSvgElements();
            shapes = new List<Path>();
            newPath = new List<SvgCommand>();
            shapeIndex = 0;
            pathIndex = 0;

            foreach (var svg in svgs)
            {
                shapes.Add(Helper.TransformSvgToXamlPath(svg));

                shapes[shapeIndex].MouseEnter += ShapeFunctionality.Hover;
                shapes[shapeIndex].MouseLeave += ShapeFunctionality.EndHover;
                shapes[shapeIndex].MouseLeftButtonDown += ShapeFunctionality.Click;

                canvas.Children.Add(shapes[shapeIndex]);
                shapeIndex++;
            }
        }

        public static void WriteToFile(string path)
        {
            SvgDocumentWriter writer = new SvgDocumentWriter();

            SvgWrapper wrapper = new SvgWrapper();
            int i = 0;
            foreach (var svg in svgs)
            {
                wrapper.SetChild($"element {i++}", svg);
            }

            writer.WriteToFile(path, wrapper);
        }

        private static string GetNewPathString()
        {
            string path = "F1";

            foreach (var cmd in newPath)
            {
                path += $" {cmd.ToString()}";
            }
            return path;
        }

        public static void SetFillColor (Color color)
        {
            wasSet[0] = true;
            fillColor = color;
            shapes[shapeIndex].Fill = new SolidColorBrush(color);
        }

        public static void SetStrokeColor(Color color)
        {
            wasSet[1] = true;
            strokeColor = color;
            shapes[shapeIndex].Stroke = new SolidColorBrush(color);
        }
        public static void SetStrokeWidth(int px)
        {
            wasSet[2] = true;
            strokeWidth = px;
            shapes[shapeIndex].StrokeThickness = px;
        }


        public static Shape StartPath(Point mouseClick)
        {
            pathIndex = 0;
            wasSet = new List<bool>();
            wasSet.Add(false);
            wasSet.Add(false);
            wasSet.Add(false);

            newPath = new List<SvgCommand>();
            newPath.Add(factory.MCmd(mouseClick.X, mouseClick.Y));

            var shape = new Path();
            shape.Data = Geometry.Parse(GetNewPathString());

            shapes.Add(shape);

            return shape;
        }
        public static Path AddLineToPath(Point mouseClick)
        {
            newPath.Add(factory.LCmd(mouseClick.X, mouseClick.Y));
            shapes[shapeIndex].Data = Geometry.Parse(GetNewPathString());
            pathIndex++;
            return shapes[shapeIndex];
        }

        public static Path UpdatePath(Point mouseClick)
        {
            if (newPath[pathIndex].type == PointType.M)
            {
                AddLineToPath(mouseClick);
            }

            newPath[pathIndex] = factory.LCmd(mouseClick.X, mouseClick.Y);
            shapes[shapeIndex].Data = Geometry.Parse(GetNewPathString());
            return shapes[shapeIndex];
        }

        public static Path EndPath(bool autoClose = false)
        {
            if (autoClose)
            {
                newPath.Add(factory.ZCmd());
            }

            var svg = new SvgElement();
            svg.Path = newPath;
            svgs.Add(svg);

            shapes[shapeIndex].Data = Geometry.Parse(GetNewPathString());

            if (wasSet[0])
            {
                string s = shapes[shapeIndex].Fill.ToString();
                svg.Attributes.Add("fill", s.Remove(1, 2));
            }
                
            else svg.Attributes.Add("fill", "none");
            if (wasSet[1])
            {
                string s = shapes[shapeIndex].Stroke.ToString();
                svg.Attributes.Add("stroke", s.Remove(1, 2));
            }
            if (wasSet[2])
            {
                svg.Attributes.Add("stroke-width", shapes[shapeIndex].StrokeThickness.ToString());
            }

            shapes[shapeIndex].MouseEnter += ShapeFunctionality.Hover;
            shapes[shapeIndex].MouseLeave += ShapeFunctionality.EndHover;
            shapes[shapeIndex].MouseLeftButtonDown += ShapeFunctionality.Click;

            newPath = new List<SvgCommand>();

            return shapes[shapeIndex++];
        }
    }
}
