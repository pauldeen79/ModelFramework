namespace ModelFramework.Objects.Extensions;

internal sealed record Overload
{
    public string[] ArgumentTypes { get; set; }
    public bool[] ArgumentNullables { get; set; }
    public string InitializeExpression { get; set; }
    public string MethodName { get; set; }
    public string[] ArgumentNames { get; set; }
    public bool[] ArgumentParamArrays { get; set; }

    public Overload(string[] argumentTypes,
                    string initializeExpression,
                    string methodName,
                    string[] argumentNames,
                    bool[] argumentNullables,
                    bool[] argumentParamArrays)
    {
        if (argumentTypes.Length != argumentNullables.Length
            || argumentTypes.Length != argumentNames.Length
            || argumentTypes.Length != argumentParamArrays.Length)
        {
            throw new ArgumentException("Overload data is incorrect. Name, type, nullable information and param array should have the same amount of items");
        }

        ArgumentTypes = argumentTypes;
        InitializeExpression = initializeExpression;
        MethodName = methodName;
        ArgumentNames = argumentNames;
        ArgumentNullables = argumentNullables;
        ArgumentParamArrays = argumentParamArrays;
    }

    public Argument[] GetArguments()
    {
        var result = new List<Argument>();
        for (int i = 0; i < ArgumentTypes.Length; i++)
        {
            result.Add(new Argument(ArgumentTypes[i], ArgumentNames[i], ArgumentNullables[i], ArgumentParamArrays[i]));
        }
        return result.ToArray();
    }
}

internal sealed class Argument
{
    public string TypeName { get; }
    public string Name { get; }
    public bool IsNullable { get; }
    public bool IsParamArray { get; }

    public Argument(string argumentType, string argumentName, bool argumentTypeNullable, bool argumentTypeParamArray)
    {
        TypeName = argumentType;
        Name = argumentName;
        IsNullable = argumentTypeNullable;
        IsParamArray = argumentTypeParamArray;
    }
}
