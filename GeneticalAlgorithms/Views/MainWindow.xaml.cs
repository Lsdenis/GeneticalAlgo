using System.Windows;
using GeneticalAlgorithms.ViewModels;

namespace GeneticalAlgorithms.Views
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new SecondLabViewModel();
        }
    }
}