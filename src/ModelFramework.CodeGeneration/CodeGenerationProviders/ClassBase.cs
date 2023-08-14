namespace ModelFramework.CodeGeneration.CodeGenerationProviders;

public abstract class ClassBase : ICodeGenerationProvider
{
    public abstract string DefaultFileName { get; }
    public abstract bool RecurseOnDeleteGeneratedFiles { get; }

    public abstract object CreateModel();

    public virtual Encoding Encoding => Encoding.UTF8;
    public virtual string LastGeneratedFilesFilename => $"*{FileNameSuffix}.cs";
    public virtual string Path => string.Empty;
    public virtual Action? AdditionalActionDelegate => null;
    public virtual bool GenerateMultipleFiles => true;
    public virtual bool SkipWhenFileExists => false;

    protected abstract bool EnableNullableContext { get; }
    protected abstract bool CreateCodeGenerationHeader { get; }
    protected virtual string FileNameSuffix => ".template.generated";
    protected virtual bool UseCustomInitializersOnAttributeBuilder => false;

    protected virtual AttributeBuilder? AttributeInitializeDelegate(Attribute sourceAttribute) => AttributeBuilder.DefaultInitializer(sourceAttribute);

    public object CreateAdditionalParameters()
        => new Dictionary<string, object>
        {
            { nameof(CSharpClassGenerator.EnableNullableContext), EnableNullableContext },
            { nameof(CSharpClassGenerator.CreateCodeGenerationHeader), CreateCodeGenerationHeader },
            { nameof(CSharpClassGenerator.GenerateMultipleFiles), GenerateMultipleFiles },
            { nameof(CSharpClassGenerator.SkipWhenFileExists), SkipWhenFileExists },
            { nameof(CSharpClassGenerator.FileNamePrefix), FileNamePrefix },
            { nameof(CSharpClassGenerator.FileNameSuffix), FileNameSuffix }
        };

    public object CreateGenerator()
        => new TemplateProxy(new CSharpClassGenerator());

    private string FileNamePrefix => string.IsNullOrEmpty(Path)
        ? string.Empty
        : Path + "/";
}
