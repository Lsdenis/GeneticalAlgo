using System;
using System.Collections;
using System.Collections.Generic;
using GeneticalAlgorithms.Core.Items;

namespace GeneticalAlgorithms.Core.Helpers
{
    public static class CrossingoverHelper
    {
        public static List<Item> MakeCrossingover(List<Item> reproduceItems)
        {
            var newItems = new List<Item>();
            var pairs = RandomHelper.GenerateCrossingoverPairs(reproduceItems.Count);

            foreach (var pair in pairs)
            {
                var crossingoverItems = PerformCrossingover(reproduceItems, pair);

                newItems.Add(crossingoverItems.Item1);
                newItems.Add(crossingoverItems.Item2);
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

        public static List<RealItem> MakeRealCrossingover(List<RealItem> reproduceItems)
        {
            var newItems = new List<RealItem>();
            var pairs = RandomHelper.GenerateCrossingoverPairs(reproduceItems.Count);

            foreach (var pair in pairs)
            {
                var crossingoverItems = PerformRealCrossingover(reproduceItems, pair);

                newItems.Add(crossingoverItems.Item1);
                newItems.Add(crossingoverItems.Item2);
            }

            return newItems;
        }

        private static double GetCrossedValue(double x1, double x2, double alpha)
        {
            return x1 + alpha * (x2 - x1);
        }

        private static Tuple<RealItem, RealItem> PerformRealCrossingover(
            List<RealItem> reproduceItems,
            Tuple<int, int> pair)
        {
            var itemFirst = reproduceItems[pair.Item1];
            var itemSecond = reproduceItems[pair.Item2];

            var crossIndex1 = RandomHelper.GetRecombinationIndex();
            var crossIndex2 = RandomHelper.GetRecombinationIndex();

            var firstX1 = GetCrossedValue(itemFirst.X1, itemSecond.X1, crossIndex1);
            var firstX2 = GetCrossedValue(itemFirst.X2, itemSecond.X2, crossIndex2);
            var secondX1 = GetCrossedValue(itemFirst.X1, itemSecond.X1, crossIndex1);
            var secondX2 = GetCrossedValue(itemFirst.X2, itemSecond.X2, crossIndex2);

            var crossedFirstRealItem = new RealItem(firstX1, firstX2);
            var crossedSecondRealItem = new RealItem(secondX1, secondX2);

            return new Tuple<RealItem, RealItem>(crossedFirstRealItem, crossedSecondRealItem);
        }
    }
}