using System;

using Tunny.Util;

namespace Tunny.Input
{
    [Serializable]
    public class NumberVariable : VariableBase
    {
        public double LowerBond { get; }
        public double UpperBond { get; }
        public bool IsInteger { get; }
        public double Epsilon { get; }
        public double Value { get; }

        public NumberVariable(
            double lowerBond, double upperBond, bool isInteger, string nickName, double epsilon,
            double value, Guid id)
        : base(nickName, id)
        {
            TLog.MethodStart();
            LowerBond = lowerBond;
            UpperBond = upperBond;
            IsInteger = isInteger;
            Epsilon = epsilon;
            Value = value;
        }
    }
}
