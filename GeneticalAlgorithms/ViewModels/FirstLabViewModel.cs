using System;
using System.Collections.Generic;
using System.Linq;
using GeneticalAlgorithms.Core.Helpers;
using GeneticalAlgorithms.Core.Items;
using OxyPlot;

namespace GeneticalAlgorithms.ViewModels
{
    public class FirstLabViewModel : MainViewModel
    {
        private List<Item> _items;

        protected override int MinValue => 0;

        protected override int MaxValue => 7;

        public override List<Item> Items
        {
            get => _items;
            set
            {
                _items = value?.OrderBy(item => item.GetDoubleValue(MinValue, MaxValue)).ToList();

                if (value == null)
                {
                    return;
                }

                ItemsSeries = _items.Select(item => new DataPoint(item.GetDoubleValue(MinValue, MaxValue),
                    Function(item.GetDoubleValue(MinValue, MaxValue)))).ToList();

                var maxItem = _items.Aggregate((i, j) =>
                    Function(i.GetDoubleValue(MinValue, MaxValue)) > Function(j.GetDoubleValue(MinValue, MaxValue))
                        ? i
                        : j);

                var doubleValue = maxItem.GetDoubleValue(MinValue, MaxValue);
                ItemValue = doubleValue.ToString();
                MaxItemValueFunction = Function(doubleValue).ToString();
            }
        }

        protected override double Function(params double[] doubleParams)
        {
            return (doubleParams[0] + 1.3) * Math.Sin(0.5 * Math.PI * doubleParams[0] + 1);
        }

        protected override void OnCalculateClicked()
        {
            NumberOfSteps = 0;
            var functionValues = new List<DataPoint>();
            for (double i = MinValue; i <= MaxValue; i += 0.1)
            {
                functionValues.Add(new DataPoint(i, Function(i)));
            }

            FunctionSeries = functionValues;

            var newGenerationItems = GeneratorHelper.Generate(
                GensHelper.GetNumberOfGens(MinValue, MaxValue, SolutionAccuracy),
                PopulationNumber);

            Items = newGenerationItems;
        }

        protected override void OnNextStepClicked()
        {
            NumberOfSteps++;
            var reproduceItems = ReproductionHelper.Reproduce(Function, Items, MinValue, MaxValue);
            var newItems = CrossingoverHelper.MakeCrossingover(reproduceItems);
            MutationHelper.Mutate(newItems);
            Items = newItems;
        }
    }
}