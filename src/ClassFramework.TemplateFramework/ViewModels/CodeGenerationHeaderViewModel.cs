namespace ClassFramework.TemplateFramework.ViewModels;

public class CodeGenerationHeaderViewModel : CsharpClassGeneratorViewModelBase
{
    public string Version
        => !string.IsNullOrEmpty(Settings.EnvironmentVersion)
            ? Settings.EnvironmentVersion
            : Environment.Version.ToString();
}

public class CodeGenerationHeaderViewModelFactoryComponent : IViewModelFactoryComponent
{
    public object Create()
        => new CodeGenerationHeaderViewModel();

    public bool Supports(object model)
        => model is CodeGenerationHeaderModel;
}
