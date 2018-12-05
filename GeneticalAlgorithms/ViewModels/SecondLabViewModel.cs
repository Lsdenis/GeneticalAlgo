using System;
using System.Collections.Generic;
using System.Linq;
using GeneticalAlgorithms.Core.Helpers;
using GeneticalAlgorithms.Core.Items;
using OxyPlot;
using OxyPlot.Series;

namespace GeneticalAlgorithms.ViewModels
{
    public class SecondLabViewModel : MainViewModel
    {
        public SecondLabViewModel()
        {
            ChartModel = new PlotModel();

            //generate values
            var xx = ArrayBuilder.CreateVector(MinValue, MaxValue, 10);
            var yy = ArrayBuilder.CreateVector(MinValue, MaxValue, 10);
            var peaksData = ArrayBuilder.Evaluate((d, d1) => Function(d, d1), xx, yy);

            var cs = new ContourSeries
            {
                Color = OxyColors.Black,
                LabelBackground = OxyColors.White,
                ColumnCoordinates = yy,
                RowCoordinates = xx,
                Data = peaksData
            };
            ChartModel.Series.Add(cs);

            var solutions = new ScatterSeries
            {
                MarkerType = MarkerType.Circle,
                MarkerFill = OxyColor.FromRgb(0xFF, 0, 0)
            };
            ChartModel.Series.Add(solutions);

            ChartModel.Series.Add(new ScatterSeries());
        }

        protected override int MinValue => -100;

        protected override int MaxValue => 100;

        public override List<Item> Items { get; set; }

        public List<RealItem> RealItems { get; set; }

        public PlotModel ChartModel { get; }

        protected override double Function(params double[] doubleParams)
        {
            var x1 = doubleParams[0];
            var x2 = doubleParams[1];
            return Math.Pow(Math.Pow(x1, 2) + Math.Pow(x2, 2), 0.25) *
                   (Math.Pow(Math.Sin(50 * Math.Pow(Math.Pow(x1, 2) + Math.Pow(x2, 2), 0.1)), 2) + 1);
        }

        protected override void OnCalculateClicked()
        {
            NumberOfSteps = 0;
            RealItems = GeneratorHelper.Generate(MinValue, MaxValue, SolutionAccuracy, PopulationNumber);
        }

        protected override void OnNextStepClicked()
        {
            NumberOfSteps++;
            var reproduceItems = ReproductionHelper.ReproduceReal(Function, RealItems);
            var newItems =
                CrossingoverHelper.MakeRealCrossingover(reproduceItems, SolutionAccuracy, CrossingoverPossibility);
            MutationHelper.MutateReal(newItems, NumberOfSteps, MaxSteps, MutationPossibility);
            RealItems = newItems;

            var minItem = RealItems.Aggregate((i, j) =>
                Function(i.X1, i.X2) < Function(j.X1, j.X2)
                    ? i
                    : j);

            ItemValue = $"x1: {minItem.X1}\nx2: {minItem.X2}";
            var functionValue = Function(minItem.X1, minItem.X2);
            MaxItemValueFunction = functionValue.ToString();

            // ------------------------------------------------------

            for (var i = ChartModel.Series.Count - 1; i > 0; i--)
            {
                ChartModel.Series.RemoveAt(i);
            }

            var solutions = new ScatterSeries
            {
                MarkerType = MarkerType.Circle,
                MarkerFill = OxyColor.FromRgb(0, 0, 0xFF)
            };

            foreach (var p in RealItems)
            {
                var plotPoint = new ScatterPoint(p.X1, p.X2, 5);
                solutions.Points.Add(plotPoint);
            }

            ChartModel.Series.Add(solutions);

            var minValueSeries = new ScatterSeries
            {
                MarkerType = MarkerType.Circle,
                MarkerFill = OxyColor.FromRgb(0xFF, 0, 0)
            };

            minValueSeries.Points.Add(new ScatterPoint(minItem.X1, minItem.X2, 7, functionValue));
            ChartModel.Series.Add(minValueSeries);

            ChartModel.InvalidatePlot(true);
        }

        protected override void OnGenerateClicked()
        {
            for (var i = 0; i < MaxSteps; i++)
            {
                OnNextStepClicked();
            }
        }
    }
}