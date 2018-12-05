using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using GeneticalAlgorithms.Core.Items;
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
            DataContext = new ThirdLabViewModel(DrawPath);
        }

        private const int Multiplier = 10;

        private void DrawPath(List<TSPItem> tour, int[] order)
        {
            PathGrid.Children.Clear();
            for (var i = 0; i < order.Length; i++)
            {
                var firstIndex = i;
                var nextIndex = i + 1;
                if (nextIndex == order.Length)
                {
                    nextIndex = 0;
                }

                PathGrid.Children.Add(new Line
                {
                    X1 = tour[firstIndex].X * Multiplier,
                    Y1 = tour[firstIndex].Y * Multiplier,
                    X2 = tour[nextIndex].X * Multiplier,
                    Y2 = tour[nextIndex].Y * Multiplier,
                    Stroke = Brushes.Green
                });
            }
        }
    }
}