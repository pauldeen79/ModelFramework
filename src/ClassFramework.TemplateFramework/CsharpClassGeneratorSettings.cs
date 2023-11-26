namespace ClassFramework.TemplateFramework;

public sealed record CsharpClassGeneratorSettings
{
#pragma warning disable S107 // Methods should not have too many parameters
    public CsharpClassGeneratorSettings(bool generateMultipleFiles,
                                        bool skipWhenFileExists,
                                        bool createCodeGenerationHeader,
                                        string? environmentVersion,
                                        string? filenamePrefix,
                                        string? filenameSuffix,
                                        bool enableNullableContext,
                                        int indentCount,
                                        CultureInfo cultureInfo)
#pragma warning restore S107 // Methods should not have too many parameters
    {
        GenerateMultipleFiles = generateMultipleFiles;
        SkipWhenFileExists = skipWhenFileExists;
        CreateCodeGenerationHeader = createCodeGenerationHeader;
        EnvironmentVersion = environmentVersion;
        FilenamePrefix = filenamePrefix;
        FilenameSuffix = filenameSuffix;
        EnableNullableContext = enableNullableContext;
        IndentCount = indentCount;
        CultureInfo = cultureInfo;
    }

    public bool GenerateMultipleFiles { get; }
    public bool SkipWhenFileExists { get; }
    public bool CreateCodeGenerationHeader { get; }
    public string? EnvironmentVersion { get; }
    public string? FilenamePrefix { get; }
    public string? FilenameSuffix { get; }
    public bool EnableNullableContext { get; }
    public int IndentCount { get; }
    public CultureInfo CultureInfo { get; }

    public CsharpClassGeneratorSettings ForSubclasses()
        => new(false, SkipWhenFileExists, false, null, null, null, false, IndentCount + 1, CultureInfo);
}
