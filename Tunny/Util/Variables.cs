namespace Tunny.Util
{
    public struct Variable
    {
        public double LowerBond { get; }
        public double UpperBond { get; }
        public bool IsInteger { get; }
        public string NickName { get; }

        public Variable(double lowerBond, double upperBond, bool isInteger, string nickName)
        {
            LowerBond = lowerBond;
            UpperBond = upperBond;
            IsInteger = isInteger;
            NickName = nickName;
        }
    }
}
