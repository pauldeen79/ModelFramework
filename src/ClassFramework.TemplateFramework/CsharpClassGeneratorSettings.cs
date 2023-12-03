namespace ClassFramework.TemplateFramework;

public partial record CsharpClassGeneratorSettings
{
    public CsharpClassGeneratorSettings ForSubclasses()
        => new(Path, CultureInfo, GenerateMultipleFiles, SkipWhenFileExists, CreateCodeGenerationHeader, EnvironmentVersion, FilenameSuffix, EnableNullableContext, IndentCount + 1);
    
    public string? FilenamePrefix
        => string.IsNullOrEmpty(Path)
            ? string.Empty
            : Path + "/";
}
