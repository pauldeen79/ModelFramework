namespace ClassFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class OverrideTypeEntities : ClassFrameworkCSharpClassBase
{
    public override string Path => $"{Constants.Paths.Domain}/Types";

    protected override bool EnableEntityInheritance => true;
    protected override bool EnableBuilderInhericance => true;
    protected override ModelFramework.Objects.Contracts.IClass? BaseClass => CreateBaseclass(typeof(ITypeBase), Constants.Namespaces.Domain);

    public override object CreateModel()
        => GetImmutableClasses(GetOverrideModels(typeof(ITypeBase)), $"{Constants.Namespaces.Domain}.Types")
            .OfType<ModelFramework.Objects.Contracts.IClass>()
            .Select(x => new ModelFramework.Objects.Builders.ClassBuilder(x)
                .AddMethods(new ModelFramework.Objects.Builders.ClassMethodBuilder()
                    .WithName("ToBuilder")
                    .WithOverride()
                    .WithTypeName($"{Constants.Namespaces.DomainBuilders}.TypeBaseBuilder")
                    .AddLiteralCodeStatements(x.Name.EndsWith("Base")
                        ? $"throw new {typeof(NotSupportedException).FullName}(\"You can't convert a base class to builder\");"
                        : $"return new {Constants.Namespaces.DomainBuilders}.Types.{x.Name}Builder(this);")
                )
                .BuildTyped()
            )
            .ToArray();
}
