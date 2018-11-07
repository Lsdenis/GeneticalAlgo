using System;

namespace GeneticalAlgorithms.Core.Helpers
{
    public static class GensHelper
    {
        public static int GetNumberOfGens(int minValue, int maxValue, int solutionAccuracy)
        {
            var numberOfEqualParts = (int) ((maxValue - minValue) * Math.Pow(10, solutionAccuracy));
            var numberOfGens = 0;

            while (Math.Pow(2, numberOfGens) < numberOfEqualParts)
            {
                numberOfGens++;
            }

            return numberOfGens;
        }
    }
}