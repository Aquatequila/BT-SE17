using BT.ViewModel;
using Svg.Path.Operations;
using Svg.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace GUI_BT_SE17.Shapes
{
    internal sealed class ShapeMoveCommand
    {
        private ViewModel model = MainWindow.GetViewModel();
        private Path shape;

        private Point origin;
        private SvgElement source;

        public ShapeMoveCommand(SvgElement source, Point origin)
        {
            shape = model.SelectedShape;
            this.origin = origin;
            this.source = new SvgElement(source);
        }

        public Path MoveTo(Point destination)
        {
            var deltaX = destination.X - origin.X;
            var deltaY = destination.Y - origin.Y;
            var result = TransformedPathGenerator.TranslatePath(source, deltaX, deltaY);

            return Helper.TransformSvgToXamlPath(result);
        }

        public Path MoveTo(Point destination, out SvgElement svg)
        {
            var deltaX = destination.X - origin.X;
            var deltaY = destination.Y - origin.Y;
            svg = TransformedPathGenerator.TranslatePath(source, deltaX, deltaY);

            return Helper.TransformSvgToXamlPath(svg);
        }
    }
}
