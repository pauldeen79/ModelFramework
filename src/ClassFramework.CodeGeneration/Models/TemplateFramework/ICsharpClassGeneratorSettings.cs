﻿namespace ClassFramework.CodeGeneration.Models.TemplateFramework;

internal interface ICsharpClassGeneratorSettings
{
    bool RecurseOnDeleteGeneratedFiles { get; }
    [Required(AllowEmptyStrings = true)] string LastGeneratedFilesFilename { get; }
    [Required] Encoding Encoding { get; }

    [Required(AllowEmptyStrings = true)] string Path { get; }
    [Required] CultureInfo CultureInfo { get; }
    bool GenerateMultipleFiles { get; }
    bool SkipWhenFileExists { get; }
    bool CreateCodeGenerationHeader { get; }
    string? EnvironmentVersion { get; }
    string? FilenameSuffix { get; }
    bool EnableNullableContext { get; }
    bool EnableGlobalUsings { get; }
}
