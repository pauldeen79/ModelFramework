namespace ClassFramework.TemplateFramework.ViewModels;

public class CodeGenerationHeaderViewModel : CsharpClassGeneratorViewModel
{
    public CodeGenerationHeaderViewModel(CsharpClassGeneratorSettings settings) : base(settings)
    {
    }

    public bool CreateCodeGenerationHeader
        => Settings.CreateCodeGenerationHeader;

    public string Version
        => !string.IsNullOrEmpty(Settings.EnvironmentVersion)
            ? Settings.EnvironmentVersion
            : Environment.Version.ToString();
}
