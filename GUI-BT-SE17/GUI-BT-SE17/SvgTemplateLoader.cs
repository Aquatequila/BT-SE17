using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI_BT_SE17
{
    internal class SvgTemplateLoader
    {
        private String directory;
        public SvgTemplateLoader(String directoryPath)
        {
            directory = directoryPath;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            
        }
        public List<TemplateItem> GetItems()
        {
            var result = new List<TemplateItem>();

            var files = Directory.GetFiles(directory);

            foreach (var file in files)
            {
                result.Add(new TemplateItem(file));
            }
            return result;
        }

        public void PrintFileNames()
        {
            var files = Directory.GetFiles(directory);

            foreach (var file in files)
            {
                Console.WriteLine(file);
            }
        }

        public void GenerateImageOfItems()
        {
            var files = Directory.GetFiles(directory);

            foreach (var file in files)
            {
                new TemplateItem(file);
            }
        }

    }
}
