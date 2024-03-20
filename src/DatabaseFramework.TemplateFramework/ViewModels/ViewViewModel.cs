namespace DatabaseFramework.TemplateFramework.ViewModels;

public class ViewViewModel : DatabaseSchemaGeneratorViewModelBase<View>, INameContainer
{
    public string Schema
        => GetModel().Schema.FormatAsDatabaseIdentifier();

    public string Name
        => GetModel().Name.FormatAsDatabaseIdentifier();

    public string Definition
        => GetModel().Definition;

    public CodeGenerationHeaderModel CodeGenerationHeaders
        => new CodeGenerationHeaderModel(GetModel(), Settings.CreateCodeGenerationHeader);
}
