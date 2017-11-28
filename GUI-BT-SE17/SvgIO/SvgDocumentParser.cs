using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Media;
using System.IO;
using Svg.Wrapper;

namespace Svg.IO
{
    
    public class SvgDocumentParser // XAML may need F1 in front of every path lsdflak j
    {
        private void BitmapWrite() // save WPF canvas as file
        {
            var Canvas1 = new Canvas();
            RenderTargetBitmap bmp = new RenderTargetBitmap((int)Canvas1.Width, (int)Canvas1.Height, 96, 96, PixelFormats.Default);
            bmp.Render(Canvas1);

            var encoder = new PngBitmapEncoder();
            var outputFrame = BitmapFrame.Create(bmp);
            encoder.Frames.Add(outputFrame);

            using (var file = File.OpenWrite("TestImage.png"))
            {
                encoder.Save(file);
            }
        }

        private SvgCommandFactory factory = new SvgCommandFactory();
        public string Path { get; set; }
        private int ParseMCommand(int startIndex, List<string> splited, List<SvgCommand> result, bool isAbsolut = true)
        {
            
            double x = Double.Parse(splited[++startIndex], CultureInfo.InvariantCulture);
            double y = Double.Parse(splited[++startIndex], CultureInfo.InvariantCulture);
            //Console.WriteLine($"M {x} {y}");
            if (isAbsolut)
                result.Add(factory.MCmd(x, y));
            else
                result.Add(factory.mCmd(x, y));

            return startIndex;
        }
        private int ParseLCommand(int startIndex, List<string> splited, List<SvgCommand> result, bool isAbsolut = true)
        {
            double x = Double.Parse(splited[++startIndex], CultureInfo.InvariantCulture);
            double y = Double.Parse(splited[++startIndex], CultureInfo.InvariantCulture);

            //Console.WriteLine($"L {x} {y}");

            if (isAbsolut)
                result.Add(factory.LCmd(x, y));
            else
                result.Add(factory.lCmd(x, y));

            return startIndex;
        }

        private int ParseACommand(int startIndex, List<string> splited, List<SvgCommand> result, bool isAbsolut = true)
        {
            double x = Double.Parse(splited[++startIndex]);
            double y = Double.Parse(splited[++startIndex]);
            double rx = Double.Parse(splited[++startIndex]);
            double ry = Double.Parse(splited[++startIndex]);
            double xAxisRotation = Double.Parse(splited[++startIndex]);
            double largeArc = Double.Parse(splited[++startIndex]);
            double sweep = Double.Parse(splited[++startIndex]);

            if (isAbsolut)
                result.Add(factory.ACmd(x, y, rx, ry, xAxisRotation, largeArc, sweep));
            else
                result.Add(factory.aCmd(x, y, rx, ry, xAxisRotation, largeArc, sweep));

            return startIndex;
        }
        private int ParseQCommand(int startIndex, List<string> splited, List<SvgCommand> result, bool isAbsolut = true)
        {
            //double x, double y, double x1, double y1
            double x  = Double.Parse(splited[++startIndex]);
            double y  = Double.Parse(splited[++startIndex]);
            double x1 = Double.Parse(splited[++startIndex]);
            double y1 = Double.Parse(splited[++startIndex]);

            if (isAbsolut)
                result.Add(factory.QCmd(x, y, x1, y1));
            else
                result.Add(factory.qCmd(x, y, x1, y1));

            return startIndex;
        }

        private List<SvgCommand> ParseParth(string path)
        {
            List<string> data = path.Split(' ').ToList<string>();
            List<SvgCommand> result = new List<SvgCommand>();

            try
            {
                for (int i = 0; i < data.Count; i++)
                {
                    switch (data[i])
                    {
                        case "M": i = ParseMCommand(i, data, result); break;
                        case "m": i = ParseMCommand(i, data, result, false); break;
                        case "L": i = ParseLCommand(i, data, result); break;
                        case "l": i = ParseLCommand(i, data, result, false); break;
                        case "A": i = ParseACommand(i, data, result); break;
                        case "a": i = ParseACommand(i, data, result, false); break;
                        case "Q": i = ParseQCommand(i, data, result); break;
                        case "q": i = ParseQCommand(i, data, result, false); break;
                        case "Z": i = ParseZCommand(i, data, result); break;
                        case "z": i = ParseZCommand(i, data, result, false); break;
                        default: throw new NotSupportedException("Parsing failed : Svg command not supported or invalid");
                    }
                }
            }
            // show error in GUI window
            catch (ArgumentNullException)
            {
                Console.WriteLine("argument not found");
                throw;
            }
            catch (FormatException)
            {
                Console.WriteLine("invalid value found, path definition incorrect");
            }
            catch (OverflowException)
            {
                Console.WriteLine("parsed doublevalue too big");
            }
            catch (NotSupportedException)
            {
                Console.WriteLine("parsing failed, not supported cmd found");
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("index invalid");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return result;
        }

        private int ParseZCommand(int startIndex, List<string> splited, List<SvgCommand> result, bool isAbsolut = true)
        {
            if (isAbsolut)
                result.Add(factory.ZCmd());
            else
                result.Add(factory.zCmd());

            return startIndex++;
        }

        public SvgDocumentParser(string path)
        {
            Path = path;
        }

        public SvgWrapper Parse()
        {
            Console.WriteLine("start parsing document...");
            try
            {
                var svgWrapper = new SvgWrapper();
                var doc = new XmlDocument();
                doc.Load(Path);

                //Console.WriteLine(doc.DocumentElement.Name);

                foreach (XmlAttribute attribute in doc.DocumentElement.Attributes) // root node
                {
                    if (attribute.Name == "viewbox")
                    {
                        svgWrapper.Attributes.Remove("viewbox");
                        svgWrapper.Attributes.Add(attribute.Name, attribute.Value);
                    }
                }
                int i = 0;
                foreach (XmlNode elem in doc.DocumentElement)
                {
                    SvgElement svg = new SvgElement();
                    //Console.WriteLine(elem.Name + " : ");

                    foreach (XmlAttribute attribute in elem.Attributes)
                    {
                        svg.SetAttribute(attribute.Name, attribute.Value);
                        //Console.WriteLine(attribute.Name + "=" + attribute.Value + ", ");
                    }
                    svg.Path = ParseParth(svg.Attributes["d"]);
                    //svg.WriteToConsole();
                    svgWrapper.SetChild($"{i++}", svg);
                }
                //SVGDocumentWriter writer = new SVGDocumentWriter();
                //writer.WriteToFile("otherSvg.svg", svgWrapper);
                Console.WriteLine("finished parsing document...");
                return svgWrapper;
            }
            catch (XmlException)
            {
                Console.WriteLine("SvgDocumentParser.Parse() : Syntax error in svg file found");
                return null;
            }
        }

    }
}
