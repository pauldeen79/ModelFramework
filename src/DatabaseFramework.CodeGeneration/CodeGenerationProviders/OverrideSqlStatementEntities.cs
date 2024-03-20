namespace DatabaseFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class OverrideSqlStatementEntities : DatabaseFrameworkCSharpClassBase
{
    public OverrideSqlStatementEntities(ICsharpExpressionDumper csharpExpressionDumper, IPipeline<IConcreteTypeBuilder, BuilderContext> builderPipeline, IPipeline<IConcreteTypeBuilder, BuilderExtensionContext> builderExtensionPipeline, IPipeline<IConcreteTypeBuilder, EntityContext> entityPipeline, IPipeline<IConcreteTypeBuilder, OverrideEntityContext> overrideEntityPipeline, IPipeline<TypeBaseBuilder, ReflectionContext> reflectionPipeline, IPipeline<InterfaceBuilder, InterfaceContext> interfacePipeline) : base(csharpExpressionDumper, builderPipeline, builderExtensionPipeline, entityPipeline, overrideEntityPipeline, reflectionPipeline, interfacePipeline)
    {
    }

    public override string Path => "DatabaseFramework.Domain/SqlStatements";

    protected override bool EnableEntityInheritance => true;
    protected override Class? BaseClass => CreateBaseclass(typeof(ISqlStatementBase), "DatabaseFramework.Domain");

    public override IEnumerable<TypeBase> Model
        => GetEntities(GetOverrideModels(typeof(ISqlStatementBase)), "DatabaseFramework.Domain.SqlStatements");
}
