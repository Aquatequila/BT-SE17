using System;
using System.Collections.Generic;

namespace Svg.Wrapper
{
    public class SvgElement
    {
        public Dictionary<string, string> Attributes { get; set; }
        public String Tagname { get; set; }
        public String Id { get; set; }
        public List<SvgCommand> Path { get; set; }
        //private Dictionary<string, SVGElement> childs;

        public SvgElement(string tagname = "path")
        {
            Tagname    = tagname;
            Id         = ""; // TODO 
            Attributes = new Dictionary<string, string>();
            Path       = new List<SvgCommand>();
            //this.childs     = new Dictionary<string, SVGElement>();
        }

        public void WriteToConsole()
        {
            Console.WriteLine("++++++++++++ ELEMENT ++++++++++++");
            Console.WriteLine(Tagname);
            foreach(var attribute in Attributes)
            {
                Console.WriteLine($"{attribute.Key}={attribute.Value}");
            }
            Console.Write("d=");
            foreach(var point in Path)
            {
                Console.Write(point.ToString() + " ");
            }

            Console.WriteLine();
            Console.WriteLine("++++++++++++   END   ++++++++++++");
        }

        public void SetAttribute(string attributeName, string attributeValue)
        {
            Attributes[attributeName] = attributeValue;
        }

        public void RemoveAttribute(string attributeName)
        {
            Attributes.Remove(attributeName);
        }

        public void WriteToFile(tagWriter writeElement, endOfTagWithContent endTag)
        {
            string d = "";
            foreach(var point in Path)
            {
                if (d != "")
                {
                    d += " ";
                }
                d += point.ToString();
            }
            Attributes["d"] = d;
            writeElement(Tagname, Attributes, false);
        }
    }
}
