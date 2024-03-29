﻿namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public class CsharpExpressionDumperClassBaseTests
{
    [Fact]
    public void Can_Generate_Code_To_CsharpClass()
    {
        // Act
        var actual = GenerateCode.For<Sut>(new CodeGenerationSettings("Sut", false, false, true)).TemplateFileManager.MultipleContentBuilder.Contents.First().Builder.ToString();

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
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
