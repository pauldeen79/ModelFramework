namespace ClassFramework.Pipelines.Tests.Reflection.Features;

public class AddFieldsFeatureTests : TestBase<Pipelines.Reflection.Features.AddFieldsFeature>
{
    public class Process : AddFieldsFeatureTests
    {
        [Fact]
        public void Throws_On_Null_Context()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Process(context: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void Adds_Fields_When_Available()
        {
            // Arrange
            var sut = CreateSut();
            var sourceModel = typeof(MyFieldTestClass);
            var model = new ClassBuilder();
            var settings = CreateReflectionSettings(copyAttributes: true);
            var context = new PipelineContext<TypeBaseBuilder, ReflectionContext>(model, new ReflectionContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Fields.Should().HaveCount(2);
            model.Fields.Select(x => x.Visibility).Should().AllBeEquivalentTo(Visibility.Public);
            model.Fields.Select(x => x.ReadOnly).Should().BeEquivalentTo([false, true]);
            model.Fields.Select(x => x.Name).Should().BeEquivalentTo("myField", "myReadOnlyField");
            model.Fields.Select(x => x.TypeName).Should().BeEquivalentTo("System.Int32", "System.String");
            model.Fields.Select(x => x.IsNullable).Should().BeEquivalentTo([false, true]);
            model.Fields.Select(x => x.IsValueType).Should().BeEquivalentTo([true, false]);
            model.Fields[0].Attributes.Should().ContainSingle();
            model.Fields[model.Fields.Count - 1].Attributes.Should().BeEmpty();
        }
    }
}

#pragma warning disable CA1812 // Avoid uninstantiated internal classes
internal sealed class MyFieldTestClass
#pragma warning restore CA1812 // Avoid uninstantiated internal classes
{
    [Required]
    public int myField;
#pragma warning disable CA1823 // Avoid unused private fields
#pragma warning disable S1144 // Unused private types or members should be removed
#pragma warning disable IDE0051 // Remove unused private members
    public readonly string? myReadOnlyField;
#pragma warning restore IDE0051 // Remove unused private members
#pragma warning restore S1144 // Unused private types or members should be removed
#pragma warning restore CA1823 // Avoid unused private fields
}
