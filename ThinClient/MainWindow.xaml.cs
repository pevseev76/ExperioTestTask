using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Castle.Windsor;
using Registration = Castle.MicroKernel.Registration;
using System.ComponentModel;

namespace ThinClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged, IDisposable
    {
        private int nodesCounter = 0;
        private readonly WindsorContainer container = new WindsorContainer();

        public GraphDataContext Graph { get; set; }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        
        public MainWindow()
        {
            InitializeComponent();

            container.Register(Registration.Component.For<DataProvider.IDataProvider>().ImplementedBy<DataProvider.DataProviderClient>());
            
            container.Register(Registration.Component.For<CalculationShortestPath.ICalculatorShortestPath>().ImplementedBy<CalculationShortestPath
                .CalculatorShortestPathClient>());
            
            var dbClient = container.Resolve<DataProvider.IDataProvider>();
            var cspClient = container.Resolve<CalculationShortestPath.ICalculatorShortestPath>();
            Graph = new GraphDataContext(dbClient, cspClient, mainGrid);
            
            DataContext = Graph;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            nodesCounter = 0;
            
            if(Graph!= null)
                Graph.Load();
        }

        private void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;

            if(listBox != null)
                listBox.SelectedIndex = -1;
        }

        private void Button_Loaded(object sender, RoutedEventArgs e)
        {
            var element = sender as FrameworkElement;

            if (element == null)
                return;
            var node = element.DataContext as NodeViewModel;

            if (node == null)
                return;

            node.Element = element;
            nodesCounter++;

            if (nodesCounter >= Graph.Nodes.Count)
                Graph.LoadRibs();
        }

        private void btnPointClick(object sender, RoutedEventArgs e)
        {
            Graph.PointChoosed(sender as FrameworkElement);
        }

        private void btnShortestPathClick(object sender, RoutedEventArgs e)
        {
            Graph.CalculateShortestPath();
        }

        private void ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            Graph.CalculateRibs();
        }

        public void Dispose()
        {
            container.Dispose();
        }
    }
}
