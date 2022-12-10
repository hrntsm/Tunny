using System;

namespace Tunny.Util
{
    [Serializable]
    public class Variable
    {
        public double LowerBond { get; }
        public double UpperBond { get; }
        public bool IsInteger { get; }
        public string NickName { get; }
        public double Epsilon { get; }
        public double Value { get; }
        public Guid InstanceId { get; }

        public Variable(
            double lowerBond, double upperBond, bool isInteger, string nickName, double epsilon,
            double value, Guid id)
        {
            LowerBond = lowerBond;
            UpperBond = upperBond;
            IsInteger = isInteger;
            NickName = nickName;
            Epsilon = epsilon;
            Value = value;
            InstanceId = id;
        }
    }
}
