namespace DatabaseFramework.CodeGeneration.Models.TemplateFramework;

internal interface IDatabaseSchemaGeneratorSettings
{
    bool RecurseOnDeleteGeneratedFiles { get; }
    [Required(AllowEmptyStrings = true)] string LastGeneratedFilesFilename { get; }
    [Required] Encoding Encoding { get; }

    [Required(AllowEmptyStrings = true)] string Path { get; }
    [Required] CultureInfo CultureInfo { get; }
    bool GenerateMultipleFiles { get; }
    bool SkipWhenFileExists { get; }
    bool CreateCodeGenerationHeader { get; }
    [Required(AllowEmptyStrings = true)] string FilenameSuffix { get; }
}
