using Svg.IO;
using Svg.Path.Operations;
using Svg.Wrapper;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace SvgInserterTest
{
    class Program
    {
        private static void GeneratePath (ref List<SvgCommand> path)
        {
            var factory = new SvgCommandFactory();

            path.Add(factory.MCmd(60, 60));
            path.Add(factory.LCmd(0, 0));
        }
        private static void InsertTemplateBetween(ref List<SvgCommand> source, int index)
        {
            var path = new List<SvgCommand>();
            var factory = new SvgCommandFactory();

            path.Add(factory.LCmd(40, 40));
            path.Add(factory.MCmd(30, 30));

            source.InsertRange(index, path);
        }

        static void Main(string[] args)
        {
            SvgElement elem = new SvgElement();
            elem.SetAttribute("stroke", "black");
            elem.SetAttribute("fill", "none");
            elem.SetAttribute("stroke-width", "2");
            SvgElement elem2 = new SvgElement();
            elem2.SetAttribute("stroke", "green");
            elem2.SetAttribute("fill", "none");
            elem2.SetAttribute("stroke-width", "2");

            var template = new List<SvgCommand>();
            var factory = new SvgCommandFactory();

            template.Add(factory.MCmd(0, 0));
            template.Add(factory.LCmd(10, -10));
            template.Add(factory.LCmd(20, 0));


            var path = new List<SvgCommand>();
            GeneratePath(ref path);

            var start = factory.MCmd(30, 0);
            var end = factory.LCmd(30, 90);
            elem2.Path = new List<SvgCommand> { start, end };
            int index = 1;
            elem.Path = path;

            TemplateInserter.TryApplyTemplate(template, ref elem, ref index, start, end);

            SvgWrapper wrapper = new SvgWrapper();
            wrapper.SetChild("1", elem);
            wrapper.SetChild("2", elem2);
            SvgDocumentWriter writer = new SvgDocumentWriter();
            writer.WriteToFile("insertTest.svg", wrapper);
        }
    }
}
