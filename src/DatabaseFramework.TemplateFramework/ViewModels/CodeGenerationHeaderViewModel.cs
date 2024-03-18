namespace DatabaseFramework.TemplateFramework.ViewModels;

public class CodeGenerationHeaderViewModel : DatabaseSchemaGeneratorViewModelBase<CodeGenerationHeaderModel>
{
    public bool CreateCodeGenerationHeader
        => GetModel().CreateCodeGenerationHeader;

    public string Name
        => GetModel().Name.FormatAsDatabaseIdentifier();

    public string? Schema
        => GetModel().Schema?.FormatAsDatabaseIdentifier();

    public string ObjectType
        => GetModel().ObjectType;
}
