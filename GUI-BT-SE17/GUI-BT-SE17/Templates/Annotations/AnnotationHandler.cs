using GUI_BT_SE17.Shapes;
using GUI_BT_SE17.ViewModels;
using Svg.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace GUI_BT_SE17.Templates.Annotations
{
    internal class AnnotationHandler
    {
        private AnnotationDrawer drawer;
        private DrawingModel drawModel = DrawingModel.GetInstance();
        private bool notInitialized = true;

        private List<int> indices;

        public AnnotationHandler(AnnotationDrawer drawer)
        {
            this.drawer = drawer ?? throw new ArgumentNullException("AnnotationHandler: drawer is null in AnnotationHandler(drawer)");
        }

        public void Start(Point mouseclick)
        {
            drawer.Start(mouseclick);
        }

        public void Update(Point mouseclick)
        {
            if (drawer.Updateable)
            {
                var paths = drawer.Update(mouseclick, out var svgs);

                if (notInitialized)
                {
                    //init, insert into model
                    notInitialized = false;
                    indices = drawModel.AddElements(paths, svgs);
                }
                else
                {
                    //update
                    drawModel.UpdateElements(indices, paths, svgs);
                }
            }
        }

        public void End()
        {
            drawModel.AddEventsFor(indices);
        }
    }
}
