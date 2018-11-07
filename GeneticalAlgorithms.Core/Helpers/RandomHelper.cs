using System;
using System.Collections;
using System.Collections.Generic;
using GeneticalAlgorithms.Core.Items;

namespace GeneticalAlgorithms.Core.Helpers
{
    public static class RandomHelper
    {
        private static readonly Random Random;

        static RandomHelper()
        {
            Random = new Random(DateTime.Now.Second + DateTime.Now.Millisecond);
        }

        public static BitArray GenerateBinaryItem(int numberOfGens)
        {
            var bitArray = new BitArray(numberOfGens, false);

            for (var i = 0; i < numberOfGens; i++)
            {
                bitArray[i] = Convert.ToBoolean(Random.Next(0, 2));
            }

            return bitArray;
        }

        public static List<Item> GetReproductionItems(
            Dictionary<Item, ItemAdditionalInfo> itemToResultDictionary,
            int itemsCount)
        {
            var items = new List<Item>();

            while (items.Count < itemsCount)
            {
                var nextItemRandom = Random.NextDouble();

                double randomSum = 0;
                foreach (var itemInfo in itemToResultDictionary)
                {
                    randomSum += itemInfo.Value.NormalizedValue;
                    if (nextItemRandom > randomSum)
                    {
                        continue;
                    }

                    items.Add(itemInfo.Key);
                    break;
                }
            }

            return items;
        }

        public static List<Tuple<int, int>> GenerateCrossingoverPairs(int reproduceItemsCount)
        {
            var crossingoverPairs = new List<Tuple<int, int>>();
            var usedItems = new bool[reproduceItemsCount];

            while (crossingoverPairs.Count < reproduceItemsCount / 2)
            {
                var pairValue1 = Random.Next(0, reproduceItemsCount);
                while (usedItems[pairValue1])
                {
                    pairValue1 = Random.Next(0, reproduceItemsCount);
                }

                usedItems[pairValue1] = true;

                var pairValue2 = Random.Next(0, reproduceItemsCount);
                while (usedItems[pairValue2])
                {
                    pairValue2 = Random.Next(0, reproduceItemsCount);
                }

                usedItems[pairValue2] = true;

                crossingoverPairs.Add(new Tuple<int, int>(pairValue1, pairValue2));
            }

            return crossingoverPairs;
        }

        public static int GetCrossIndex(int numberOfGens)
        {
            return Random.Next(0, numberOfGens);
        }

        public static int GetMutationItemIndex(int numberOfItems)
        {
            return Random.Next(0, numberOfItems);
        }

        public static int GetMutationIndex(int numberOfGens)
        {
            return Random.Next(0, numberOfGens);
        }
    }
}