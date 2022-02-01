namespace ModelFramework.Objects.Extensions;

internal sealed record Overload
{
    public string ArgumentType { get; set; }
    public string InitializeExpression { get; set; }
    public string MethodName { get; set; }
    public string ArgumentName { get; set; }
    public Overload(string argumentType, string initializeExpression, string methodName, string argumentName)
    {
        ArgumentType = argumentType;
        InitializeExpression = initializeExpression;
        MethodName = methodName;
        ArgumentName = argumentName;
    }
}
