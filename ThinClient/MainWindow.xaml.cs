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

namespace ThinClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public IGraph Graph { get; set;}
        
        public MainWindow()
        {
            InitializeComponent();

            DataContext = Graph;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(Graph!= null)
                Graph.Load();
        }

        private void nodesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;

            if(listBox != null)
                listBox.SelectedIndex = -1;
        }
    }
}
