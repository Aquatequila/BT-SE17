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

            path.Add(factory.MCmd(120, 100));
            path.Add(factory.LCmd(0, 0));
        }
        private static void InsertTemplateBetween(ref List<SvgCommand> source, int index)
        {
            var path = new List<SvgCommand>();
            var factory = new SvgCommandFactory();

            path.Add(factory.LCmd(120, 100));
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

            SvgElement elem3 = new SvgElement();
            elem3.SetAttribute("stroke", "red");
            elem3.SetAttribute("fill", "none");
            elem3.SetAttribute("stroke-width", "2");



            var template = new List<SvgCommand>();
            var factory = new SvgCommandFactory();

            template.Add(factory.MCmd(0, 0));
            template.Add(factory.MCmd(20, 0));


            var path = new List<SvgCommand>();
            GeneratePath(ref path);

            var start = factory.MCmd(100, 60);
            var end = factory.LCmd(100, 200);
            elem2.Path = new List<SvgCommand> { start, end };
            int index = 1;
            elem.Path = path;

            TemplateInserter.TryApplyTemplate(template, ref elem, ref index, start, end);

            var a = factory.MCmd(120, 100);
            var b = factory.LCmd(0, 0);
            elem3.Path = new List<SvgCommand> { a, b };

            SvgWrapper wrapper = new SvgWrapper();
            wrapper.SetChild("0", elem3);
            wrapper.SetChild("1", elem);
            wrapper.SetChild("2", elem2);
            SvgDocumentWriter writer = new SvgDocumentWriter();
            writer.WriteToFile("insertTest.svg", wrapper);
        }
    }
}
