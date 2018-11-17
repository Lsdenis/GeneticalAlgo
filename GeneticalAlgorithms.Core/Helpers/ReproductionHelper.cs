using System;
using System.Collections.Generic;
using System.Linq;
using GeneticalAlgorithms.Core.Items;

namespace GeneticalAlgorithms.Core.Helpers
{
    public static class ReproductionHelper
    {
        public static List<Item> Reproduce(
            Func<double[], double> function,
            List<Item> items,
            int minValue,
            int maxValue)
        {
            var itemToResultDictionary = items.ToDictionary(item => item,
                item => new ItemAdditionalInfo
                    {FunctionValue = function.Invoke(new[] {item.GetDoubleValue(minValue, maxValue)})});

            // to remove useless gens
            itemToResultDictionary = itemToResultDictionary.Where(pair => pair.Value.FunctionValue > 0).
                ToDictionary(pair => pair.Key, pair => pair.Value);

            var sumOfFunc = itemToResultDictionary.Sum(pair => Math.Abs(pair.Value.FunctionValue));

            foreach (var itemInfo in itemToResultDictionary)
            {
                itemInfo.Value.NormalizedValue = itemInfo.Value.FunctionValue / sumOfFunc;
                itemInfo.Value.ExpectedNumberOfCopies = itemInfo.Value.NormalizedValue * items.Count;
                itemInfo.Value.RealNumberOfCopies = Convert.ToInt32(Math.Round(itemInfo.Value.ExpectedNumberOfCopies));
            }

            return RandomHelper.GetReproductionItems(itemToResultDictionary, items.Count);
        }

        public static List<RealItem> ReproduceReal(
            Func<double[], double> function,
            List<RealItem> items)
        {
            var itemToResultDictionary = items.ToDictionary(item => item,
                item => new ItemAdditionalInfo
                    { FunctionValue = function.Invoke(new[] { item.X1, item.X2 }) });

            var maxFunctionValue = itemToResultDictionary.Max(pair => pair.Value.FunctionValue);
            foreach (var additionalInfo in itemToResultDictionary.Values)
            {
                additionalInfo.FunctionValue = maxFunctionValue - additionalInfo.FunctionValue;
            }

            var sumOfFunc = itemToResultDictionary.Sum(pair => Math.Abs(pair.Value.FunctionValue));

            foreach (var itemInfo in itemToResultDictionary)
            {
                itemInfo.Value.NormalizedValue = itemInfo.Value.FunctionValue / sumOfFunc;
                itemInfo.Value.ExpectedNumberOfCopies = itemInfo.Value.NormalizedValue * items.Count;
                itemInfo.Value.RealNumberOfCopies = Convert.ToInt32(Math.Round(itemInfo.Value.ExpectedNumberOfCopies));
            }

            return RandomHelper.GetReproductionRealItems(itemToResultDictionary, items.Count);
        }

        public static List<int[]> ReproduceTSP(List<TSPItem> items, List<int[]> solutions, int populationNumber)
        {
            var itemToResultDictionary = solutions.ToDictionary(solution => solution,
                solution => new ItemAdditionalInfo
                    { FunctionValue = solution.SolutionValue(items) });

            var maxFunctionValue = itemToResultDictionary.Max(pair => pair.Value.FunctionValue);
            foreach (var additionalInfo in itemToResultDictionary.Values)
            {
                additionalInfo.FunctionValue = maxFunctionValue - additionalInfo.FunctionValue;
            }

            var sumOfFunc = itemToResultDictionary.Sum(pair => Math.Abs(pair.Value.FunctionValue));

            foreach (var itemInfo in itemToResultDictionary)
            {
                itemInfo.Value.NormalizedValue = itemInfo.Value.FunctionValue / sumOfFunc;
                itemInfo.Value.ExpectedNumberOfCopies = itemInfo.Value.NormalizedValue * populationNumber;
                itemInfo.Value.RealNumberOfCopies = Convert.ToInt32(Math.Round(itemInfo.Value.ExpectedNumberOfCopies));
            }

            return RandomHelper.GetReproductionTSPItems(itemToResultDictionary, populationNumber);
        }
    }
}