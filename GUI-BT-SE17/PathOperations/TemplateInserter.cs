using Svg.Wrapper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Svg.Path.Operations
{
    public static class TemplateInserter
    {
        private static void InsertTemplate(List<SvgCommand> template, ref SvgElement source, ref int index)
        {
            source.Path.InsertRange(index, template);
            index += template.Count - 1; // check for the rest of the line
        }

        private static PointF Init (this PointF point, SvgCommand command) 
        {
            point.X = (float) command.x;
            point.Y = (float) command.y;
            return point;
        }

        public static bool TryApplyTemplate(SvgCommand previous, SvgCommand current, int selfIndex, List<SvgCommand> template, List<SvgElement> svgs, out List<SvgCommand> result)
        {
            var elem = new SvgElement();
            elem.Path = new List<SvgCommand> { previous, current };
            int index = 1;
            //bool success = false;

            for (int i = 0; i < svgs.Count; i++) // for each svg
            {
                if (i != selfIndex) // not own lines
                {
                    SvgCommand start = new SvgCommand();
                    SvgCommand end = new SvgCommand();

                    for (int j = 0; j < svgs[i].Path.Count; j++)
                    {
                        if (j == 0)
                        {
                            end = svgs[i].Path[j];
                        }
                        else
                        {
                            start = end;
                            end = svgs[i].Path[j];
                            if (TryApplyTemplate(template, ref elem, ref index, start, end))
                            {
                                result = elem.Path;
                                return true;
                            }
                        }
                    }
                }
            }
            result =  elem.Path;
            return false;
        }

        public static bool TryApplyTemplate(List<SvgCommand> template, ref SvgElement source, ref int index, SvgCommand start, SvgCommand end)
        {
            var first = new PointF().Init(template.First());
            var last = new PointF().Init(template.Last());

            var A = new PointF().Init(start);
            var B = new PointF().Init(end);
            var C = new PointF().Init(source.Path[index- 1]);
            var D = new PointF().Init(source.Path[index]);

            if (LineInterception.TryGetPointOfIntersection(A, B, C, D, out var intersection, out var angle, out var sector))
            {
                var distance = (int) (last.X - first.X) / 2;

                var unit = LineInterception.GetDeltaUnit(C, D);

                var before = LineInterception.GetPoint(intersection, sector, distance, unit, true);
                var after = LineInterception.GetPoint(intersection, sector, distance, unit, false);

                if (!LineInterception.IsInsideOfBounds(C, D, before) || !LineInterception.IsInsideOfBounds(C, D, after))
                {
                    // the template is too large to apply
                    return false;
                }
                else // template can be applied
                {
                    // rotate
                    var tempSvg = new SvgElement { Path = template };
                    var rotated = PathMatrixOperation.RotateSvg(tempSvg, angle);
                    var i = 0;
                    // translate
                    var deltaX = before.X - rotated.Path[i].x;
                    var deltaY = before.Y - rotated.Path[i].y;
                    PathMatrixOperation.TranslatePath(ref rotated, deltaX, deltaY);
                    // replace
                    var factory = new SvgCommandFactory();
                    var cmd = factory.LCmd(before.X, before.Y);
                    rotated.Path[i] = cmd;
                    // insert
                    InsertTemplate(rotated.Path, ref source, ref index);
                    return true;
                }
            }
            else // no intersection
            {
                return false;
            }
        }

        

        
    }
}
