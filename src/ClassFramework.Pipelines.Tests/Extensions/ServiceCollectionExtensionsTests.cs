namespace ClassFramework.Pipelines.Tests.Extensions;

public class ServiceCollectionExtensionsTests : TestBase
{
    public class AddPipelines : ServiceCollectionExtensionsTests
    {
        [Fact]
        public void Registers_All_Required_Dependencies()
        {
            // Arrange
            var serviceCollection = new ServiceCollection()
                .AddScoped(_ => Fixture.Freeze<IFormattableStringParser>()) // note that normally, you would probably use AddParsers from the CrossCutting.Utilities.Parsers package...
                .AddCsharpExpressionCreator()
                .AddPipelines();

            // Act & Assert
            serviceCollection.Invoking(x =>
            {
                using var provder = x.BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });
            }).Should().NotThrow();
        }

        [Fact]
        public void Can_Resolve_BuilderPipelineBuilder()
        {
            // Arrange
            var serviceCollection = new ServiceCollection()
                .AddScoped(_ => Fixture.Freeze<IFormattableStringParser>()) // note that normally, you would probably use AddParsers from the CrossCutting.Utilities.Parsers package...
                .AddPipelines();
            using var provider = serviceCollection.BuildServiceProvider();
            using var scope = provider.CreateScope();

            // Act
            var builder = scope.ServiceProvider.GetRequiredService<IPipelineBuilder<ClassBuilder, BuilderContext>>();

            // Assert
            builder.Should().BeOfType<Pipelines.Builder.PipelineBuilder>();
        }

        [Fact]
        public void Can_Resolve_BuilderPipeline()
        {
            // Arrange
            var serviceCollection = new ServiceCollection()
                .AddScoped(_ => Fixture.Freeze<IFormattableStringParser>()) // note that normally, you would probably use AddParsers from the CrossCutting.Utilities.Parsers package...
                .AddPipelines();
            using var provider = serviceCollection.BuildServiceProvider();
            using var scope = provider.CreateScope();

            // Act
            var builder = scope.ServiceProvider.GetRequiredService<IPipeline<ClassBuilder, BuilderContext>>();

            // Assert
            builder.Should().BeOfType<Pipeline<ClassBuilder, BuilderContext>>();
        }

        [Fact]
        public void Can_Resolve_EntityPipelineBuilder()
        {
            // Arrange
            var serviceCollection = new ServiceCollection()
                .AddScoped(_ => Fixture.Freeze<IFormattableStringParser>()) // note that normally, you would probably use AddParsers from the CrossCutting.Utilities.Parsers package...
                .AddPipelines();
            using var provider = serviceCollection.BuildServiceProvider();
            using var scope = provider.CreateScope();

            // Act
            var builder = scope.ServiceProvider.GetRequiredService<IPipelineBuilder<ClassBuilder, EntityContext>>();

            // Assert
            builder.Should().BeOfType<Pipelines.Entity.PipelineBuilder>();
        }

        [Fact]
        public void Can_Resolve_EntityPipeline()
        {
            // Arrange
            var serviceCollection = new ServiceCollection()
                .AddScoped(_ => Fixture.Freeze<IFormattableStringParser>()) // note that normally, you would probably use AddParsers from the CrossCutting.Utilities.Parsers package...
                .AddPipelines();
            using var provider = serviceCollection.BuildServiceProvider();
            using var scope = provider.CreateScope();

            // Act
            var builder = scope.ServiceProvider.GetRequiredService<IPipeline<ClassBuilder, EntityContext>>();

            // Assert
            builder.Should().BeOfType<Pipeline<ClassBuilder, EntityContext>>();
        }
    }
}
