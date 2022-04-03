namespace BayesOpt.Util
{
    public struct Variable
    {
        public readonly decimal LowerBond;
        public readonly decimal UpperBond;
        public readonly bool Integer;

        public Variable(decimal lowerBond, decimal upperBond, bool integer)
        {
            LowerBond = lowerBond;
            UpperBond = upperBond;
            Integer = integer;
        }
    }
}