using Svg.IO;
using Svg.Path.Operations;
using Svg.Wrapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GUI_BT_SE17
{
    public class TemplateItem
    {
        public List<SvgElement> Svgs
        {
            get;
            private set;
        }
        public BoundingRectangle BoundingBox { get; }
        public int Items { get; }
        private PngBitmapEncoder png;
        public String PngPath { get; private set; }

        public TemplateItem(String path)
        {
            var parser = new SvgDocumentParser(path);
            var wrapper = parser.Parse();

            Svgs = wrapper.GetSvgElements();

            Items = Svgs.Count;
            BoundingBox = TransformedPathGenerator.CalculateBoundingRectangle(Svgs);
            Translate();
            //GenerateImage(path); // not working properly
        }

        private void Translate()
        {
            var updated = new List<SvgElement>();
            foreach (var svg in Svgs)
            {
                updated.Add(TransformedPathGenerator.TranslatePath(svg, -BoundingBox.minX, -BoundingBox.minY));
            }
            Svgs = updated;
        }


        private void GenerateImage(String path)
        {
            RenderTargetBitmap bmp = new RenderTargetBitmap((int)BoundingBox.maxX + 10, (int)BoundingBox.maxY + 10, 96, 96, PixelFormats.Default);

            var canvas = new Canvas();
            canvas.Background = new SolidColorBrush(Colors.White);
            canvas.Width = BoundingBox.maxX;
            canvas.Height = BoundingBox.maxY;

            foreach (var svg in Svgs)
            {
                Console.WriteLine(svg.Path.ToString());
                canvas.Children.Add(ShapeContainer.TransformSvgToXamlPath(svg));
            }

            bmp.Render(canvas);

            png = new PngBitmapEncoder();
            png.Frames.Add(BitmapFrame.Create(bmp));
            PngPath = path.Replace(".svg", ".png");

            using (var stm = File.Create(PngPath))
            {
                png.Save(stm);
            }
        }
    }
}
