using System;

namespace GeneticalAlgorithms.Core.Items
{
    public class RealItem : ICloneable
    {
        public RealItem(double x1, double x2)
        {
            X1 = x1;
            X2 = x2;
        }

        public double X1 { get; }

        public double X2 { get; }

        public object Clone()
        {
            return new RealItem(X1, X2);
        }
    }
}