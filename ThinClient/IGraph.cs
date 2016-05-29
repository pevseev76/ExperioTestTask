using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace ThinClient
{
    public interface IGraph : INotifyPropertyChanged
    {
        double TotalWidth { get; }
        ObservableCollection<NodeViewModel> Nodes { get; }
        ObservableCollection<RibViewModel> Ribs { get; }
        void LoadPoints();
        void LoadRibs();
        void PointChoosed(FrameworkElement element);
    }
}
