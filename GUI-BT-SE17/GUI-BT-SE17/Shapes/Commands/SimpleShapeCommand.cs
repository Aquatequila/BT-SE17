using Svg.Path.Operations;
using Svg.Wrapper;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Shapes;

namespace GUI_BT_SE17.Shapes.Commands
{
    internal static class SimpleShapeCommand
    {
        public static Path MirrorHorizontal(SvgElement source, Point mirrorCenter, out SvgElement result)
        {
            result = TransformedPathGenerator.MirrorHorizontalRelative(new List<SvgElement> { source }, mirrorCenter)?[0] ?? null;

            return Helper.TransformSvgToXamlPath(result);
        }

        public static Path MirrorVertical(SvgElement source, Point mirrorCenter, out SvgElement result)
        {
            result = TransformedPathGenerator.MirrorVerticalRelative(new List<SvgElement> { source }, mirrorCenter)?[0] ?? null;

            return Helper.TransformSvgToXamlPath(result);
        }
    }
}
