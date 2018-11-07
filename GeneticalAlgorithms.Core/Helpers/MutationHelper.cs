using System.Collections.Generic;
using GeneticalAlgorithms.Core.Items;

namespace GeneticalAlgorithms.Core.Helpers
{
    public static class MutationHelper
    {
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
    }
}