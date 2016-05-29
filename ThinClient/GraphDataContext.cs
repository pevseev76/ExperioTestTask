﻿using System;
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

        #endregion

        private const int pointsDistance = 120;
        private const int pointRadius = 60;
        private const int graphMargin = 10;

        private double totalWidth;
        private readonly DataProvider.IDataProvider dataProviderClient;

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

        public GraphDataContext(DataProvider.IDataProvider client, UIElement mainContainer)
        {
            dataProviderClient = client;

            Nodes = new ObservableCollection<NodeViewModel>();
            Ribs = new ObservableCollection<RibViewModel>();
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
                Nodes.Add(new NodeViewModel() { ID = label.Key, Element = null, NodeLabel = label.Value, 
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

            IDictionary<string, string[]> adjacentNodes = null;

            try
            {
                adjacentNodes = dataProviderClient.GetAdjacentNodes();
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
                        LineColor = new SolidColorBrush(totalColor)
                    });

                    Offset = Math.Max(Offset, Math.Max(begin.Y - GraphMargin.Top - 1,end.Y - GraphMargin.Top - 1));
                }
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
