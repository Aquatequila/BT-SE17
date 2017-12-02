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

        public SvgElement(string tagname = "path")
        {
            Tagname    = tagname;
            Id         = ""; // unused 
            Attributes = new Dictionary<string, string>();
            Path       = new List<SvgCommand>();
        }

        public SvgElement(SvgElement svgElement, string tagname = "path")
        {
            Tagname = tagname;
            Id = ""; // unused
            Attributes = new Dictionary<string, string>();
            Path = new List<SvgCommand>();

            foreach (var command in svgElement.Path)
            {
                Path.Add(command);
            }

            foreach (var pair in svgElement.Attributes)
            {
                Attributes.Add(pair.Key, pair.Value);
            }
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
