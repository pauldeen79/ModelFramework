namespace DatabaseFramework.Database.Extensions;

public static class ViewSelectConditionExtensions
{
    public static ViewSelectCondition AsFirstCondition(this ViewSelectCondition instance)
        => instance.ToBuilder().WithCombination(string.Empty).Build();
}
