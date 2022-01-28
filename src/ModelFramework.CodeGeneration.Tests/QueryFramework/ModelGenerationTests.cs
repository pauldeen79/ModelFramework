using System.IO;
using System.Linq;
using CrossCutting.Common.Extensions;
using CsharpExpressionDumper.Abstractions;
using CsharpExpressionDumper.Core.Extensions;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using ModelFramework.Objects.Extensions;
using ModelFramework.Objects.Settings;
using ModelFramework.CodeGeneration.Tests.QueryFramework.CodeGenerationProviders;
using TextTemplateTransformationFramework.Runtime.CodeGeneration;
using Xunit;
using ModelFramework.CodeGeneration.ObjectHandlerPropertyFilters;
using ModelFramework.Objects.Contracts;

namespace ModelFramework.CodeGeneration.Tests.QueryFramework
{
    public class ModelGenerationTests
    {
        private static readonly CodeGenerationSettings Settings = new CodeGenerationSettings
        (
            //basePath: @"C:\Temp\QueryFramework",
            //basePath: Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\"),
            basePath: @"C:\Git\QueryFramework\src",
            generateMultipleFiles: false,
            dryRun: true
        );

        // Bootstrap test that generates c# code for the model used in code generation :)
        [Fact]
        public void Can_Generate_Model_For_Abstractions()
        {
            // Arrange
            var models = new[]
            {
                typeof(IAttribute),
                typeof(IAttributeParameter),
                typeof(IClass),
                typeof(IClassConstructor),
                typeof(IClassField),
                typeof(IClassMethod),
                typeof(IClassProperty),
                typeof(IEnum),
                typeof(IEnumMember),
                typeof(IInterface),
                typeof(IParameter)
            }.Select(x => x.ToClassBuilder(new ClassSettings()).Build()).Select(x => x.ToInterfaceBuilder()).ToArray();
            var serviceCollection = new ServiceCollection();
            var serviceProvider = serviceCollection
                .AddCsharpExpressionDumper()
                .AddSingleton<IObjectHandlerPropertyFilter, SkipDefaultValuesForModelFramework>()
                .BuildServiceProvider();
            var dumper = serviceProvider.GetRequiredService<ICsharpExpressionDumper>();

            // Act
            var code = dumper.Dump(models);

            // Assert
            code.Should().NotBeEmpty();
        }

        [Fact]
        public void Can_Generate_Records_From_Model()
        {
            Verify(GenerateCode.For<AbstractionsInterfaces>(Settings));
            Verify(GenerateCode.For<CoreRecords>(Settings));
            Verify(GenerateCode.For<CoreBuilders>(Settings));
        }

        private void Verify(GenerateCode generatedCode)
        {
            if (Settings.DryRun)
            {
                var actual = generatedCode.GenerationEnvironment.ToString();

                // Assert
                actual.NormalizeLineEndings().Should().NotBeNullOrEmpty().And.NotStartWith("Error:");
            }
        }
    }
}
