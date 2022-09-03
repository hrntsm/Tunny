namespace Tunny.Util
{
    public struct Variable
    {
        public double LowerBond { get; }
        public double UpperBond { get; }
        public bool IsInteger { get; }
        public string NickName { get; }
        public double Epsilon { get; }

        public Variable(double lowerBond, double upperBond, bool isInteger, string nickName, double epsilon)
        {
            LowerBond = lowerBond;
            UpperBond = upperBond;
            IsInteger = isInteger;
            NickName = nickName;
            Epsilon = epsilon;
        }
    }
}
