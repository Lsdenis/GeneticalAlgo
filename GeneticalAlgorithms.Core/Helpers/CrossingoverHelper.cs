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
    }
}