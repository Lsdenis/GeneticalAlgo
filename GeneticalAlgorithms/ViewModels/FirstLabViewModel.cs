using System;
using System.Collections.Generic;
using GeneticalAlgorithms.Core.Helpers;
using OxyPlot;

namespace GeneticalAlgorithms.ViewModels
{
    public class FirstLabViewModel : MainViewModel
    {
        protected override int MinValue => 0;

        protected override int MaxValue => 7;

        protected override double Function(double inputValue)
        {
            return (inputValue + 1.3) * Math.Sin(0.5 * Math.PI * inputValue + 1);
        }

        protected override void OnCalculateClicked()
        {
            var functionValues = new List<DataPoint>();
            for (double i = MinValue; i < MaxValue; i += 0.1)
            {
                functionValues.Add(new DataPoint(i, Function(i)));
            }

            FunctionSeries = functionValues;

            var newGenerationItems = GeneratorHelper.Generate(GensHelper.GetNumberOfGens(MinValue, MaxValue, SolutionAccuracy),
                PopulationNumber);
            MutationHelper.Mutate(newGenerationItems);

            Items = newGenerationItems;
        }

        protected override void OnNextStepClicked()
        {
            var reproduceItems = ReproductionHelper.Reproduce(Function, Items, MinValue, MaxValue);
            Items = CrossingoverHelper.MakeCrossingover(reproduceItems);
        }
    }
}