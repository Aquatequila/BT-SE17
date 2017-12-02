using Svg.IO;
using Svg.Path.Operations;
using Svg.Wrapper;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Shapes;

namespace GUI_BT_SE17.Wrapper
{
    using Svg = SvgElement;
    using Transform = TransformedPathGenerator;

    public class TemplateWrapper
    {
        public Shape Shape { get; private set; }
        public Svg Template { get; private set; }
        public String ImagePath { get; }

        public SvgCommand firstCommand { get; }

        public TemplateWrapper(String templatePath, String imagePath)
        {
            ImagePath = imagePath;

            var parser = new SvgDocumentParser(templatePath);
            var wrapper = parser.Parse();

            Template = wrapper.GetSvgElements()?[0];
            if (Template == null)
            {
                throw new NullReferenceException($"Svg could not be loaded @{templatePath}");
            }

            if (Template.Path.Count < 2)
            {
                throw new ArgumentException($"Semantic error, template has not enough elements to make sense, path:{templatePath}");
            }

            firstCommand = Template.Path[0];
        }

        public List<SvgCommand> ApplyTemplate(SvgCommand start1, SvgCommand end1, SvgCommand start2, SvgCommand end2)
        {
            var result = new List<SvgCommand>();
            var copy = new List<SvgCommand>(); // working copy

            foreach (var elem in Template.Path)
            {
                copy.Add(elem);
            } 


            // TODO: calculate left and right point for distance between start and end
            // TODO: check if it is possible to apply the template based on the distances
            // TODO: calcualte intersection, check if there is an intersection
            // TODO: adjust roatation of the template

            // TODO: build result



            return result;
        }

        



    }
}
