using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GeneticalAlgorithms.Core.Items;

namespace GeneticalAlgorithms.Core.Helpers
{
    public static class CrossingoverHelper
    {
        private const int DefaultUnsetSolution = -1;

        public static List<Item> MakeCrossingover(List<Item> reproduceItems, int crossingoverPossibility)
        {
            var newItems = new List<Item>();
            var pairs = RandomHelper.GenerateCrossingoverPairs(reproduceItems.Count);

            foreach (var pair in pairs)
            {
                if (!RandomHelper.ShouldActionBePerformed(crossingoverPossibility))
                {
                    newItems.Add((Item) reproduceItems[pair.Item1].Clone());
                    newItems.Add((Item) reproduceItems[pair.Item2].Clone());
                }
                else
                {
                    var crossingoverItems = PerformCrossingover(reproduceItems, pair);

                    newItems.Add(crossingoverItems.Item1);
                    newItems.Add(crossingoverItems.Item2);
                }
            }

            return newItems;
        }

        private static Tuple<Item, Item> PerformCrossingover(List<Item> reproduceItems, Tuple<int, int> pair)
        {
            var itemFirst = reproduceItems[pair.Item1];
            var itemSecond = reproduceItems[pair.Item2];

            var numberOfGens = itemFirst.Value.Count;

            var crossIndex = RandomHelper.GetCrossIndex(numberOfGens);
            var crossedFirstItemData = new BitArray(numberOfGens);
            var crossedSecondItemData = new BitArray(numberOfGens);
            for (var i = 0; i < numberOfGens; i++)
            {
                if (crossIndex > i)
                {
                    crossedFirstItemData[i] = itemFirst.Value[i];
                    crossedSecondItemData[i] = itemSecond.Value[i];
                }
                else
                {
                    crossedFirstItemData[i] = itemSecond.Value[i];
                    crossedSecondItemData[i] = itemFirst.Value[i];
                }
            }

            return new Tuple<Item, Item>(new Item(crossedFirstItemData), new Item(crossedSecondItemData));
        }

        public static List<RealItem> MakeRealCrossingover(
            List<RealItem> reproduceItems,
            int solutionAccuracy,
            int crossingoverPossibility)
        {
            var newItems = new List<RealItem>();
            var pairs = RandomHelper.GenerateCrossingoverPairs(reproduceItems.Count);

            foreach (var pair in pairs)
            {
                if (!RandomHelper.ShouldActionBePerformed(crossingoverPossibility))
                {
                    newItems.Add((RealItem) reproduceItems[pair.Item1].Clone());
                    newItems.Add((RealItem) reproduceItems[pair.Item1].Clone());
                }
                else
                {
                    var crossingoverItems = PerformRealCrossingover(reproduceItems, pair, solutionAccuracy);

                    newItems.Add(crossingoverItems.Item1);
                    newItems.Add(crossingoverItems.Item2);
                }
            }

            return newItems;
        }

        private static double GetCrossedValue(double x1, double x2, double alpha)
        {
            return x1 + alpha * (x2 - x1);
        }

        private static Tuple<RealItem, RealItem> PerformRealCrossingover(
            List<RealItem> reproduceItems,
            Tuple<int, int> pair,
            int solutionAccuracy)
        {
            var itemFirst = reproduceItems[pair.Item1];
            var itemSecond = reproduceItems[pair.Item2];

            var crossIndex1 = RandomHelper.GetRecombinationIndex();
            var crossIndex2 = RandomHelper.GetRecombinationIndex();

            var firstX1 = Math.Round(GetCrossedValue(itemFirst.X1, itemSecond.X1, crossIndex1), solutionAccuracy);
            var firstX2 = Math.Round(GetCrossedValue(itemFirst.X2, itemSecond.X2, crossIndex2), solutionAccuracy);
            var secondX1 = Math.Round(GetCrossedValue(itemFirst.X1, itemSecond.X1, crossIndex1), solutionAccuracy);
            var secondX2 = Math.Round(GetCrossedValue(itemFirst.X2, itemSecond.X2, crossIndex2), solutionAccuracy);

            var crossedFirstRealItem = new RealItem(firstX1, firstX2);
            var crossedSecondRealItem = new RealItem(secondX1, secondX2);

            return new Tuple<RealItem, RealItem>(crossedFirstRealItem, crossedSecondRealItem);
        }

        public static List<int[]> MakeTSPCrossingover(List<int[]> reproduceItems)
        {
            var newItems = new List<int[]>();

            var pairs = RandomHelper.GenerateCrossingoverPairs(reproduceItems.Count);

            foreach (var pair in pairs)
            {
                var crossingoverItems = PerformTSPCrossingover(reproduceItems, pair);

                newItems.Add(crossingoverItems.Item1);
                newItems.Add(crossingoverItems.Item2);
            }

            return newItems;
        }

        private static Tuple<int[], int[]> PerformTSPCrossingover(List<int[]> reproduceItems, Tuple<int, int> pair)
        {
            var itemFirst = reproduceItems[pair.Item1];
            var itemSecond = reproduceItems[pair.Item2];

            var numberOfItems = itemFirst.Length;

            var crossIndex1 = RandomHelper.GetTSPRecombinationIndex(0, numberOfItems - 2);
            var crossIndex2 = RandomHelper.GetTSPRecombinationIndex(crossIndex1 + 1, numberOfItems - 1);

            var crossedFirstSolution = new int[numberOfItems];
            var crossedSecondSolution = new int[numberOfItems];

            for (var i = 0; i < numberOfItems; i++)
            {
                crossedFirstSolution[i] = DefaultUnsetSolution;
                crossedSecondSolution[i] = DefaultUnsetSolution;
            }

            for (var index = crossIndex1 + 1; index <= crossIndex2; index++)
            {
                crossedFirstSolution[index] = itemSecond[index];
                crossedSecondSolution[index] = itemFirst[index];
            }

            for (var index = 0; index <= crossIndex1; index++)
            {
                SetFromParent(itemFirst, crossedFirstSolution, index);
                SetFromParent(itemSecond, crossedSecondSolution, index);
            }

            for (var index = crossIndex2 + 1; index < numberOfItems; index++)
            {
                SetFromParent(itemFirst, crossedFirstSolution, index);
                SetFromParent(itemSecond, crossedSecondSolution, index);
            }

            for (var index = 0; index < numberOfItems; index++)
            {
                SetFromAnotherParent(crossedFirstSolution, itemSecond, index);
                SetFromAnotherParent(crossedSecondSolution, itemFirst, index);
            }

            return new Tuple<int[], int[]>(crossedFirstSolution, crossedSecondSolution);
        }

        private static void SetFromAnotherParent(int[] crossedSolution, int[] parent, int index)
        {
            if (crossedSolution[index] != DefaultUnsetSolution)
            {
                return;
            }

            crossedSolution[index] = parent[index];
        }

        private static void SetFromParent(int[] parent, int[] crossedSolution, int index)
        {
            var valueFromParent = parent[index];

            if (crossedSolution.All(value => value != valueFromParent))
            {
                crossedSolution[index] = valueFromParent;
            }
        }
    }
}