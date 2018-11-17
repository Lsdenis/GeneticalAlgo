using System;
using System.Collections.Generic;
using GeneticalAlgorithms.Core.Items;

namespace GeneticalAlgorithms.Core.Helpers
{
    public static class MutationHelper
    {
        private const int RealMutationChangeValue = 2;

        public static void Mutate(List<Item> newGenerationItems)
        {
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

        public static void MutateReal(List<RealItem> newGenerationItems, int numberOfSteps, int maximumNumberOfSteps)
        {
            var mutationItemIndex = RandomHelper.GetMutationItemIndex(newGenerationItems.Count);
            var itemToMutate = newGenerationItems[mutationItemIndex];
            ProceedRealMutation(itemToMutate, numberOfSteps, maximumNumberOfSteps);
        }

        private static void ProceedRealMutation(RealItem itemToMutate, int numberOfSteps, int maximumNumberOfSteps)
        {
            
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
    }
}