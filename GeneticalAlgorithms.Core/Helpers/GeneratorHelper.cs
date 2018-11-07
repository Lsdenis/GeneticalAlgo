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
    }
}