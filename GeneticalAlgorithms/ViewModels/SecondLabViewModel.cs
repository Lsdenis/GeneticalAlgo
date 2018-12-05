using System;
using System.Collections.Generic;
using System.Linq;
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
            RealItems = GeneratorHelper.Generate(MinValue, MaxValue, SolutionAccuracy, PopulationNumber);
        }

        protected override void OnNextStepClicked()
        {
            NumberOfSteps++;
            var reproduceItems = ReproductionHelper.ReproduceReal(Function, RealItems);
            var newItems = CrossingoverHelper.MakeRealCrossingover(reproduceItems, SolutionAccuracy, CrossingoverPossibility);
//            MutationHelper.MutateReal(newItems, NumberOfSteps, MaxSteps, MutationPossibility);
            RealItems = newItems;

            var maxItem = RealItems.Aggregate((i, j) =>
                Function(i.X1, i.X2) > Function(j.X1, j.X2)
                    ? i
                    : j);

            ItemValue = $"x1: {maxItem.X1}\nx2: {maxItem.X2}";
            MaxItemValueFunction = Function(maxItem.X1, maxItem.X2).ToString();
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