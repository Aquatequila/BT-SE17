using System;
using System.Collections.Generic;

namespace Svg.Wrapper
{
    public delegate void tagWriter(string tagname, Dictionary<string, string> attributes, bool hasContent);
    public delegate void endOfTagWithContent(string tagname);

    public class SvgWrapper
    {
        public Dictionary<string, string> Attributes { get; set; }
        private static String tagname    = "svg";
        private const String xmlns      = "http://www.w3.org/2000/svg";
        private const String xlinkns    = "http://www.w3.org/1999/xlink";
        private const String xmlVersion = "1.0";

        private Dictionary<string, SvgElement> childs;

        public SvgWrapper(int width = 500, int height = 500)
        {
            if ((width < 0) || (height < 0))
            {
                throw new ArgumentOutOfRangeException("negative values for width and/or height make no sense");
            }
            this.Attributes                 = new Dictionary<string, string>();
            this.Attributes["xmlns"]        = xmlns;
            this.Attributes["xmlns:xlink"]  = xlinkns;
            this.Attributes["viewbox"]      = String.Format("0 0 {0} {1}", width, height);
            this.childs                     = new Dictionary<string, SvgElement>();
        }

        public void SetChild(string id,  SvgElement element)
        {
            childs[id] = element;
        }
        public SvgElement GetChild(string id)
        {
            SvgElement returnValue;
            if (childs.TryGetValue(id, out returnValue))
            {
                return returnValue;
            }
            return null;
        }
        public bool RemoveChild(string id)
        {
            return childs.Remove(id);
        }

        public void WriteToFile (tagWriter writeElement, endOfTagWithContent endTag)
        {
            var dictionary = new Dictionary<String, String>();
            writeElement(tagname, Attributes, true);

            foreach (KeyValuePair<string, SvgElement> elem in childs)
            {
                elem.Value.WriteToFile(writeElement, endTag);
            }

            endTag(tagname);
        }
    }
}
