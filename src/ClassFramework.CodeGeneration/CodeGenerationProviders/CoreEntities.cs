namespace ClassFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class CoreEntities : ClassFrameworkCSharpClassBase
{
    public override string Path => Constants.Paths.Domain;

    public override object CreateModel()
        => GetImmutableClasses(GetCoreModels(), Constants.Namespaces.Domain)
            .OfType<ModelFramework.Objects.Contracts.IClass>()
            .Select(x => new ModelFramework.Objects.Builders.ClassBuilder(x)
                .AddMethods(new ModelFramework.Objects.Builders.ClassMethodBuilder()
                    .WithName("ToBuilder")
                    .WithTypeName($"{Constants.Namespaces.DomainBuilders}.{x.Name}Builder")
                    .AddLiteralCodeStatements($"return new {Constants.Namespaces.DomainBuilders}.{x.Name}Builder(this);")
                )
                .BuildTyped()
            )
            .ToArray();
}
