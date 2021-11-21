using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using ModelFramework.Common.Default;
using ModelFramework.Objects;
using ModelFramework.Objects.Default;
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
            var input = new Class("Test", "MyNamespace", methods: new[] { new ClassMethod("MyMethod", "MyType", parameters: new[] { new Parameter("param1", "MyType") }) });

            // Act
            var actual = input.ToInterface(new InterfaceSettings());

            // Assert
            var generator = new CSharpClassGenerator();
            var src = TemplateRenderHelper.GetTemplateOutput(generator, new[] { actual });
            src.Should().Be(@"using System;
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
        public void Can_Generate_Interface_From_Class_With_Possibility_To_Skip_Method()
        {
            // Arrange
            var input = new Class("Test", "MyNamespace", methods: new[]
            {
                new ClassMethod("MyMethod", "MyType", parameters: new[] { new Parameter("param1", "MyType") }),
                new ClassMethod("MySkippedMethod", "MyType", parameters: new[] { new Parameter("param1", "MyType") }, metadata: new[] { new Metadata(MetadataNames.SkipMethodOnAutoGenerateInterface, "") }),
            });

            // Act
            var actual = input.ToInterface(new InterfaceSettings());

            // Assert
            var generator = new CSharpClassGenerator();
            var src = TemplateRenderHelper.GetTemplateOutput(generator, new[] { actual });
            src.Should().Be(@"using System;
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
            var input = new Class("Test", "MyNamespace", methods: new[] { new ClassMethod("MyMethod", "MyType", parameters: new[] { new Parameter("param1", "MyType") }) });

            // Act
            var actual = input.ToInterface(new InterfaceSettings(applyGenericTypes: new Dictionary<string, string> { { "MyType", "T" } }));

            // Assert
            var generator = new CSharpClassGenerator();
            var src = TemplateRenderHelper.GetTemplateOutput(generator, new[] { actual });
            src.Should().Be(@"using System;
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
            var input = new Class
            (
                "Test",
                "MyNamespace",
                properties: new[]
                {
                    new ClassProperty
                    (
                        "Test",
                        typeof(string).FullName,
                        getterCodeStatements: new[] { "return _test;" }.ToLiteralCodeStatements(),
                        setterCodeStatements: new[] { "_test = value;" }.ToLiteralCodeStatements()
                    )
                }
            );

            // Act
            var actual = input.ToInterface(new InterfaceSettings());

            // Assert
            var generator = new CSharpClassGenerator();
            var src = TemplateRenderHelper.GetTemplateOutput(generator, new[] { actual });
            src.Should().Be(@"using System;
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
            var input = new Class
            (
                "Test",
                "MyNamespace",
                properties: new[]
                {
                    new ClassProperty
                    (
                        "Test",
                        typeof(string).FullName,
                        getterCodeStatements: new[] { "return _test;" }.ToLiteralCodeStatements(),
                        setterCodeStatements: new[] { "_test = value;" }.ToLiteralCodeStatements()
                    )
                }
            ).ToImmutableClass(new ImmutableClassSettings(implementIEquatable: true));

            // Act
            var actual = input.ToInterface(new InterfaceSettings());

            // Assert
            var generator = new CSharpClassGenerator();
            var src = TemplateRenderHelper.GetTemplateOutput(generator, new[] { actual });
            src.Should().Be(@"using System;
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
