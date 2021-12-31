﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CrossCutting.Common.Extensions;
using FluentAssertions;
using ModelFramework.Objects.Builders;
using ModelFramework.Objects.Extensions;
using ModelFramework.Objects.Settings;
using TextTemplateTransformationFramework.Runtime;
using Xunit;

namespace ModelFramework.Generators.Objects.Tests.Extensions
{
    [ExcludeFromCodeCoverage]
    public class TypeBaseExtensionsTests
    {
        [Fact]
        public void Can_Generate_Interface_From_Class()
        {
            // Arrange
            var input = new ClassBuilder()
                .WithName("Test")
                .WithNamespace("MyNamespace")
                .AddMethods(new ClassMethodBuilder().WithName("MyMethod").WithTypeName("MyType").AddParameters(new ParameterBuilder().WithName("param1").WithTypeName("MyType")))
                .Build();

            // Act
            var actual = input.ToInterface(new InterfaceSettings());

            // Assert
            var generator = new CSharpClassGenerator();
            var src = TemplateRenderHelper.GetTemplateOutput(generator, new[] { actual });
            src.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
    public interface ITest
    {
        MyType MyMethod(MyType param1);
    }
}
");
        }

        [Fact]
        public void Can_Generate_Interface_From_Class_With_GenericTypeContraints()
        {
            // Arrange
            var input = new ClassBuilder()
                .WithName("Test")
                .WithNamespace("MyNamespace")
                .AddMethods(new ClassMethodBuilder().WithName("MyMethod")
                                                    .WithTypeName("MyType")
                                                    .AddParameters(new ParameterBuilder().WithName("param1").WithTypeName("MyType")))
                .Build();

            // Act
            var actual = input.ToInterface(new InterfaceSettings(applyGenericTypes: new Dictionary<string, string> { { "MyType", "T" } }));

            // Assert
            var generator = new CSharpClassGenerator();
            var src = TemplateRenderHelper.GetTemplateOutput(generator, new[] { actual });
            src.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
    public interface ITest<T>
    {
        T MyMethod(T param1);
    }
}
");
        }

        [Fact]
        public void Can_Generate_Interface_From_Class_With_Property()
        {
            // Arrange
            var input = new ClassBuilder()
                .WithName("Test")
                .WithNamespace("MyNamespace")
                .AddProperties
                (
                    new ClassPropertyBuilder()
                        .WithName("Test")
                        .WithType(typeof(string))
                        .AddGetterCodeStatements(new[] { "return _test;" }.ToLiteralCodeStatementBuilders())
                        .AddSetterCodeStatements(new[] { "_test = value;" }.ToLiteralCodeStatementBuilders())
                ).Build();

            // Act
            var actual = input.ToInterface(new InterfaceSettings());

            // Assert
            var generator = new CSharpClassGenerator();
            var src = TemplateRenderHelper.GetTemplateOutput(generator, new[] { actual });
            src.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
    public interface ITest
    {
        string Test
        {
            get;
            set;
        }
    }
}
");
        }

        [Fact]
        public void Can_Generate_Interface_From_Immutable_Class()
        {
            // Arrange
            var input = new ClassBuilder()
                .WithName("Test")
                .WithNamespace("MyNamespace")
                .AddProperties
                (
                    new ClassPropertyBuilder()
                        .WithName("Test")
                        .WithType(typeof(string))
                        .AddGetterCodeStatements(new[] { "return _test;" }.ToLiteralCodeStatementBuilders())
                        .AddSetterCodeStatements(new[] { "_test = value;" }.ToLiteralCodeStatementBuilders())
                ).Build()
                .ToImmutableClass(new ImmutableClassSettings(implementIEquatable: true));

            // Act
            var actual = input.ToInterface(new InterfaceSettings());

            // Assert
            var generator = new CSharpClassGenerator();
            var src = TemplateRenderHelper.GetTemplateOutput(generator, new[] { actual });
            src.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
    public interface ITest : IEquatable<Test>
    {
        string Test
        {
            get;
        }
    }
}
");
        }
    }
}
