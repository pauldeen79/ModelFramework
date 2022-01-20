namespace ModelFramework.Database
{
    public partial record ViewCondition
    {
        public override string ToString() => string.IsNullOrEmpty(Combination)
            ? Expression
            : $"{Combination} {Expression}";
    }
}
