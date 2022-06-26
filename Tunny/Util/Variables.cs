namespace Tunny.Util
{
    public struct Variable
    {
        public decimal LowerBond { get; }
        public decimal UpperBond { get; }
        public bool IsInteger { get; }
        public string NickName { get; }

        public Variable(decimal lowerBond, decimal upperBond, bool isInteger, string nickName)
        {
            LowerBond = lowerBond;
            UpperBond = upperBond;
            IsInteger = isInteger;
            NickName = nickName;
        }
    }
}
