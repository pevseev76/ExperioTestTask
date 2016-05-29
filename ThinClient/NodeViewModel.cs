using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ThinClient
{
    public class NodeViewModel : INotifyPropertyChanged
    {
        private Brush nodeBackground;
        private Brush nodeBorder;

        public event PropertyChangedEventHandler PropertyChanged;
        
        public string ID { get; set; }

        public string NodeLabel { get; set; }
        public FrameworkElement Element { get; set; }
        
        public Brush NodeBackground
        {
            get { return nodeBackground; }

            set
            {
                nodeBackground = value;
                OnPropertyChanged("NodeBackground");
            }
        }
        
        public Brush NodeBorder
        {
            get { return nodeBorder; }

            set
            {
                nodeBorder = value;
                OnPropertyChanged("NodeBorder");
            }
        }
        
        
        public double Distance { get; set; }
        public double Radius { get; set; }
        
        private void OnPropertyChanged(string propertyName)
        {
            var eh = PropertyChanged;

            if (eh != null)
                eh(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
