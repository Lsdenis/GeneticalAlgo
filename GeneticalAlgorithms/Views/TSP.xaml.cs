using System.Windows;
using GeneticalAlgorithms.ViewModels;

namespace GeneticalAlgorithms.Views
{
    /// <summary>
    ///     Interaction logic for TSP.xaml
    /// </summary>
    public partial class TSP : Window
    {
        public TSP()
        {
            InitializeComponent();
            DataContext = new ThirdLabViewModel();
        }
    }
}