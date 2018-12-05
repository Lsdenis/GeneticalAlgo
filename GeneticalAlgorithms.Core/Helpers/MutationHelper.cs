using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GeneticalAlgorithms.Core.Items;

namespace GeneticalAlgorithms.Core.Helpers
{
    public static class MutationHelper
    {
        private const int RealMutationChangeValue = 2;

        public static void Mutate(List<Item> newGenerationItems, int mutationPossibility)
        {
            if (!RandomHelper.ShouldActionBePerformed(mutationPossibility))
            {
                return;
            }

            var mutationItemIndex = RandomHelper.GetMutationItemIndex(newGenerationItems.Count);
            var itemToMutate = newGenerationItems[mutationItemIndex];
            ProceedMutation(itemToMutate);
        }

        private static void ProceedMutation(Item itemToMutate)
        {
            var numberOfGens = itemToMutate.Value.Count;
            var index = RandomHelper.GetMutationIndex(numberOfGens);
            itemToMutate.Value[index] = !itemToMutate.Value[index];
        }

        public static void MutateReal(
            List<RealItem> newGenerationItems,
            int numberOfSteps,
            int maximumNumberOfSteps,
            int mutationPossibility)
        {
            if (!RandomHelper.ShouldActionBePerformed(mutationPossibility))
            {
                return;
            }

            var mutationItemIndex = RandomHelper.GetMutationItemIndex(newGenerationItems.Count);
            var itemToMutate = newGenerationItems[mutationItemIndex];
            newGenerationItems[mutationItemIndex] = ProceedRealMutation(itemToMutate);
        }

        private static RealItem ProceedRealMutation(RealItem itemToMutate)
        {
            var realMutationValue = RandomHelper.GetRealMutationValue();
            var valueToMutate = realMutationValue == 0 ? itemToMutate.X1 : itemToMutate.X2;
            var bytes = BitConverter.GetBytes(valueToMutate);
            var bits = new BitArray(bytes);
            var mutationItemIndex = RandomHelper.GetMutationItemIndex(bits.Count);
            bits[mutationItemIndex] = !bits[mutationItemIndex];

            var bytesMutated = new byte[bytes.Length];
            bits.CopyTo(bytesMutated, 0);
            var mutatedX = BitConverter.ToDouble(bytesMutated, 0);

            var mutatedItem = realMutationValue == 0 ? new RealItem(mutatedX, itemToMutate.X2) : new RealItem(itemToMutate.X1, mutatedX);
            return mutatedItem;
        }

        //        private static void ProceedRealMutation(RealItem itemToMutate, int numberOfSteps, int maximumNumberOfSteps)
        //        {
        //            var realMutationValue = RandomHelper.GetRealMutationValue();
        //            if (realMutationValue == 0)
        //            {
        //                var a = 0;
        //                var delta = CalculateDelta(numberOfSteps, maximumNumberOfSteps, a, realMutationValue);
        //            }
        //            else
        //            {
        //                var a = 1;
        //                var delta = CalculateDelta(numberOfSteps, maximumNumberOfSteps, a, realMutationValue);
        //            }
        //        }

        private static double CalculateDelta(
            int numberOfSteps,
            int maximumNumberOfSteps,
            double mutationValue,
            int realMutationValue)
        {
            return mutationValue *
                   (1 -
                    Math.Pow(realMutationValue, (1 - numberOfSteps / maximumNumberOfSteps) * RealMutationChangeValue));
        }

        public static void MutateTSP(List<int[]> solutions, int mutationPossibility)
        {
            foreach (var solution in solutions)
            {
                if (!RandomHelper.ShouldActionBePerformed(mutationPossibility))
                {
                    ProceedTSPMutation(solution);
                }
                else
                {
                    ProceedTSPMutation2(solution);
                }
            }
        }

        private static void ProceedTSPMutation2(int[] itemToMutate)
        {
            var numberOfItems = itemToMutate.Length;

            var crossIndex1 = RandomHelper.GetTSPRecombinationIndex(0, numberOfItems - 2);
            var crossIndex2 = RandomHelper.GetTSPRecombinationIndex(crossIndex1 + 1, numberOfItems - 1);

            var tspSolution = RandomHelper.GenerateTSPSolution(crossIndex2 - crossIndex1 + 1);
            var clone = itemToMutate.ToList();

            for (var i = 0; i < tspSolution.Length; i++)
            {
                itemToMutate[i + crossIndex1] = clone[tspSolution[i] + crossIndex1];
            }
        }

        private static void ProceedTSPMutation(int[] itemToMutate)
        {
            var numberOfItems = itemToMutate.Length;

            var crossIndex1 = RandomHelper.GetTSPRecombinationIndex(0, numberOfItems - 2);
            var crossIndex2 = RandomHelper.GetTSPRecombinationIndex(crossIndex1 + 1, numberOfItems - 1);

            var listToReverse = itemToMutate.ToList();
            listToReverse.Reverse();

            for (var i = crossIndex1; i <= crossIndex2; i++)
            {
                itemToMutate[i] = listToReverse[i];
            }
        }
    }
}