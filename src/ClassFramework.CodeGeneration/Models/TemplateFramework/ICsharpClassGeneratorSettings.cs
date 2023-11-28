namespace ClassFramework.CodeGeneration.Models.TemplateFramework;

internal interface ICsharpClassGeneratorSettings
{
    bool GenerateMultipleFiles { get; }
    bool SkipWhenFileExists { get; }
    bool CreateCodeGenerationHeader { get; }
    string? EnvironmentVersion { get; }
    string? FilenamePrefix { get; }
    string? FilenameSuffix { get; }
    bool EnableNullableContext { get; }
    int IndentCount { get; }
    [Required] CultureInfo CultureInfo { get; }
}
