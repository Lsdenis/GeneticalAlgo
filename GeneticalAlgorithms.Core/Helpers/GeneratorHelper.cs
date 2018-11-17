using System.Collections.Generic;
using GeneticalAlgorithms.Core.Items;

namespace GeneticalAlgorithms.Core.Helpers
{
    public static class GeneratorHelper
    {
        public static List<Item> Generate(
            int numberOfGens,
            int populationNumber)
        {
            var population = new List<Item>();

            for (var i = 0; i < populationNumber; i++)
            {
                population.Add(new Item(RandomHelper.GenerateBinaryItem(numberOfGens)));
            }

            return population;
        }

        public static List<RealItem> Generate(
            int minValue,
            int maxValue,
            int accuracy,
            int populationNumber)
        {
            var population = new List<RealItem>();

            for (var i = 0; i < populationNumber; i++)
            {
                population.Add(new RealItem(RandomHelper.GenerateDoubleForItem(minValue, maxValue, accuracy),
                    RandomHelper.GenerateDoubleForItem(minValue, maxValue, accuracy)));
            }

            return population;
        }

        public static List<int[]> GenerateTSPSolutions(int count, int populationNumber)
        {
            var generate = new List<int[]>();

            for (var i = 0; i < populationNumber; i++)
            {
                generate.Add(RandomHelper.GenerateTSPSolution(count));
            }

            return generate;
        }
    }
}