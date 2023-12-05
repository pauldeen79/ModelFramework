namespace ClassFramework.CodeGeneration.Models.TemplateFramework;

internal interface ICsharpClassGeneratorSettings
{
    [Required(AllowEmptyStrings = true)] string Path { get; }
    [Required] CultureInfo CultureInfo { get; }
    bool GenerateMultipleFiles { get; }
    bool SkipWhenFileExists { get; }
    bool CreateCodeGenerationHeader { get; }
    string? EnvironmentVersion { get; }
    string? FilenameSuffix { get; }
    bool EnableNullableContext { get; }
}
