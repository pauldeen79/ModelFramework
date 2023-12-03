namespace ClassFramework.TemplateFramework.ViewModels;

public class CodeGenerationHeaderViewModel : CsharpClassGeneratorViewModelBase
{
    public CodeGenerationHeaderViewModel(CsharpClassGeneratorSettings settings)
        : base(settings)
    {
    }

    public string Version
        => !string.IsNullOrEmpty(Settings.EnvironmentVersion)
            ? Settings.EnvironmentVersion
            : Environment.Version.ToString();
}

public class CodeGenerationHeaderViewModelCreator : IViewModelCreator
{
    public object Create(object model, CsharpClassGeneratorSettings settings)
        => new CodeGenerationHeaderViewModel(settings);

    public bool Supports(object model)
        => model is CodeGenerationHeaderModel;
}
