using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace GUI_BT_SE17
{
    internal class TemplateLoader
    {
        private String directory;
        public TemplateLoader(String directoryPath)
        {
            directory = directoryPath;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            
        }
        public ObservableCollection<TemplateItem> GetItems()
        {
            var result = new ObservableCollection<TemplateItem>();

            var files = Directory.GetFiles(directory);

            foreach (var file in files)
            {
                result.Add(new TemplateItem(file));
            }
            return result;
        }


        //public void GenerateImageOfItems()
        //{
        //    var files = Directory.GetFiles(directory);

        //    foreach (var file in files)
        //    {
        //        new TemplateItem(file);
        //    }
        //}

    }
}
