using System.IO;
using System.Linq;
using CrossCutting.Common.Extensions;
using CsharpExpressionDumper.Abstractions;
using CsharpExpressionDumper.Core.Extensions;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using ModelFramework.Objects.Extensions;
using ModelFramework.Objects.Settings;
//using QueryFramework.Abstractions;
using ModelFramework.CodeGeneration.Tests.QueryFramework.CodeGenerationProviders;
//using QueryFramework.CodeGeneration.ObjectHandlerPropertyFilters;
using TextTemplateTransformationFramework.Runtime.CodeGeneration;
using Xunit;

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
        //[Fact]
        //public void Can_Generate_Model_For_Abstractions()
        //{
        //    // Arrange
        //    var models = new[]
        //    {
        //        typeof(IQueryCondition),
        //        typeof(IQueryExpression),
        //        typeof(IQueryExpressionFunction),
        //        typeof(IQueryParameter),
        //        typeof(IQueryParameterValue),
        //        typeof(IQuerySortOrder)
        //    }.Select(x => x.ToClassBuilder(new ClassSettings())).ToArray();
        //    var serviceCollection = new ServiceCollection();
        //    var serviceProvider = serviceCollection
        //        .AddCsharpExpressionDumper()
        //        .AddSingleton<IObjectHandlerPropertyFilter, SkipDefaultValuesForModelFramework>()
        //        .BuildServiceProvider();
        //    var dumper = serviceProvider.GetRequiredService<ICsharpExpressionDumper>();

        //    // Act
        //    var code = dumper.Dump(models);

        //    // Assert
        //    code.Should().NotBeEmpty();
        //}

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
