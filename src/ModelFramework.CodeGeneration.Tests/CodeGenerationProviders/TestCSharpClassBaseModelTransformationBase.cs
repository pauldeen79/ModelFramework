namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public abstract class TestCSharpClassBaseModelTransformationBase : CSharpClassBase
{
    public override string Path => @"NotUsed";
    public override string DefaultFileName => "NotUsed";
    public override bool RecurseOnDeleteGeneratedFiles => false;

    protected override string RootNamespace => "MyNamespace";
    protected override Type RecordCollectionType => typeof(IReadOnlyCollection<>);
    protected override Type RecordConcreteCollectionType => typeof(ReadOnlyCollection<>);
    protected override bool EnableNullableContext => true;
    protected override bool CreateCodeGenerationHeader => false;

    protected override string GetFullBasePath() => throw new NotImplementedException();

    protected override Dictionary<string, string> GetBuilderNamespaceMappings() => new()
    {
        { "MyNamespace.Domain", "MyNamespace.Domain.Builders" }
    };

    protected override Dictionary<string, string> GetModelMappings() => new()
    {
        { "ModelFramework.CodeGeneration.Tests.CodeGenerationProviders.I", "MyNamespace.Domain." }
    };

    protected override string[] GetCustomBuilderTypes() => new[] { typeof(IMyBaseClass).GetEntityClassName() };

    /// <summary>
    /// Example of simple models, no inheritance
    /// </summary>
    protected ITypeBase[] GetCoreModelTransformationTypes()
        => MapCodeGenerationModelsToDomain(new[] { typeof(IMyClass) });

    /// <summary>
    /// Example of base class for use inherited type generation
    /// </summary>
    protected ITypeBase[] GetAbstractModelTransformationTypes()
        => MapCodeGenerationModelsToDomain(new[] { typeof(IMyBaseClass) });

    /// <summary>
    /// Example of inherited class for use inherited type generation
    /// </summary>
    protected ITypeBase[] GetOverrideModelTransformationTypes()
        => MapCodeGenerationModelsToDomain(new[] { typeof(IMyDerivedClass) });
}

public interface IMyClass
{
    IReadOnlyCollection<IMyClass> SubTypes { get; }
    IMyClass? ParentType { get; }
}

public interface IMyBaseClass
{
    string BaseProperty { get; }
    IReadOnlyCollection<IMyBaseClass> Children { get; }
}

public interface IMyDerivedClass : IMyBaseClass
{
    IMyClass RequiredDomainProperty { get; }
}
