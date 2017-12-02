using Svg.Wrapper;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GUI_BT_SE17.Shapes
{
    internal static class Helper
    {
        public static Path TransformSvgToXamlPath(SvgElement svg)
        {
            var path = new Path();

            if (svg.Attributes.TryGetValue("stroke", out var stroke))
            {
                string xamlColor = stroke.Insert(1, "FF");
                path.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString(xamlColor));
                Console.WriteLine(xamlColor);
            }
            if (svg.Attributes.TryGetValue("fill", out var fill))
            {
                string xamlColor = fill.Insert(1, "FF");
                path.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(xamlColor));
                Console.WriteLine(xamlColor);
            }
            if (svg.Attributes.TryGetValue("stroke-width", out var px))
            {
                path.StrokeThickness = int.Parse(px);
                Console.WriteLine(px);
            }
            string data = "F1";
            foreach (var command in svg.Path)
            {
                data += $" {command.ToString()}";
            }
            path.Data = Geometry.Parse(data);

            return path;
        }
    }
}
