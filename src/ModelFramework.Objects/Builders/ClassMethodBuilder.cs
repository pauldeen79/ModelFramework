namespace ModelFramework.Objects.Builders;

public partial class ClassMethodBuilder
{
    public ClassMethodBuilder AddNotImplementedException() => AddLiteralCodeStatements("throw new System.NotImplementedException();");

    public override string ToString() => !string.IsNullOrEmpty(ParentTypeFullName.ToString())
        ? $"{TypeName} {ParentTypeFullName}.{Name}({GetParametersString()})"
        : $"{TypeName} {Name}({GetParametersString()})";

    private string GetParametersString()
        => string.Join(", ", Parameters.Select(x => x.ToString()));
}
