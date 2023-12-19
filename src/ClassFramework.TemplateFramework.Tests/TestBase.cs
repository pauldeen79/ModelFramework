namespace ClassFramework.TemplateFramework.Tests;

public class TestBase
{
    protected IFixture Fixture { get; } = new Fixture().Customize(new AutoNSubstituteCustomization());
}
