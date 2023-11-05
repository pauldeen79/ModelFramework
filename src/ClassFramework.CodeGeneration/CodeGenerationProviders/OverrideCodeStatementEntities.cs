namespace ClassFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class OverrideCodeStatementEntities : ClassFrameworkCSharpClassBase
{
    public override string Path => $"{Constants.Paths.Domain}/CodeStatements";

    protected override bool EnableEntityInheritance => true;
    protected override bool EnableBuilderInhericance => true;
    protected override ModelFramework.Objects.Contracts.IClass? BaseClass => CreateBaseclass(typeof(ICodeStatementBase), Constants.Namespaces.Domain);

    public override object CreateModel()
        => GetImmutableClasses(GetOverrideModels(typeof(ICodeStatementBase)), $"{Constants.Namespaces.Domain}.CodeStatements")
            .OfType<ModelFramework.Objects.Contracts.IClass>()
            .Select(x => new ModelFramework.Objects.Builders.ClassBuilder(x)
                .AddMethods(new ModelFramework.Objects.Builders.ClassMethodBuilder()
                    .WithName("ToBuilder")
                    .WithOverride()
                    .WithTypeName($"{Constants.Namespaces.DomainBuilders}.CodeStatementBaseBuilder")
                    .AddLiteralCodeStatements($"return new {Constants.Namespaces.DomainBuilders}.CodeStatements.{x.Name}Builder(this);")
                )
                .BuildTyped()
            )
            .ToArray();
}
