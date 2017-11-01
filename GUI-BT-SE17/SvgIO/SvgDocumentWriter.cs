using System.Collections.Generic;
using Svg.Wrapper;

namespace Svg.IO
{
    public class SvgDocumentWriter
    {
        private System.IO.StreamWriter document = null;

        public SvgDocumentWriter()
        {

        }

        private void WriteXmlHeader()
        {
            document?.WriteLine("<?xml version=\"1.0\"?>");
        }

        public void WriteToFile (string filenameAndPath,  SvgWrapper source)
        {
            document = new System.IO.StreamWriter(filenameAndPath);

            WriteXmlHeader();

            source.WriteToFile(writeStartTag, endTagWithContent);

            document.Close();
        }
        private void writeAttributes(Dictionary<string, string> attributes)
        {
            foreach (KeyValuePair<string, string> entry in attributes)
            {
                writeAttribute(entry.Key, entry.Value);
            }
        }
        private void writeAttribute(string name, string value)
        {
            document.Write(" {0}=\"{1}\"", name, value);
        }

        private void startTag( string tagname)
        {
            document.Write("<{0}", tagname);
        }
        private void endTagWithContent( string tagname)
        {
            document.WriteLine("</{0}>", tagname);
        }

        private void endStartTag(bool tagHasContent = false)
        {
            if (!tagHasContent)
            {
                document.Write("/");
            }
            document.WriteLine(">");
        }
        private void writeStartTag(string tagname,  Dictionary<string, string> attributes, bool hasContent)
        {
            startTag(tagname);
            writeAttributes(attributes);
            endStartTag(hasContent);
        }
    }
}
