namespace DatabaseFramework.Database.Extensions;

public static class ViewConditionExtensions
{
    public static ViewCondition AsFirstCondition(this ViewCondition instance)
        => instance.ToBuilder().WithCombination(string.Empty).Build();
}
