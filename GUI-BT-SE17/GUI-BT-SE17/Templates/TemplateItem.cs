﻿using Svg.IO;
using Svg.Path.Operations;
using Svg.Wrapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GUI_BT_SE17
{
    public class TemplateItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public List<SvgElement> Svgs
        {
            get;
            private set;
        }
        public int Count { get; }

        public BoundingRectangle BoundingBox { get; private set; }

        //private PngBitmapEncoder png;

        private String pngPath;
        public String PngPath
        {
            get
            {
                return pngPath;
            }
            set
            {
                if (pngPath != value)
                {
                    pngPath = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PngPath)));
                }
            }
        }

        public TemplateItem(String path)
        {
            var parser = new SvgDocumentParser(path);
            var wrapper = parser.Parse();

            Svgs = wrapper.GetSvgElements();

            Count = Svgs.Count;
            Translate();
            //GenerateImage(path); // not working properly
        }

        private void Translate()
        {
            var updated = new List<SvgElement>();
            var boundingBox = TransformedPathGenerator.CalculateBoundingRectangle(Svgs);

            foreach (var svg in Svgs)
            {
                updated.Add(TransformedPathGenerator.TranslatePath(svg, -boundingBox.minX, -boundingBox.minY));
            }
            Svgs = updated;

            BoundingBox = TransformedPathGenerator.CalculateBoundingRectangle(Svgs);
        }


        //private void GenerateImage(String path) // not working ??
        //{
        //    RenderTargetBitmap bmp = new RenderTargetBitmap((int)BoundingBox.maxX + 10, (int)BoundingBox.maxY + 10, 96, 96, PixelFormats.Default);

        //    var canvas = new Canvas();
        //    canvas.Background = new SolidColorBrush(Colors.White);
        //    canvas.Width = BoundingBox.maxX;
        //    canvas.Height = BoundingBox.maxY;

        //    foreach (var svg in Svgs)
        //    {
        //        Console.WriteLine(svg.Path.ToString());
        //        canvas.Children.Add(ShapeContainer.TransformSvgToXamlPath(svg));
        //    }

        //    bmp.Render(canvas);

        //    png = new PngBitmapEncoder();
        //    png.Frames.Add(BitmapFrame.Create(bmp));
        //    PngPath = path.Replace(".svg", ".png");

        //    using (var stm = File.Create(PngPath))
        //    {
        //        png.Save(stm);
        //    }
        //}
    }
}
