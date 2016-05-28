using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace ThinClient
{
    public interface IGraph
    {
        double TotalWidth { get; }
        ObservableCollection<NodeViewModel> Nodes { get; }
        ObservableCollection<RibViewModel> Ribs { get; }
        void Load();
    }
}
