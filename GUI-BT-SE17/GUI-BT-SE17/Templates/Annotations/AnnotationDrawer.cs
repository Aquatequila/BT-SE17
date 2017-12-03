using BT.ViewModel;
using Svg.Path.Operations;
using Svg.Wrapper;
using System;
using System.Collections.Generic;

using System.Windows;
using System.Windows.Shapes;

namespace GUI_BT_SE17.Shapes
{
    internal class AnnotationDrawer
    {
        private Double sourceXSize;
        private Double sourceYSize;
        private Point origin;

        public bool Updateable
        {
            get; set;
        }

        public List<SvgElement> Svgs { get; set; }

        private void CopySource(List<SvgElement> source)
        {
            Svgs = new List<SvgElement>();

            foreach (var svg in source)
            {
                Svgs.Add(new SvgElement(svg));
            }
        }

        public AnnotationDrawer(TemplateItem template)
        {
            var sourceSize = template.BoundingBox;

            CopySource(template.Svgs);

            sourceXSize = sourceSize.maxX - sourceSize.minX;
            sourceYSize = sourceSize.maxY - sourceSize.minY;

            Updateable = false;
        }

        public void Start(Point mouseClick)
        {
            Updateable = true;
            origin = mouseClick;
            Svgs = TransformedPathGenerator.TranslateBy(Svgs, origin.X, origin.Y);
        }

        private double CalculateFactor(double baseLenght, double newLength)
        {
            return newLength / baseLenght;
        }

        private void GetAspectRatio(Point position, out double xRatio, out double yRatio)
        {
            var xSize = Math.Abs(origin.X - position.X);
            var ySize = Math.Abs(origin.Y - position.Y);
            
            xRatio = CalculateFactor(sourceXSize, xSize);
            yRatio = CalculateFactor(sourceYSize, ySize);
        }

        private List<Path> GenerateXAML(List<SvgElement> svgs)
        {
            var result = new List<Path>();

            foreach (var svg in svgs)
            {
                result.Add(Helper.TransformSvgToXamlPath(svg));
            }

            return result;
        } 

        public List<Path> Update(Point mouseClick, out List<SvgElement> svgs)
        {
            GetAspectRatio(mouseClick, out var xRatio, out var yRatio);

            //svgs = Svgs;
            svgs = TransformedPathGenerator.ScaleRelativeBy(Svgs, xRatio, yRatio, origin);

            if (mouseClick.X < origin.X)
                svgs = TransformedPathGenerator.MirrorVerticalRelative(svgs, origin);

            if (mouseClick.Y < origin.Y)
                svgs = TransformedPathGenerator.MirrorHorizontalRelative(svgs, origin);


            return GenerateXAML(svgs);
        }

        public List<Path> Update(Point mouseClick)
        {
            GetAspectRatio(mouseClick, out var xRatio, out var yRatio);

            var newSvg = TransformedPathGenerator.ScaleRelativeBy(Svgs, xRatio, yRatio, origin);

            if (mouseClick.X < origin.X)
                newSvg = TransformedPathGenerator.MirrorVerticalRelative(newSvg, origin);

            if (mouseClick.Y < origin.Y)
                newSvg = TransformedPathGenerator.MirrorHorizontalRelative(newSvg, origin);

            return GenerateXAML(newSvg);
        }
    }
}
