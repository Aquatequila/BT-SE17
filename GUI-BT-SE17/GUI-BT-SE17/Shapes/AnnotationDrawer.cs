using BT.ViewModel;
using Svg.Path.Operations;
using Svg.Wrapper;
using System.Collections.Generic;

using System.Windows;
using System.Windows.Shapes;

namespace GUI_BT_SE17.Shapes
{

    internal struct AnnotationData
    {
        public int templateIndex;
        public List<SvgElement> svgs;
        public List<Path> shapes;
        public Point start;
        public Point current;
    }

    internal class AnnotationDrawer
    {
        private ViewModel model;
        AnnotationData data;

        public AnnotationDrawer(ViewModel viewModel)
        {
            model = viewModel;
        }

        private int GetSelectedIndex() => model.AnnotationIndex;

        public List<Path> Start(Point mouseClick, int templateIndex)
        {
            var template = model?.Templates?[templateIndex] ?? null;

            if (template == null)
                return null;

            data = new AnnotationData();
            data.templateIndex = templateIndex;
            data.start = mouseClick;
            data.svgs = new List<SvgElement>();
            data.shapes = new List<Path>();

            data.svgs = TransformedPathGenerator.TranslateBy(template.Svgs, mouseClick.X, mouseClick.Y);
           
            foreach (var svg in data.svgs)
            {
                data.shapes.Add(Helper.TransformSvgToXamlPath(svg));
            }

            return data.shapes;
        }

        public void Update(Point mouseClick)
        {
            data.current = mouseClick;

            //data.svgs = TransformedPathGenerator.ScaleBy(svgs, )

        }

        public void End(Point mouseClick)
        {

        }

    }
}
