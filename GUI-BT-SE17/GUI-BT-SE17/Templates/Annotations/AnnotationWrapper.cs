using GUI_BT_SE17.Shapes;
using Svg.Path.Operations;
using Svg.Wrapper;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Shapes;

namespace GUI_BT_SE17
{
    using Shapez = List<Shape>;

    public class AnnotationWrapper
    {
        private TemplateItem annotation;
        private double xBase;
        private double yBase;
        public String ImagePath { get; }

        bool upToDate = false;

        public List<SvgElement> Svgs
        {
            get;
            private set;
        }

        public AnnotationWrapper(String annotationPath, String imagePath)
        {
            annotation = new TemplateItem(annotationPath);
            ImagePath = imagePath;

            var box = annotation.BoundingBox;
            yBase = box.maxY - box.minY;
            xBase = box.maxX - box.minX;
        }

        public void TranslateBy(int x, int y)
        {
            Svgs = TransformedPathGenerator.TranslateBy(Svgs, x, y);
            upToDate = false;
        }

        private double CalculateFactor(double baseLenght, double newLength)
        {
            return newLength / baseLenght;
        }

        public void AdjustSizeTo(Point startPosition, Point endPosition)
        {
            var yNew = Math.Abs(startPosition.Y - endPosition.Y);
            var xNew = Math.Abs(startPosition.X - endPosition.X);

            var xFactor = CalculateFactor(xBase, xNew);
            var yFactor = CalculateFactor(yBase, yNew);

            Svgs = TransformedPathGenerator.ScaleBy(Svgs, xFactor, yFactor);
            upToDate = false;
        }

        private Shapez shapes;
        public Shapez Shapes
        {
            get
            {
                if (!upToDate)
                {
                    shapes = new Shapez();

                    foreach (var svg in Svgs)
                    {
                        shapes.Add(Helper.TransformSvgToXamlPath(svg));
                    }
                }
                return shapes;
            }
        }
    }
}
