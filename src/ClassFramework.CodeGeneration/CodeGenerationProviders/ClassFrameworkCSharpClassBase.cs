namespace ClassFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public abstract class ClassFrameworkCSharpClassBase : CSharpClassBase
{
    public override bool RecurseOnDeleteGeneratedFiles => false;
    public override string DefaultFileName => string.Empty; // not used because we're using multiple files, but it's abstract so we need to fill ilt

    protected override bool CreateCodeGenerationHeader => true;
    protected override bool EnableNullableContext => true;
    protected override Type RecordCollectionType => typeof(IReadOnlyCollection<>);
    protected override Type RecordConcreteCollectionType => typeof(ReadOnlyValueCollection<>);
    protected override string FileNameSuffix => Constants.TemplateGenerated;
    protected override string ProjectName => Constants.ProjectName;
    protected override bool UseLazyInitialization => false; // we don't want lazy stuff in models, just getters and setters
    protected override bool AddBackingFieldsForCollectionProperties => false; // we just want static stuff - else you need to choose builders or models instead of entities
    protected override bool AddPrivateSetters => false; // we just want static stuff - else you need to choose builders or models instead of entities
    protected override ArgumentValidationType ValidateArgumentsInConstructor => ArgumentValidationType.Shared;
    protected override bool ConvertStringToStringBuilderOnBuilders => false; // we don't want string builders, just strings

    protected override bool IsMemberValid(ModelFramework.Objects.Contracts.IParentTypeContainer parent, ModelFramework.Objects.Contracts.ITypeBase typeBase)
        => parent != null
        && typeBase != null
        && (string.IsNullOrEmpty(parent.ParentTypeFullName)
            || parent.ParentTypeFullName.GetClassName() == $"I{typeBase.Name}"
            || (parent.ParentTypeFullName.StartsWith("ClassFramework.CodeGeneration.Models.Abstractions.") && typeBase.Namespace == "ClassFramework.Domain")
           );

    protected override IEnumerable<Type> GetPureAbstractModels()
    {
        yield return typeof(IType);
        yield return typeof(ICodeStatement);
    }
}
