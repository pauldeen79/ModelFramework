namespace ModelFramework.Objects.Extensions;

internal sealed record Overload
{
    public string[] ArgumentTypes { get; set; }
    public bool[] ArgumentNullables { get; set; }
    public string InitializeExpression { get; set; }
    public string MethodName { get; set; }
    public string[] ArgumentNames { get; set; }

    public Overload(string[] argumentTypes, string initializeExpression, string methodName, string[] argumentNames, bool[] argumentNullables)
    {
        if (argumentTypes.Length != argumentNullables.Length
            || argumentTypes.Length != argumentNames.Length)
        {
            throw new ArgumentException("Overload data is incorrect. Name, type and nullable information should have the same amount of items");
        }

        ArgumentTypes = argumentTypes;
        InitializeExpression = initializeExpression;
        MethodName = methodName;
        ArgumentNames = argumentNames;
        ArgumentNullables = argumentNullables;
    }

    public Argument[] GetArguments()
    {
        var result = new List<Argument>();
        for (int i = 0; i < ArgumentTypes.Length; i++)
        {
            result.Add(new Argument(ArgumentTypes[i], ArgumentNames[i], ArgumentNullables[i]));
        }
        return result.ToArray();
    }
}

internal sealed class Argument
{
    public string TypeName { get; }
    public string Name { get; }
    public bool IsNullable { get; }

    public Argument(string argumentType, string argumentName, bool argumentTypeNullable)
    {
        TypeName = argumentType;
        Name = argumentName;
        IsNullable = argumentTypeNullable;
    }
}
