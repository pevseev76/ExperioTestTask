using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinClient
{
    public class GraphDataContext : IGraph
    {
        private readonly DataProvider.IDataProvider clientDataProvider;
        
        public double TotalWidth { get; private set; }

        public ObservableCollection<NodeViewModel> Nodes { get; private set; }

        public ObservableCollection<RibViewModel> Ribs { get; private set; }

        public GraphDataContext(DataProvider.IDataProvider client)
        {
            clientDataProvider = client;

            Nodes = new ObservableCollection<NodeViewModel>();
            Ribs = new ObservableCollection<RibViewModel>();
        }

        public void Load()
        {
            throw new NotImplementedException();
        }
    }
}
