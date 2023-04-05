namespace ModelFramework.Objects.Builders;

public partial class ClassMethodBuilder
{
    public ClassMethodBuilder AddNotImplementedException() => AddLiteralCodeStatements("throw new System.NotImplementedException();");
}
