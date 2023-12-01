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
