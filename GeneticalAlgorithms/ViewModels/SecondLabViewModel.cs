using System;
using System.Collections.Generic;
using GeneticalAlgorithms.Core.Helpers;
using GeneticalAlgorithms.Core.Items;
using OxyPlot;

namespace GeneticalAlgorithms.ViewModels
{
    public class SecondLabViewModel : MainViewModel
    {
        protected override int MinValue => -100;

        protected override int MaxValue => 100;

        public override List<Item> Items { get; set; }

        public List<RealItem> RealItems { get; set; }

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
            var functionValues = new List<DataPoint>();
            for (double i = MinValue; i < MaxValue; i += 0.1)
            {
                functionValues.Add(new DataPoint(i, i));
            }

            FunctionSeries = functionValues;

            RealItems = GeneratorHelper.Generate(MinValue, MaxValue, SolutionAccuracy, PopulationNumber);
        }

        protected override void OnNextStepClicked()
        {
            if (NumberOfSteps == MaxSteps)
            {
                return;
            }

            NumberOfSteps++;
            var reproduceItems = ReproductionHelper.ReproduceReal(Function, RealItems);
            var newItems = CrossingoverHelper.MakeRealCrossingover(reproduceItems);
            MutationHelper.MutateReal(newItems, NumberOfSteps, MaxSteps);
            RealItems = newItems;
        }
    }
}