using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ThinClient
{
    public class GraphDataContext : INotifyPropertyChanged
    {
        #region Nodes and ribs colors

        private static readonly Color totalNodeBackground = Colors.Yellow;
        private static readonly Color totalNodeBorder = Colors.Black;

        #endregion

        private const int pointsDistance = 120;
        private const int pointRadius = 60;

        private double totalWidth;
        private readonly DataProvider.IDataProvider dataProviderClient;

        public event PropertyChangedEventHandler PropertyChanged;

        public int PointRadius 
        { 
            get { return pointRadius; } 
            set { PropertyChanged(this, new PropertyChangedEventArgs("PointRadius")); }
        }
        
        public double TotalWidth
        {
            get { return totalWidth; }

            private set
            {
                totalWidth = value;

                var eh = PropertyChanged;

                if (eh != null)
                    eh(this, new PropertyChangedEventArgs("TotalWidth"));
            }
        }

        public ObservableCollection<NodeViewModel> Nodes { get; private set; }

        public ObservableCollection<RibViewModel> Ribs { get; private set; }

        public GraphDataContext(DataProvider.IDataProvider client)
        {
            dataProviderClient = client;

            Nodes = new ObservableCollection<NodeViewModel>();
            Ribs = new ObservableCollection<RibViewModel>();
        }

        public void Load()
        {
            Nodes.Clear();
            Ribs.Clear();
            
            if (dataProviderClient == null)
                return;

            IDictionary<string, string> labels = null;

            try
            {
                labels = dataProviderClient.GetLabels();
            }
            catch (Exception)
            {
                return;
            }

            if (labels == null)
                return;

            foreach (var label in labels)
                Nodes.Add(new NodeViewModel() { ID = label.Key, Element = null, NodeLabel = label.Value, 
                    NodeBackground = new SolidColorBrush(totalNodeBackground), 
                    NodeBorder = new SolidColorBrush(totalNodeBorder),
                    Distance = pointsDistance, Radius = pointRadius });

            TotalWidth = (Math.Sqrt(Nodes.Count) + 1) * (2 * pointsDistance + pointRadius);
        }
    }
}
