namespace ClassFramework.TemplateFramework;

public partial record CsharpClassGeneratorSettings
{
    public CsharpClassGeneratorSettings ForSubclasses()
        => new(false, SkipWhenFileExists, false, null, null, null, false, IndentCount + 1, CultureInfo);
}
