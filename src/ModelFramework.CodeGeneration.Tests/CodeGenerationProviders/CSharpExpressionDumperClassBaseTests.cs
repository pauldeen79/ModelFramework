namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public class CsharpExpressionDumperClassBaseTests
{
    [Fact]
    public void Can_Generate_Code_To_CsharpClass()
    {
        // Arrange
        var templateFactoryMock = new Mock<ITemplateFactory>();
        templateFactoryMock.Setup(x => x.Create(It.IsAny<Type>())).Returns<Type>(t => Activator.CreateInstance(t)!);
        using var provider = new ServiceCollection()
            .AddTemplateFrameworkCodeGeneration()
            .AddTemplateFrameworkRuntime()
            .AddTemplateFramework()
            .AddSingleton(new Mock<ITemplateProviderPluginFactory>().Object)
            .AddSingleton(templateFactoryMock.Object)
            .BuildServiceProvider();
        var codeGenerationEngine = provider.GetRequiredService<ICodeGenerationEngine>();
        var templateProvider = provider.GetRequiredService<ITemplateProvider>();
        var generationEnvironment = new MultipleContentBuilderEnvironment();

        // Act
        codeGenerationEngine.Generate(new Sut(), templateProvider, generationEnvironment, new CodeGenerationSettings("UnitTest", string.Empty, dryRun: true));
        var actual = generationEnvironment.Builder.Build().Contents.First().Contents;

        // Assert
        CrossCutting.Common.Extensions.StringExtensions.NormalizeLineEndings(actual).Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
    public partial class MyCSharpClassBase
    {
        protected static ModelFramework.Objects.Contracts.ITypeBase[] GetModels()
        {
            return new[]
            {
                new ModelFramework.Objects.Builders.InterfaceBuilder()
                    .WithNamespace(@""ModelFramework.Common.Contracts"")
                    .AddInterfaces(
                        @""ModelFramework.Common.Contracts.INameContainer"")
                    .AddProperties(
                        new ModelFramework.Objects.Builders.ClassPropertyBuilder()
                            .WithHasSetter(false)
                            .WithName(@""Value"")
                            .WithTypeName(@""System.Object"")
                            .WithIsNullable(true)
                            .WithParentTypeFullName(@""ModelFramework.Common.Contracts.IMetadata""),
                        new ModelFramework.Objects.Builders.ClassPropertyBuilder()
                            .WithHasSetter(false)
                            .WithName(@""Name"")
                            .WithTypeName(@""System.String"")
                            .WithParentTypeFullName(@""ModelFramework.Common.Contracts.INameContainer""))
                    .WithName(@""IMetadata""),
            }.Select(x => x.Build()).ToArray();
        }
    }
}
");
    }

    [Fact]
    public void Can_Generate_Code_To_TTInclude_File()
    {
        // Arrange
        var sut = new Sut();

        // Act
        var code = sut.CreateCode();

        // Assert
        code.Should().Be(@"new[]
{
    new ModelFramework.Objects.Builders.InterfaceBuilder()
        .WithNamespace(@""ModelFramework.Common.Contracts"")
        .AddInterfaces(
            @""ModelFramework.Common.Contracts.INameContainer"")
        .AddProperties(
            new ModelFramework.Objects.Builders.ClassPropertyBuilder()
                .WithHasSetter(false)
                .WithName(@""Value"")
                .WithTypeName(@""System.Object"")
                .WithIsNullable(true)
                .WithParentTypeFullName(@""ModelFramework.Common.Contracts.IMetadata""),
            new ModelFramework.Objects.Builders.ClassPropertyBuilder()
                .WithHasSetter(false)
                .WithName(@""Name"")
                .WithTypeName(@""System.String"")
                .WithParentTypeFullName(@""ModelFramework.Common.Contracts.INameContainer""))
        .WithName(@""IMetadata""),
}");
    }

    private sealed class Sut : CSharpExpressionDumperClassBase
    {
        public override string Path => "Sut";
        public override string DefaultFileName => "MyCSharpClassBase.generated.cs";
        public override bool RecurseOnDeleteGeneratedFiles => false;

        protected override string[] NamespacesToAbbreviate => Array.Empty<string>();
        protected override Type[] Models => new[] { typeof(IMetadata) };
        protected override string Namespace => "MyNamespace";
        protected override string ClassName => "MyCSharpClassBase";
        protected override bool CreateCodeGenerationHeader => false;
    }
}
