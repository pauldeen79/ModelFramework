namespace DatabaseFramework.TemplateFramework.ViewModels;

public class CodeGenerationHeaderViewModel : DatabaseSchemaGeneratorViewModelBase<CodeGenerationHeaderModel>
{
    public bool CreateCodeGenerationHeader
        => GetModel().CreateCodeGenerationHeader;

    public string Name
        => GetModel().Name;

    public string Schema
        => GetModel().Schema;

    public string ObjectType
        => GetModel().ObjectType;
}
