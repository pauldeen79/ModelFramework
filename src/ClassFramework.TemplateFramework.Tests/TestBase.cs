namespace ClassFramework.TemplateFramework.Tests;

public class TestBase
{
    protected IFixture Fixture { get; }

    protected TestBase()
    {
        Fixture = new Fixture().Customize(new AutoNSubstituteCustomization());

        Fixture.Register(() => CreateCsharpClassGeneratorSettings(true));
    }

    protected static CsharpClassGeneratorSettings CreateCsharpClassGeneratorSettings(bool generateMultipleFiles)
        => new CsharpClassGeneratorSettingsBuilder()
            .WithRecurseOnDeleteGeneratedFiles(false)
            .WithLastGeneratedFilesFilename(string.Empty)
            .WithEncoding(Encoding.UTF8)
            .WithGenerateMultipleFiles(generateMultipleFiles)
            //.WithSkipWhenFileExists(false) // default value
            .WithCreateCodeGenerationHeader(true)
            .WithEnableNullableContext(true)
            .WithCultureInfo(CultureInfo.InvariantCulture)
            .WithEnvironmentVersion("1.0.0")
            ///.WithPath(string.Empty) // default value
            .Build();
}

public abstract class TestBase<T> : TestBase
{
    protected T CreateSut() => Fixture.Create<T>();
}
