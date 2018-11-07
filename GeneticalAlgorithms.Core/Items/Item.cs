using System;
using System.Collections;

namespace GeneticalAlgorithms.Core.Items
{
    public class Item
    {
        public Item(BitArray value)
        {
            Value = value;
        }

        public BitArray Value { get; }

        public double GetDoubleValue(int minValue, int maxValue)
        {
            var array = new int[1];
            Value.CopyTo(array, 0);

            return minValue + array[0] * ((maxValue - minValue) / (Math.Pow(2, Value.Length) - 1));
        }
    }
}