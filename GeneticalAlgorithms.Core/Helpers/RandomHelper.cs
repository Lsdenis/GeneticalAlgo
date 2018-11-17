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

        public static double GenerateDoubleForItem(int minValue, int maxValue, int accuracy)
        {
            var intValue = Random.Next(minValue, maxValue + 1);
            var afterDotValue = Random.Next(Convert.ToInt32(Math.Pow(10, accuracy) - 1));

            return Convert.ToDouble($"{intValue}.{afterDotValue}");
        }

        public static List<RealItem> GetReproductionRealItems(
            Dictionary<RealItem, ItemAdditionalInfo> itemToResultDictionary,
            int itemsCount)
        {
            var items = new List<RealItem>();

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

        public static double GetRecombinationIndex()
        {
            return Random.NextDouble();
        }

        public static int GetRealMutationValue()
        {
            return Random.Next(0, 2);
        }

        public static int[] GenerateTSPSolution(int count)
        {
            var solution = new int[count];
            var usedCities = new bool[count];

            for (var i = 0; i < count; i++)
            {
                var nextCity = Random.Next(0, count);
                while (usedCities[nextCity])
                {
                    nextCity = Random.Next(0, count);
                }

                solution[i] = nextCity;
            }

            return solution;
        }

        public static List<int[]> GetReproductionTSPItems(
            Dictionary<int[], ItemAdditionalInfo> itemToResultDictionary,
            int itemsCount)
        {
            var items = new List<int[]>();

            while (items.Count < itemsCount)
            {
                var nextItemRandom = Random.NextDouble();

                double randomSum = 0;
                foreach (var itemInfo in itemToResultDictionary)
                {
                    randomSum += itemInfo.Value.NormalizedValue;
                    if (nextItemRandom > randomSum ||
                        itemInfo.Value.RealNumberOfCopies == itemInfo.Value.CurrentNumberOfCopies &&
                        itemInfo.Value.RealNumberOfCopies != 0)
                    {
                        continue;
                    }

                    items.Add(itemInfo.Key);
                    itemInfo.Value.CurrentNumberOfCopies++;
                    break;
                }
            }

            return items;
        }

        public static int GetTSPRecombinationIndex(int begin, int end)
        {
            return Random.Next(begin, end + 1);
        }
    }
}