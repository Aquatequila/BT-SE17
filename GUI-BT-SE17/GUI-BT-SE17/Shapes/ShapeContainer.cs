using BT.ViewModel;
using GUI_BT_SE17.Shapes;
using GUI_BT_SE17.Shapes.Commands;
using GUI_BT_SE17.ViewModels;
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
        private static DrawingModel drawModel = DrawingModel.GetInstance();

        private static List<SvgCommand> newPath = new List<SvgCommand>();
        private static SvgCommandFactory factory = new SvgCommandFactory();
        private static List<bool> wasSet;
        private static int shapeIndex = 0;
        private static int pathIndex = 0;
        private static int strokeWidth;
        private static Color strokeColor;

        public static SvgElement GetSvgForShape (Shape shape)
        {
            var i = drawModel.Shapes.IndexOf(shape as Path);

            return i < 0 ? null : drawModel.Svgs[i];
        }

        public static void ReplaceShape(Shape source, Shape updated, ViewModel model, SvgElement svg)
        {
            var i = drawModel.Shapes.IndexOf(source as Path);

            if (i < 0)
                return;

            updated.MouseEnter += ShapeFunctionality.Hover;
            updated.MouseLeave += ShapeFunctionality.EndHover;
            updated.MouseLeftButtonDown += ShapeFunctionality.Click;
            drawModel.Shapes[i] = updated as Path;
            drawModel.Svgs[i] = svg;

            model.SelectedShape = updated;

            model.Canvas.Children.Remove(source);
            model.Canvas.Children.Add(updated);
        }

        public static void UpdateEntry(SvgElement updated, Shape shape)
        {
            var i = drawModel.Shapes.IndexOf(shape as Path);

            if (i > -1)
            {
                drawModel.Svgs[i] = updated;
            }
        }

        public static void MirrorVertical (Shape shape, Canvas canvas)
        {
            var path = shape as Path;
            var i = drawModel.Shapes.IndexOf(path);

            if (i > -1)
            {
                canvas.Children.Remove(shape);

                var svg = drawModel.Svgs[i];
                var bounds = SVG.CalculateBoundingRectangle(svg);
                var width = bounds.maxX - bounds.minX;
                var height = bounds.maxY - bounds.minY;
                var center = new Point(width / 2 + bounds.minX, height / 2 + bounds.minY);


                drawModel.Shapes[i] = SimpleShapeCommand.MirrorVertical(svg, center, out var result) as Path;
                drawModel.Shapes[i].MouseEnter += ShapeFunctionality.Hover;
                drawModel.Shapes[i].MouseLeave += ShapeFunctionality.EndHover;
                drawModel.Shapes[i].MouseLeftButtonDown += ShapeFunctionality.Click;
                canvas.Children.Add(drawModel.Shapes[i]);
                drawModel.Svgs[i] = result;
            }
        }

        public static void MirrorHorizontal(Shape shape, Canvas canvas)
        {
            var path = shape as Path;
            var i = drawModel.Shapes.IndexOf(path);

            if (i > -1)
            {
                canvas.Children.Remove(shape);

                var svg = drawModel.Svgs[i];
                var bounds = SVG.CalculateBoundingRectangle(svg);
                var width = bounds.maxX - bounds.minX;
                var height = bounds.maxY - bounds.minY;
                var center = new Point(width / 2 + bounds.minX, height / 2 + bounds.minY);


                drawModel.Shapes[i] = SimpleShapeCommand.MirrorHorizontal(svg, center, out var result) as Path;
                drawModel.Shapes[i].MouseEnter += ShapeFunctionality.Hover;
                drawModel.Shapes[i].MouseLeave += ShapeFunctionality.EndHover;
                drawModel.Shapes[i].MouseLeftButtonDown += ShapeFunctionality.Click;
                canvas.Children.Add(drawModel.Shapes[i]);
                drawModel.Svgs[i] = result;
            }
        }

        public static void AddElements (List<Path> graphicObjects, List<SvgElement> svgObjects)
        {
            foreach (var graphic in graphicObjects)
            {
                drawModel.Shapes.Add(graphic);

                drawModel.Shapes[shapeIndex].MouseEnter += ShapeFunctionality.Hover;
                drawModel.Shapes[shapeIndex].MouseLeave += ShapeFunctionality.EndHover;
                drawModel.Shapes[shapeIndex].MouseLeftButtonDown += ShapeFunctionality.Click;
            }
            foreach (var svg in svgObjects)
            {
                drawModel.Svgs.Add(svg);
            }
            shapeIndex++;
        } 

        public static Path RemoveLastPoint()
        {
            newPath.RemoveAt(pathIndex--);
            drawModel.Shapes[shapeIndex].Data = Geometry.Parse(GetNewPathString());
            return drawModel.Shapes[shapeIndex];
        }

        public static void LoadFromFile(string path, Canvas canvas)
        {
            var parser = new SvgDocumentParser(path);
            var wrapper = parser.Parse();

            drawModel.Svgs = wrapper.GetSvgElements();
            drawModel.Shapes = new List<Path>();
            newPath = new List<SvgCommand>();
            shapeIndex = 0;
            pathIndex = 0;

            foreach (var svg in drawModel.Svgs)
            {
                drawModel.Shapes.Add(Helper.TransformSvgToXamlPath(svg));

                drawModel.Shapes[shapeIndex].MouseEnter += ShapeFunctionality.Hover;
                drawModel.Shapes[shapeIndex].MouseLeave += ShapeFunctionality.EndHover;
                drawModel.Shapes[shapeIndex].MouseLeftButtonDown += ShapeFunctionality.Click;

                canvas.Children.Add(drawModel.Shapes[shapeIndex]);
                shapeIndex++;
            }
        }

        public static void WriteToFile(string path)
        {
            SvgDocumentWriter writer = new SvgDocumentWriter();

            SvgWrapper wrapper = new SvgWrapper();
            int i = 0;
            foreach (var svg in drawModel.Svgs)
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

        public static void SetFillColor (Shape shape)
        {
            var model = ViewModel.GetViewModel();
            var path = shape as Path;
            var svg = GetSvgForShape(shape);
            var canvas = model.Canvas;


            drawModel.Shapes[shapeIndex].Fill = new SolidColorBrush(model.FillColor);
            
        }

        public static void SetStrokeColor(Color color)
        {
            wasSet[1] = true;
            strokeColor = color;
            drawModel.Shapes[shapeIndex].Stroke = new SolidColorBrush(color);
        }
        public static void SetStrokeWidth(int px)
        {
            wasSet[2] = true;
            strokeWidth = px;
            drawModel.Shapes[shapeIndex].StrokeThickness = px;
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

            drawModel.Shapes.Add(shape);
            drawModel.Svgs.Add(new SvgElement());

            return shape;
        }
        public static Path AddLineToPath(Point mouseClick)
        {
            newPath.Add(factory.LCmd(mouseClick.X, mouseClick.Y));
            drawModel.Shapes[shapeIndex].Data = Geometry.Parse(GetNewPathString());
            pathIndex++;
            return drawModel.Shapes[shapeIndex];
        }

        public static Path UpdatePath(Point mouseClick)
        {
            if (newPath[pathIndex].type == PointType.M)
            {
                AddLineToPath(mouseClick);
            }

            newPath[pathIndex] = factory.LCmd(mouseClick.X, mouseClick.Y);
            drawModel.Shapes[shapeIndex].Data = Geometry.Parse(GetNewPathString());
            return drawModel.Shapes[shapeIndex];
        }

        public static Path EndPath(bool autoClose = false)
        {
            if (autoClose)
            {
                newPath.Add(factory.ZCmd());
            }

            var svg = drawModel.Svgs[shapeIndex];
            svg.Path = newPath;
            drawModel.Svgs.Add(svg);

            drawModel.Shapes[shapeIndex].Data = Geometry.Parse(GetNewPathString());

            if (wasSet[0])
            {
                string s = drawModel.Shapes[shapeIndex].Fill.ToString();
                svg.Attributes.Add("fill", s.Remove(1, 2));
            }
                
            else svg.Attributes.Add("fill", "none");
            if (wasSet[1])
            {
                string s = drawModel.Shapes[shapeIndex].Stroke.ToString();
                svg.Attributes.Add("stroke", s.Remove(1, 2));
            }
            if (wasSet[2])
            {
                svg.Attributes.Add("stroke-width", drawModel.Shapes[shapeIndex].StrokeThickness.ToString());
            }

            drawModel.Shapes[shapeIndex].MouseEnter += ShapeFunctionality.Hover;
            drawModel.Shapes[shapeIndex].MouseLeave += ShapeFunctionality.EndHover;
            drawModel.Shapes[shapeIndex].MouseLeftButtonDown += ShapeFunctionality.Click;

            newPath = new List<SvgCommand>();

            return drawModel.Shapes[shapeIndex++];
        }
    }
}
