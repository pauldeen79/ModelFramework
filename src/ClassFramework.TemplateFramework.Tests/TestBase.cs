﻿namespace ClassFramework.TemplateFramework.Tests;

public class TestBase
{
    protected IFixture Fixture { get; }

    protected TestBase()
    {
        Fixture = new Fixture().Customize(new AutoNSubstituteCustomization());

        Fixture.Register(() => CreateCsharpClassGeneratorSettings(true));
    }

    protected static CsharpClassGeneratorSettings CreateCsharpClassGeneratorSettings(bool generateMultipleFiles = true, bool enableNullableContext = true)
        => new CsharpClassGeneratorSettingsBuilder()
            .WithRecurseOnDeleteGeneratedFiles(false)
            .WithLastGeneratedFilesFilename(string.Empty)
            .WithEncoding(Encoding.UTF8)
            .WithGenerateMultipleFiles(generateMultipleFiles)
            //.WithSkipWhenFileExists(false) // default value
            .WithCreateCodeGenerationHeader(true)
            .WithEnableNullableContext(enableNullableContext)
            .WithCultureInfo(CultureInfo.InvariantCulture)
            .WithEnvironmentVersion("1.0.0")
            ///.WithPath(string.Empty) // default value
            .Build();

    protected ITemplateContext CreateTemplateContext()
        => new TemplateContext(Fixture.Freeze<ITemplateEngine>(), Fixture.Freeze<ITemplateComponentRegistry>(), "default.cs", Fixture.Freeze<ITemplateIdentifier>(), new object());
}

public abstract class TestBase<T> : TestBase
{
    protected T CreateSut() => Fixture.Create<T>();
}
