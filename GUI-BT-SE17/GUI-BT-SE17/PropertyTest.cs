using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Swk5.MediaAnnotator.View
{
    public partial class PropertyTest : INotifyPropertyChanged
    {
        private String myText = "It works";

        public String MyText {
            get => myText;
            set
            {
                if (myText != value)
                {
                    myText = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MyText)));
                }
            }
        }

        public String GetIt()
        {
            return myText;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
