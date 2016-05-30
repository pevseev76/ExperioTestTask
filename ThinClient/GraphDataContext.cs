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
        private static readonly Color totalColor = Colors.Black;
        private static readonly Color firstPointColor = Colors.Violet;
        private static readonly Color secondPointColor = Colors.LightBlue;
        private static readonly Color pathColor = Colors.Green;
        private static readonly Color noPathColor = Colors.Red;

        #endregion

        private const int pointsDistance = 120;
        private const int pointRadius = 60;
        private const int graphMargin = 10;
        
        private double totalWidth;
        private readonly DataProvider.IDataProvider dataProviderClient;
        private readonly CalculationShortestPath.ICalculatorShortestPath calculatorShortestPathClient;

        public event PropertyChangedEventHandler PropertyChanged;

        private readonly UIElement mainContainer = null;
        
        public double TotalWidth
        {
            get { return totalWidth; }

            private set
            {
                totalWidth = value;

                OnPropertyChanged("TotalWidth");
            }
        }

        public Thickness GraphMargin
        {
            get { return new Thickness(graphMargin); }
            private set { OnPropertyChanged("GraphMargin"); }
        }

        public ObservableCollection<NodeViewModel> Nodes { get; private set; }

        public ObservableCollection<RibViewModel> Ribs { get; private set; }

        public List<string> ChoosedPoints { get; set; }

        public GraphDataContext(DataProvider.IDataProvider dbClient, CalculationShortestPath.ICalculatorShortestPath cspClient , UIElement mainContainer)
        {
            dataProviderClient = dbClient;
            calculatorShortestPathClient = cspClient;

            Nodes = new ObservableCollection<NodeViewModel>();
            Ribs = new ObservableCollection<RibViewModel>();
            ChoosedPoints = new List<string>();
            this.mainContainer = mainContainer;
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
                Nodes.Add(new NodeViewModel() { ID = label.Key.Trim(), Element = null, NodeLabel = label.Value, 
                    NodeBackground = new SolidColorBrush(totalNodeBackground), 
                    NodeBorder = new SolidColorBrush(totalColor),
                    Distance = pointsDistance, Radius = pointRadius });

            TotalWidth = (Math.Sqrt(Nodes.Count) + 1) * (2 * pointsDistance + pointRadius);
            GraphMargin = new Thickness();
        }

        public void LoadRibs()
        {
            Ribs.Clear();

            if (dataProviderClient == null)
                return;

            IDictionary<string, string[]> adjacentNodes = new Dictionary<string, string[]>();

            try
            {
                var whiteNodes = dataProviderClient.GetAdjacentNodes();

                foreach (var whiteNode in whiteNodes)
                    adjacentNodes[whiteNode.Key.Trim()] = whiteNode.Value.Select(x => x.Trim()).ToArray();
            }
            catch (Exception)
            {
                return;
            }

            if (adjacentNodes == null)
                return;

            double Offset = 0.0;
            
            for (int i = 0; i < Nodes.Count; i++)
            {
                string id = Nodes[i].ID;

                if (!adjacentNodes.Keys.Contains(id))
                    continue;
                
                var adjNodes = adjacentNodes[id];

                foreach (var adjNode in adjNodes)
                {
                    NodeViewModel nodeIn;

                    try
                    {
                        nodeIn = Nodes.Where(x => string.Equals(x.ID, adjNode)).First();
                    }
                    catch (Exception)
                    {
                        continue;
                    }

                    var firstElement = Nodes[i].Element;
                    var secondElement = nodeIn.Element;
                    var begin = firstElement.TranslatePoint(new Point(firstElement.Width / 2, firstElement.Height), mainContainer);
                    var end = secondElement.TranslatePoint(new Point(secondElement.Width / 2, 0), mainContainer);
                    
                    Ribs.Add(new RibViewModel()
                    {
                        BeginX = begin.X - GraphMargin.Left,
                        BeginY = begin.Y - GraphMargin.Top - 1 - Offset,
                        EndX = end.X - GraphMargin.Left,
                        EndY = end.Y - GraphMargin.Top - 1 - Offset,
                        LineColor = new SolidColorBrush(totalColor),
                        BeginId = Nodes[i].ID,
                        EndId = nodeIn.ID

                    });

                    Offset = Math.Max(Offset, Math.Max(begin.Y - GraphMargin.Top - 1,end.Y - GraphMargin.Top - 1));
                }
            }
        }

        public void PointChoosed(FrameworkElement element)
        {
            var node = element.DataContext as NodeViewModel;

            if (node == null)
                return;
            
            if (ChoosedPoints.Count == 0)
            {
                for (int i = 0; i < Nodes.Count; i++)
                {
                    Nodes[i].NodeBackground = new SolidColorBrush(totalNodeBackground);
                    Nodes[i].NodeBorder = new SolidColorBrush(totalColor);
                }

                for (int i = 0; i < Ribs.Count; i++)
                    Ribs[i].LineColor = new SolidColorBrush(totalColor);
                
                ChoosedPoints.Add(node.ID);
                node.NodeBackground = new SolidColorBrush(firstPointColor);
                return;
            }

            if (ChoosedPoints.Count == 1)
            {
                ChoosedPoints.Add(node.ID);
                node.NodeBackground = new SolidColorBrush(secondPointColor);
                return; 
            }

            if (ChoosedPoints.Count == 2)
            {
                var firstNode = Nodes.Where(x => string.Equals(x.ID, ChoosedPoints[0])).First();
                firstNode.NodeBackground = new SolidColorBrush(totalNodeBackground);
                ChoosedPoints.RemoveAt(0);

                var secondNode = Nodes.Where(x => string.Equals(x.ID, ChoosedPoints[0])).First();
                secondNode.NodeBackground = new SolidColorBrush(firstPointColor);
                
                ChoosedPoints.Add(node.ID);
                node.NodeBackground = new SolidColorBrush(secondPointColor);

            }
        }

        public void CalculateShortestPath()
        {
            if (ChoosedPoints.Count < 2)
                return;

            var path = calculatorShortestPathClient.CalculateShortestPath(ChoosedPoints[0], ChoosedPoints[1]);

            if (path == null)
            {
                for (int i = 0; i < ChoosedPoints.Count; i++)
                {
                    var node = Nodes.Where(x => string.Equals(x.ID, ChoosedPoints[i])).First();

                    if (node == null)
                        continue;

                    node.NodeBackground = new SolidColorBrush(noPathColor);
                }

                ChoosedPoints.Clear();

                return;
            }

            for (int i = 0; i < path.Length - 1; i++)
            {
                if (i > 0)
                {
                    var node = Nodes.Where(x => string.Equals(x.ID, path[i])).First();

                    if (node == null)
                        continue;

                    node.NodeBackground = new SolidColorBrush(pathColor);
                    node.NodeBorder = new SolidColorBrush(pathColor);
                }

                var rib = Ribs.Where(x => string.Equals(x.BeginId, path[i]) && string.Equals(x.EndId, path[i + 1])).First();
                rib.LineColor = new SolidColorBrush(pathColor);
                ChoosedPoints.Clear();
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            var eh = PropertyChanged;

            if (eh != null)
                eh(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
