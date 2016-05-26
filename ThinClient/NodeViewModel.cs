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
        string ID { get; set; }
        string Label { get; set; }
        FrameworkElement Element { get; set; }
        Color Background { get; set; }
        Color Border { get; set; }
    }
}
