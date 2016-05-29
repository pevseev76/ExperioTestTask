using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ThinClient
{
    public class NodeViewModel
    {
        public string ID { get; set; }
        public string NodeLabel { get; set; }
        public FrameworkElement Element { get; set; }
        public Brush NodeBackground { get; set; }
        public Brush NodeBorder { get; set; }
        public double Distance { get; set; }
        public double Radius { get; set; }
    }
}
