namespace BayesOpt.Util
{
    public struct Variable
    {
        public readonly decimal LowerBond;
        public readonly decimal UpperBond;
        public readonly bool Integer;
        public readonly string NickName;

        public Variable(decimal lowerBond, decimal upperBond, bool integer, string nickName)
        {
            LowerBond = lowerBond;
            UpperBond = upperBond;
            Integer = integer;
            NickName = nickName;
        }
    }
}