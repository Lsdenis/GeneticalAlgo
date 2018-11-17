using System;
using System.Collections.Generic;
using GeneticalAlgorithms.Core.Items;

namespace GeneticalAlgorithms.Core
{
    public static class IntExtensions
    {
        public static double SolutionValue(this int[] solution, List<TSPItem> items)
        {
            double value = 0;

            for (var i = 1; i < solution.Length; i++)
            {
                value += Math.Sqrt(Math.Pow(items[solution[i]].X - items[solution[i - 1]].X, 2) +
                                   Math.Pow(items[solution[i]].Y - items[solution[i - 1]].Y, 2));
            }

            return value;
        }
    }
}