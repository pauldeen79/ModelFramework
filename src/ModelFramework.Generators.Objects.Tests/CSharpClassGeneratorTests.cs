namespace ModelFramework.Generators.Objects.Tests;

public class CSharpClassGeneratorTests
{
    #region expected results
    private const string ImmutableClassCode = @"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
    public class MyRecord
    {
        public string Property1
        {
            get;
        }

        public bool Property2
        {
            get;
        }

        public MyRecord With(string property1 = default(string), bool property2 = default(bool))
        {
            return new MyRecord
            (
                property1 == default(string) ? this.Property1 : property1,
                property2 == default(bool) ? this.Property2 : property2
            );
        }

        public MyRecord(string property1, bool property2)
        {
            this.Property1 = property1;
            this.Property2 = property2;
        }
    }
}
";
    private const string ImmutableClassCodeNoWithMethod = @"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
    public class MyRecord
    {
        public string Property1
        {
            get;
        }

        public bool Property2
        {
            get;
        }

        public MyRecord(string property1, bool property2)
        {
            this.Property1 = property1;
            this.Property2 = property2;
        }
    }
}
";
    private const string PocoClassCode = @"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
    public class MyPoco
    {
        public string Property1
        {
            get;
            set;
        }

        public bool Property2
        {
            get;
            set;
        }
    }
}
";
    private const string ImmutableClassCodeWithIEnumerable = @"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
    public class MyRecord
    {
        public string Property1
        {
            get;
        }

        public bool Property2
        {
            get;
        }

        public System.Collections.Immutable.IImmutableList<string> Property3
        {
            get;
        }

        public MyRecord With(string property1 = default(string), bool property2 = default(bool), System.Collections.Immutable.IImmutableList<string> property3 = default(System.Collections.Immutable.IImmutableList<string>))
        {
            return new MyRecord
            (
                property1 == default(string) ? this.Property1 : property1,
                property2 == default(bool) ? this.Property2 : property2,
                property3 == default(System.Collections.Immutable.IImmutableList<string>) ? this.Property3 : property3
            );
        }

        public MyRecord(string property1, bool property2, System.Collections.Generic.IEnumerable<string> property3)
        {
            this.Property1 = property1;
            this.Property2 = property2;
            this.Property3 = property3;
        }
    }
}
";
    private const string ImmutableClassCodeWithCollection = @"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
    public class MyRecord
    {
        public string Property1
        {
            get;
        }

        public bool Property2
        {
            get;
        }

        public System.Collections.Immutable.IImmutableList<string> Property3
        {
            get;
        }

        public MyRecord With(string property1 = default(string), bool property2 = default(bool), System.Collections.Immutable.IImmutableList<string> property3 = default(System.Collections.Immutable.IImmutableList<string>))
        {
            return new MyRecord
            (
                property1 == default(string) ? this.Property1 : property1,
                property2 == default(bool) ? this.Property2 : property2,
                property3 == default(System.Collections.Immutable.IImmutableList<string>) ? this.Property3 : property3
            );
        }

        public MyRecord(string property1, bool property2, System.Collections.Generic.IEnumerable<string> property3)
        {
            this.Property1 = property1;
            this.Property2 = property2;
            this.Property3 = property3;
        }
    }
}
";
    private const string ImmutableBuilderClassCodeWithNullChecks = @"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
    public class MyRecord
    {
        public string Property1
        {
            get;
        }

        public System.Collections.Generic.IReadOnlyCollection<string> Property2
        {
            get;
        }

        public MyCustomType Property3
        {
            get;
        }

        public System.Collections.Generic.IReadOnlyCollection<MyCustomType> Property4
        {
            get;
        }

        public MyRecord(string property1, System.Collections.Generic.IEnumerable<string> property2, MyCustomType property3, System.Collections.Generic.IEnumerable<MyCustomType> property4)
        {
            this.Property1 = property1;
            this.Property2 = new System.Collections.Generic.List<System.String>(property2 ?? Enumerable.Empty<System.String>());
            this.Property3 = property3;
            this.Property4 = new System.Collections.Generic.List<MyCustomType>(property4 ?? Enumerable.Empty<MyCustomType>());
        }
    }

    public class MyRecordBuilder
    {
        public string Property1
        {
            get;
            set;
        }

        public System.Collections.Generic.List<string> Property2
        {
            get;
            set;
        }

        public MyCustomTypeBuilder Property3
        {
            get;
            set;
        }

        public System.Collections.Generic.List<MyCustomTypeBuilder> Property4
        {
            get;
            set;
        }

        public MyNamespace.MyRecord Build()
        {
            return new MyNamespace.MyRecord(Property1, Property2, Property3?.Build(), Property4.Select(x => x.Build()));
        }

        public MyRecordBuilder WithProperty1(string property1)
        {
            Property1 = property1;
            return this;
        }

        public MyRecordBuilder AddProperty2(System.Collections.Generic.IEnumerable<string> property2)
        {
            return AddProperty2(property2.ToArray());
        }

        public MyRecordBuilder AddProperty2(params string[] property2)
        {
            if (property2 != null)
            {
                Property2.AddRange(property2);
            }
            return this;
        }

        public MyRecordBuilder WithProperty3(MyCustomTypeBuilder property3)
        {
            Property3 = property3;
            return this;
        }

        public MyRecordBuilder AddProperty4(System.Collections.Generic.IEnumerable<MyCustomTypeBuilder> property4)
        {
            return AddProperty4(property4.ToArray());
        }

        public MyRecordBuilder AddProperty4(params MyCustomTypeBuilder[] property4)
        {
            if (property4 != null)
            {
                Property4.AddRange(property4);
            }
            return this;
        }

        public MyRecordBuilder()
        {
            Property2 = new System.Collections.Generic.List<string>();
            Property4 = new System.Collections.Generic.List<MyCustomTypeBuilder>();
            Property1 = string.Empty;
            Property3 = default;
        }

        public MyRecordBuilder(MyNamespace.MyRecord source)
        {
            if (source == null)
            {
                throw new System.ArgumentNullException(""source"");
            }
            Property2 = new System.Collections.Generic.List<string>();
            Property4 = new System.Collections.Generic.List<MyCustomTypeBuilder>();
            Property1 = source.Property1;
            if (source.Property2 != null) Property2.AddRange(source.Property2);
            Property3 = new MyCustomTypeBuilder(source.Property3);
            if (source.Property4 != null) Property4.AddRange(source.Property4.Select(x => new MyCustomTypeBuilder(x)));
        }
    }
}
";
    private const string ImmutableBuilderClassCodeWithoutNullChecks = @"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
    public class MyRecord
    {
        public string Property1
        {
            get;
        }

        public System.Collections.Generic.IReadOnlyCollection<string> Property2
        {
            get;
        }

        public MyCustomType Property3
        {
            get;
        }

        public System.Collections.Generic.IReadOnlyCollection<MyCustomType> Property4
        {
            get;
        }

        public MyRecord(string property1, System.Collections.Generic.IEnumerable<string> property2, MyCustomType property3, System.Collections.Generic.IEnumerable<MyCustomType> property4)
        {
            this.Property1 = property1;
            this.Property2 = new System.Collections.Generic.List<System.String>(property2 ?? Enumerable.Empty<System.String>());
            this.Property3 = property3;
            this.Property4 = new System.Collections.Generic.List<MyCustomType>(property4 ?? Enumerable.Empty<MyCustomType>());
        }
    }

    public class MyRecordBuilder
    {
        public string Property1
        {
            get;
            set;
        }

        public System.Collections.Generic.List<string> Property2
        {
            get;
            set;
        }

        public MyCustomTypeBuilder Property3
        {
            get;
            set;
        }

        public System.Collections.Generic.List<MyCustomTypeBuilder> Property4
        {
            get;
            set;
        }

        public MyNamespace.MyRecord Build()
        {
            return new MyNamespace.MyRecord(Property1, Property2, Property3?.Build(), Property4.Select(x => x.Build()));
        }

        public MyRecordBuilder WithProperty1(string property1)
        {
            Property1 = property1;
            return this;
        }

        public MyRecordBuilder AddProperty2(System.Collections.Generic.IEnumerable<string> property2)
        {
            return AddProperty2(property2.ToArray());
        }

        public MyRecordBuilder AddProperty2(params string[] property2)
        {
            Property2.AddRange(property2);
            return this;
        }

        public MyRecordBuilder WithProperty3(MyCustomTypeBuilder property3)
        {
            Property3 = property3;
            return this;
        }

        public MyRecordBuilder AddProperty4(System.Collections.Generic.IEnumerable<MyCustomTypeBuilder> property4)
        {
            return AddProperty4(property4.ToArray());
        }

        public MyRecordBuilder AddProperty4(params MyCustomTypeBuilder[] property4)
        {
            Property4.AddRange(property4);
            return this;
        }

        public MyRecordBuilder()
        {
            Property2 = new System.Collections.Generic.List<string>();
            Property4 = new System.Collections.Generic.List<MyCustomTypeBuilder>();
            Property1 = string.Empty;
            Property3 = default;
        }

        public MyRecordBuilder(MyNamespace.MyRecord source)
        {
            Property2 = new System.Collections.Generic.List<string>();
            Property4 = new System.Collections.Generic.List<MyCustomTypeBuilder>();
            Property1 = source.Property1;
            Property2.AddRange(source.Property2);
            Property3 = new MyCustomTypeBuilder(source.Property3);
            Property4.AddRange(source.Property4.Select(x => new MyCustomTypeBuilder(x)));
        }
    }
}
";
    #endregion

    [Fact]
    public void IntegrationTest()
    {
        // Arrange
        var model = new[]
        {
            new ClassBuilder()
                .WithName("MyClass")
                .WithNamespace("MyNamespace")
                .AddFields(GetFields())
                .AddProperties(GetProperties())
                .AddMethods(GetMethods())
                .AddConstructors(GetConstructors())
                .AddEnums(GetEnums())
                .AddSubClasses(GetSubClasses())
                .AddAttributes
                (
                    new AttributeBuilder()
                        .WithName("MyAttribute")
                        .AddParameters
                        (
                            new AttributeParameterBuilder().WithValue(1).WithName("Name1"),
                            new AttributeParameterBuilder().WithValue(2).WithName("Name2")
                        )
                ).Build()
        };
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
    [MyAttribute(Name1 = 1, Name2 = 2)]
    public class MyClass
    {
        public string MyProperty1
        {
            get;
            set;
        }

        [MyAttribute]
        public int MyProperty2
        {
            get;
            set;
        }

        public void Method1(string Parameter1, int Parameter2)
        {
            throw new NotImplementedException();
        }

        public MyClass(string Parameter1, int Parameter2)
        {
            throw new NotImplementedException();
        }

        private string _myField1;

        private string _myField2;

        public enum MyEnum
        {
            Member1 = 1,
            Member2 = 2
        }

        public class MySubClass
        {
            public string MyProperty1
            {
                get;
                set;
            }
    
            [MyAttribute]
            public int MyProperty2
            {
                get;
                set;
            }
    
            private string _myField1;
    
            private string _myField2;

            public class MySubSubClass
            {
                public class MySubSubSubClass
                {
                }
            }
        }
    }
}
");
    }

    [Fact]
    public void GeneratesDefaultUsings()
    {
        // Arrange
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, Enumerable.Empty<ITypeBase>());

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
");
    }

    [Fact]
    public void Generates_Class_With_GeneratedCodeAttribute()
    {
        // Arrange
        var model = new[]
        {
            new ClassBuilder()
                .WithName("MyClass")
                .WithNamespace("MyNamespace")
                .AddGeneratedCodeAttribute("MyGenerator", "1.2.3.4")
                .Build()
        };
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute(@""MyGenerator"", @""1.2.3.4"")]
    public class MyClass
    {
    }
}
");
    }

    [Fact]
    public void Generates_Class_With_ExcludeFromCodeCoverageAttribute()
    {
        // Arrange
        var model = new[]
        {
            new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").AddExcludeFromCodeCoverageAttribute().Build()
        };
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute]
    public class MyClass
    {
    }
}
");
    }

    [Fact]
    public void GeneratesClassWithDefaultParameters()
    {
        // Arrange
        var model = new[]
        {
            new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").Build()
        };
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
    public class MyClass
    {
    }
}
");
    }

    [Fact]
    public void GeneratesInterface()
    {
        // Arrange
        var model = new[]
        {
            new InterfaceBuilder()
                .WithName("IMyInterface")
                .WithNamespace("MyNamespace")
                .Build()
        };
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
    public interface IMyInterface
    {
    }
}
");
    }

    [Fact]
    public void Generates_Interface_With_GeneratedCodeAttribute()
    {
        // Arrange
        var model = new[]
        {
            new InterfaceBuilder()
                .WithName("MyClass")
                .WithNamespace("MyNamespace")
                .AddGeneratedCodeAttribute("MyGenerator", "1.2.3.4")
                .Build()
        };
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute(@""MyGenerator"", @""1.2.3.4"")]
    public interface MyClass
    {
    }
}
");
    }

    [Fact]
    public void GeneratesRecord()
    {
        // Arrange
        var model = new[]
        {
            new ClassBuilder()
                .WithName("MyRecord")
                .WithNamespace("MyNamespace")
                .WithRecord()
                .AddProperties(new ClassPropertyBuilder().WithName("Property1").WithType(typeof(string)).WithHasInitializer())
                .Build()
        };
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
    public record MyRecord
    {
        public string Property1
        {
            get;
            init;
        }
    }
}
");
    }

    [Fact]
    public void GeneratesRecordUsingReflection()
    {
        // Arrange
        var input = typeof(Person);

        // Act
        var actual = input.ToClass();
        var sut = new CSharpClassGenerator();
        var code = TemplateRenderHelper.GetTemplateOutput(sut, new[] { actual });

        // Assert
        actual.Record.Should().BeTrue();
        code.Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelFramework.Generators.Objects.Tests
{
    public record Person
    {
        public string FirstName
        {
            get;
            init;
        }

        public string LastName
        {
            get;
            init;
        }
    }
}
");
    }

    [Fact]
    public void GeneratesPartialInterface()
    {
        // Arrange
        var model = new[]
        {
            new InterfaceBuilder().WithName("IMyInterface").WithNamespace("MyNamespace").WithPartial().Build()
        };
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
    public partial interface IMyInterface
    {
    }
}
");
    }

    [Fact]
    public void GeneratesOnlyClassOnRecursion()
    {
        // Arrange
        var rootTemplate = new CSharpClassGenerator();
        rootTemplate.Initialize();
        rootTemplate.TemplateContext.Model = new ClassBuilder().WithName("ParentClass").WithNamespace("MyNamespace").Build();
        var sut = new CSharpClassGenerator();
        var model = new[]
        {
            new ClassBuilder().WithName("MySubClass").WithNamespace("MyNamespace").Build()
        };
        sut.TemplateContext = TemplateInstanceContext.CreateRootContext(rootTemplate).CreateChildContext(sut, sut.Model);

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"    public class MySubClass
    {
    }
");
    }

    [Fact]
    public void GeneratesPrivateClass()
    {
        // Arrange
        var model = new[]
        {
            new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").WithVisibility(Visibility.Private).Build()
        };
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
    private class MyClass
    {
    }
}
");
    }

    [Fact]
    public void GeneratesInternalClass()
    {
        // Arrange
        var model = new[]
        {
            new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").WithVisibility(Visibility.Internal).Build()
        };
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
    internal class MyClass
    {
    }
}
");
    }

    [Fact]
    public void GeneratesStaticClass()
    {
        // Arrange
        var model = new[]
        {
            new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").WithStatic().Build()
        };
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
    public static class MyClass
    {
    }
}
");
    }

    [Fact]
    public void GeneratesPartialClass()
    {
        // Arrange
        var model = new[]
        {
            new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").WithPartial().Build()
        };
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
    public partial class MyClass
    {
    }
}
");
    }

    [Fact]
    public void GeneratesStaticPartialClass()
    {
        // Arrange
        var model = new[]
        {
            new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").WithStatic().WithPartial().Build()
        };
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
    public static partial class MyClass
    {
    }
}
");
    }

    [Fact]
    public void GeneratesSealedClass()
    {
        // Arrange
        var model = new[]
        {
            new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").WithSealed().Build()
        };
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
    public sealed class MyClass
    {
    }
}
");
    }

    [Fact]
    public void GeneratesAbstractClass()
    {
        // Arrange
        var model = new[]
        {
            new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").WithAbstract().Build()
        };
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
    public abstract class MyClass
    {
    }
}
");
    }

    [Fact]
    public void GeneratesClassWithCustomAttributeTemplate()
    {
        // Arrange
        var sut = new CSharpClassGenerator
        {
            Session = new Dictionary<string, object>
            {
                {
                    nameof(CSharpClassGenerator.Model), new[]
                    {
                        new ClassBuilder()
                            .WithName("MyClass")
                            .WithNamespace("MyNamespace")
                            .AddAttributes
                            (
                                new AttributeBuilder()
                                    .WithName("MyAttribute")
                                    .AddMetadata(new MetadataBuilder().WithName(Common.MetadataNames.CustomTemplateName).WithValue("MyTemplate"))
                            ).Build()
                    }
                }
            }
        };
        var builder = new StringBuilder();
        sut.Initialize();
        sut.RegisterChildTemplate("MyTemplate", () => new DelegateTemplate { RenderDelegate = new Action<StringBuilder>(b => b.AppendLine("    [MyCustomTemplate]")) });

        // Act
        sut.Render(builder);
        var actual = builder.ToString();

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
    [MyCustomTemplate]
    public class MyClass
    {
    }
}
");
    }

    [Fact]
    public void GeneratesClassWithCustomPropertyTemplate()
    {
        // Arrange
        var sut = new CSharpClassGenerator
        {
            Session = new Dictionary<string, object>
            {
                {
                    nameof(CSharpClassGenerator.Model), new[]
                    {
                        new ClassBuilder()
                            .WithName("MyClass")
                            .WithNamespace("MyNamespace")
                            .AddProperties
                            (
                                new ClassPropertyBuilder()
                                    .WithName("MyAttribute")
                                    .WithTypeName("string")
                                    .AddMetadata
                                    (
                                        new MetadataBuilder().WithName(Common.MetadataNames.CustomTemplateName).WithValue("MyTemplate")
                                    )
                            ).Build()
                    }
                }
            }
        };
        sut.Initialize();
        sut.RegisterChildTemplate("MyTemplate", () => new DelegateTemplate { RenderDelegate = new Action<StringBuilder>(b => b.AppendLine("        public string MyCustomTemplateGeneratedProperty { get; set; }")) });
        var builder = new StringBuilder();

        // Act
        sut.Render(builder);
        var actual = builder.ToString();

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
    public class MyClass
    {
        public string MyCustomTemplateGeneratedProperty { get; set; }
    }
}
");
    }

    [Fact]
    public void GeneratesClassWithCustomMethodTemplate()
    {
        // Arrange
        var sut = new CSharpClassGenerator
        {
            Session = new Dictionary<string, object>
            {
                {
                    nameof(CSharpClassGenerator.Model), new[]
                    {
                        new ClassBuilder()
                            .WithName("MyClass")
                            .WithNamespace("MyNamespace")
                            .AddMethods
                            (
                                new ClassMethodBuilder()
                                    .WithName("MyMethod")
                                    .WithTypeName("string")
                                    .AddMetadata
                                    (
                                        new MetadataBuilder().WithName(Common.MetadataNames.CustomTemplateName).WithValue("MyTemplate")
                                    )
                            ).Build()
                    }
                }
            }
        };
        sut.Initialize();
        sut.RegisterChildTemplate("MyTemplate", () => new DelegateTemplate { RenderDelegate = new Action<StringBuilder>(b => b.AppendLine("        public string MyCustomTemplateGeneratedMethod() { throw new NotImplementedException; }")) });
        var builder = new StringBuilder();

        // Act
        sut.Render(builder);
        var actual = builder.ToString();

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
    public class MyClass
    {
        public string MyCustomTemplateGeneratedMethod() { throw new NotImplementedException; }
    }
}
");
    }

    [Fact]
    public void GeneratesClassWithCustomCtorTemplate()
    {
        // Arrange
        var sut = new CSharpClassGenerator
        {
            Session = new Dictionary<string, object>
            {
                {
                    nameof(CSharpClassGenerator.Model), new[]
                    {
                        new ClassBuilder()
                            .WithName("MyClass")
                            .WithNamespace("MyNamespace")
                            .AddConstructors
                            (
                                new ClassConstructorBuilder()
                                    .AddMetadata
                                    (
                                        new MetadataBuilder().WithName(Common.MetadataNames.CustomTemplateName).WithValue("MyTemplate")
                                    )
                            ).Build()
                    }
                }
            }
        };
        sut.Initialize();
        sut.RegisterChildTemplate("MyTemplate", () => new DelegateTemplate { RenderDelegate = new Action<StringBuilder>(b => b.AppendLine("        public MyClass() { throw new NotImplementedException; }")) });
        var builder = new StringBuilder();

        // Act
        sut.Render(builder);
        var actual = builder.ToString();

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
    public class MyClass
    {
        public MyClass() { throw new NotImplementedException; }
    }
}
");
    }

    [Fact]
    public void GeneratesClassWithCustomFieldTemplate()
    {
        // Arrange
        var sut = new CSharpClassGenerator
        {
            Session = new Dictionary<string, object>
            {
                {
                    nameof(CSharpClassGenerator.Model), new[]
                    {
                        new ClassBuilder()
                            .WithName("MyClass")
                            .WithNamespace("MyNamespace")
                            .AddFields
                            (
                                new ClassFieldBuilder()
                                    .WithName("MyField")
                                    .WithTypeName("string")
                                    .AddMetadata
                                    (
                                        new MetadataBuilder().WithName(Common.MetadataNames.CustomTemplateName).WithValue("MyTemplate")
                                    )
                            ).Build()
                    }
                }
            }
        };
        sut.Initialize();
        sut.RegisterChildTemplate("MyTemplate", () => new DelegateTemplate { RenderDelegate = new Action<StringBuilder>(b => b.AppendLine("        public string myCustomFieldInjectedFromTemplate;")) });
        var builder = new StringBuilder();

        // Act
        sut.Render(builder);
        var actual = builder.ToString();

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
    public class MyClass
    {
        public string myCustomFieldInjectedFromTemplate;
    }
}
");
    }

    [Fact]
    public void GeneratesClassWithCustomEnumTemplate()
    {
        // Arrange
        var sut = new CSharpClassGenerator
        {
            Session = new Dictionary<string, object>
            {
                {
                    nameof(CSharpClassGenerator.Model), new[]
                    {
                        new ClassBuilder()
                            .WithName("MyClass")
                            .WithNamespace("MyNamespace")
                            .AddEnums
                            (
                                new EnumBuilder()
                                    .WithName("MyEnum")
                                    .AddMembers
                                    (
                                        new EnumMemberBuilder().WithName("Member1"),
                                        new EnumMemberBuilder().WithName("Member2"),
                                        new EnumMemberBuilder().WithName("Member3")
                                    )
                                    .AddMetadata
                                    (
                                        new MetadataBuilder().WithName(Common.MetadataNames.CustomTemplateName).WithValue("MyTemplate")
                                    )
                            ).Build()
                    }
                }
            }
        };
        sut.Initialize();
        sut.RegisterChildTemplate("MyTemplate", () => new DelegateTemplate { RenderDelegate = new Action<StringBuilder>(b => b.AppendLine("        public enum MyEnum { One, Two, Three };")) });
        var builder = new StringBuilder();

        // Act
        sut.Render(builder);
        var actual = builder.ToString();

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
    public class MyClass
    {
        public enum MyEnum { One, Two, Three };
    }
}
");
    }

    [Fact]
    public void GeneratesMultipleNamespacesAndMultipleClassesSortedAlphabetically()
    {
        // Arrange
        var model = new[]
        {
            new ClassBuilder().WithName("MyClass2").WithNamespace("Namespace1").Build(),
            new ClassBuilder().WithName("MyClass1").WithNamespace("Namespace2").Build(),
            new ClassBuilder().WithName("MyClass2").WithNamespace("Namespace2").Build(),
            new ClassBuilder().WithName("MyClass1").WithNamespace("Namespace1").Build(),
        };
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Namespace1
{
    public class MyClass1
    {
    }

    public class MyClass2
    {
    }
}

namespace Namespace2
{
    public class MyClass1
    {
    }

    public class MyClass2
    {
    }
}
");
    }

    [Fact]
    public void GeneratesClassesWithMultipleOutput()
    {
        // Arrange
        var model = new[]
        {
            new ClassBuilder().WithName("MyClass1").WithNamespace("MyNamespace").Build(),
            new ClassBuilder().WithName("MyClass2").WithNamespace("MyNamespace").Build()
        };
        var additionalParameters = new Dictionary<string, object>
        {
            {
                nameof(CSharpClassGenerator.GenerateMultipleFiles),
                true
            }
        };
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model, additionalParameters: additionalParameters);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"<?xml version=""1.0"" encoding=""utf-16""?>
<MultipleContents xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""http://schemas.datacontract.org/2004/07/TextTemplateTransformationFramework"">
  <BasePath i:nil=""true"" />
  <Contents>
    <Contents>
      <FileName>MyClass1.cs</FileName>
      <Lines xmlns:d4p1=""http://schemas.microsoft.com/2003/10/Serialization/Arrays"">
        <d4p1:string>using System;</d4p1:string>
        <d4p1:string>using System.Collections.Generic;</d4p1:string>
        <d4p1:string>using System.Linq;</d4p1:string>
        <d4p1:string>using System.Text;</d4p1:string>
        <d4p1:string></d4p1:string>
        <d4p1:string>namespace MyNamespace</d4p1:string>
        <d4p1:string>{</d4p1:string>
        <d4p1:string>    public class MyClass1</d4p1:string>
        <d4p1:string>    {</d4p1:string>
        <d4p1:string>    }</d4p1:string>
        <d4p1:string>}</d4p1:string>
        <d4p1:string></d4p1:string>
      </Lines>
      <SkipWhenFileExists>false</SkipWhenFileExists>
    </Contents>
    <Contents>
      <FileName>MyClass2.cs</FileName>
      <Lines xmlns:d4p1=""http://schemas.microsoft.com/2003/10/Serialization/Arrays"">
        <d4p1:string>using System;</d4p1:string>
        <d4p1:string>using System.Collections.Generic;</d4p1:string>
        <d4p1:string>using System.Linq;</d4p1:string>
        <d4p1:string>using System.Text;</d4p1:string>
        <d4p1:string></d4p1:string>
        <d4p1:string>namespace MyNamespace</d4p1:string>
        <d4p1:string>{</d4p1:string>
        <d4p1:string>    public class MyClass2</d4p1:string>
        <d4p1:string>    {</d4p1:string>
        <d4p1:string>    }</d4p1:string>
        <d4p1:string>}</d4p1:string>
        <d4p1:string></d4p1:string>
      </Lines>
      <SkipWhenFileExists>false</SkipWhenFileExists>
    </Contents>
  </Contents>
</MultipleContents>");
    }

    [Fact]
    public void GeneratesImmutableClassWithInjectedTemplates_Model_With_Method()
    {
        // Arrange
        var properties = new[]
        {
            new ClassPropertyBuilder().WithName("Property1").WithType(typeof(string)),
            new ClassPropertyBuilder().WithName("Property2").WithType(typeof(bool))
        };

        var model = new[]
        {
            new ClassBuilder()
                .WithName("MyRecord")
                .WithNamespace("MyNamespace")
                .AddProperties(properties)
                .Build()
                .ToImmutableClass(new ImmutableClassSettings(createWithMethod: true))
        };
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(ImmutableClassCode);
    }

    [Fact]
    public void GeneratesImmutableClassWithIEquatable()
    {
        // Arrange
        var properties = new[]
        {
            new ClassPropertyBuilder().WithName("Property1").WithType(typeof(string)),
            new ClassPropertyBuilder().WithName("Property2").WithType(typeof(bool))
        };

        var model = new[]
        {
            new ClassBuilder()
                .WithName("MyRecord")
                .WithNamespace("MyNamespace")
                .AddProperties(properties)
                .Build()
                .ToImmutableClass(new ImmutableClassSettings(implementIEquatable: true))
        };
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
    public class MyRecord : IEquatable<MyRecord>
    {
        public string Property1
        {
            get;
        }

        public bool Property2
        {
            get;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as MyRecord);
        }

        public bool Equals(MyRecord other)
        {
            return other != null &&
                   Property1 == other.Property1 &&
                   Property2 == other.Property2;
        }

        public override int GetHashCode()
        {
            int hashCode = 235838129;
            hashCode = hashCode * -1521134295 + EqualityComparer<System.String>.Default.GetHashCode(Property1);
            hashCode = hashCode * -1521134295 + Property2.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(MyRecord left, MyRecord right)
        {
            return EqualityComparer<MyRecord>.Default.Equals(left, right);
        }

        public static bool operator !=(MyRecord left, MyRecord right)
        {
            return !(left == right);
        }

        public MyRecord(string property1, bool property2)
        {
            this.Property1 = property1;
            this.Property2 = property2;
        }
    }
}
");
    }

    [Fact]
    public void GeneratesImmutableClassWithInjectedTemplates_Model_No_With_Method()
    {
        // Arrange
        var properties = new[]
        {
                new ClassPropertyBuilder().WithName("Property1").WithType(typeof(string)),
                new ClassPropertyBuilder().WithName("Property2").WithType(typeof(bool))
            };

        var model = new[]
        {
            new ClassBuilder()
                .WithName("MyRecord")
                .WithNamespace("MyNamespace")
                .AddProperties(properties)
                .Build()
                .ToImmutableClass(new ImmutableClassSettings(createWithMethod: false))
        };
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(ImmutableClassCodeNoWithMethod);
    }

    [Fact]
    public void GeneratesImmutableBuilderClassWithNullChecks()
    {
        // Arrange
        var properties = new[]
        {
            new ClassPropertyBuilder().WithName("Property1").WithType(typeof(string)),
            new ClassPropertyBuilder().WithName("Property2").WithType(typeof(ICollection<string>)).ConvertCollectionOnBuilderToEnumerable(true),
            new ClassPropertyBuilder().WithName("Property3").WithTypeName("MyCustomType").ConvertSinglePropertyToBuilderOnBuilder(),
            new ClassPropertyBuilder().WithName("Property4").WithTypeName(typeof(ICollection<string>).FullName!.FixTypeName().Replace("System.String","MyCustomType")).ConvertCollectionPropertyToBuilderOnBuilder(true)
        };
        var cls = new ClassBuilder()
            .WithName("MyRecord")
            .WithNamespace("MyNamespace")
            .AddProperties(properties)
            .Build()
            .ToImmutableClass(new ImmutableClassSettings("System.Collections.Generic.IReadOnlyCollection"));
        var settings = new ImmutableBuilderClassSettings(constructorSettings: new ImmutableBuilderClassConstructorSettings(addCopyConstructor: true, addNullChecks: true));
        var model = new[]
        {
            cls,
            cls.ToImmutableBuilderClass(settings)
        };
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(ImmutableBuilderClassCodeWithNullChecks);
    }

    [Fact]
    public void GeneratesImmutableBuilderClassWithoutNullChecks()
    {
        // Arrange
        var properties = new[]
        {
            new ClassPropertyBuilder().WithName("Property1").WithType(typeof(string)),
            new ClassPropertyBuilder().WithName("Property2").WithTypeName(typeof(ICollection<string>).FullName!.FixTypeName()).ConvertCollectionOnBuilderToEnumerable(true),
            new ClassPropertyBuilder().WithName("Property3").WithTypeName("MyCustomType").ConvertSinglePropertyToBuilderOnBuilder(),
            new ClassPropertyBuilder().WithName("Property4").WithTypeName(typeof(ICollection<string>).FullName!.FixTypeName().Replace("System.String","MyCustomType")).ConvertCollectionPropertyToBuilderOnBuilder(true)
        };
        var cls = new ClassBuilder()
            .WithName("MyRecord")
            .WithNamespace("MyNamespace")
            .AddProperties(properties)
            .Build()
            .ToImmutableClass(new ImmutableClassSettings("System.Collections.Generic.IReadOnlyCollection"));
        var settings = new ImmutableBuilderClassSettings(constructorSettings: new ImmutableBuilderClassConstructorSettings(addCopyConstructor: true, addNullChecks: false));
        var model = new[]
        {
            cls,
            cls.ToImmutableBuilderClass(settings)
        };
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(ImmutableBuilderClassCodeWithoutNullChecks);
    }

    [Fact]
    public void GeneratesImmutableBuilderClassWithReservedKeyword()
    {
        // Arrange
        var cls = new ClassBuilder()
            .WithName("MyRecord")
            .WithNamespace("MyNamespace")
            .AddProperties(new ClassPropertyBuilder().WithName("Static").WithType(typeof(bool)))
            .Build()
            .ToImmutableClass(new ImmutableClassSettings());
        var settings = new ImmutableBuilderClassSettings(constructorSettings: new ImmutableBuilderClassConstructorSettings(addCopyConstructor: true));
        var model = new[]
        {
            cls,
            cls.ToImmutableBuilderClass(settings)
        };
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
    public class MyRecord
    {
        public bool Static
        {
            get;
        }

        public MyRecord(bool @static)
        {
            this.Static = @static;
        }
    }

    public class MyRecordBuilder
    {
        public bool Static
        {
            get;
            set;
        }

        public MyNamespace.MyRecord Build()
        {
            return new MyNamespace.MyRecord(Static);
        }

        public MyRecordBuilder WithStatic(bool @static)
        {
            Static = @static;
            return this;
        }

        public MyRecordBuilder()
        {
            Static = default;
        }

        public MyRecordBuilder(MyNamespace.MyRecord source)
        {
            Static = source.Static;
        }
    }
}
");
    }

    [Fact]
    public void GeneratesImmutableBuilderClassWithDifferentSetMethodName()
    {
        // Arrange
        var properties = new[]
        {
            new ClassPropertyBuilder().WithName("Property1").WithType(typeof(string))
        };
        var cls = new ClassBuilder()
            .WithName("MyRecord")
            .WithNamespace("MyNamespace")
            .AddProperties(properties)
            .Build()
            .ToImmutableClass(new ImmutableClassSettings("System.Collections.Generic.IReadOnlyCollection"));
        var settings = new ImmutableBuilderClassSettings(nameSettings: new ImmutableBuilderClassNameSettings(setMethodNameFormatString: "Set{0}"));
        var model = new[]
        {
            cls,
            cls.ToImmutableBuilderClass(settings)
        };
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
    public class MyRecord
    {
        public string Property1
        {
            get;
        }

        public MyRecord(string property1)
        {
            this.Property1 = property1;
        }
    }

    public class MyRecordBuilder
    {
        public string Property1
        {
            get;
            set;
        }

        public MyNamespace.MyRecord Build()
        {
            return new MyNamespace.MyRecord(Property1);
        }

        public MyRecordBuilder SetProperty1(string property1)
        {
            Property1 = property1;
            return this;
        }

        public MyRecordBuilder()
        {
            Property1 = string.Empty;
        }
    }
}
");
    }

    [Fact]
    public void GeneratesImmutableBuilderClassWithAttributes()
    {
        // Arrange
        var properties = new[]
        {
            new ClassPropertyBuilder().WithName("Property1")
                                        .WithType(typeof(string))
                                        .AddAttributes(new AttributeBuilder().WithName("MyAttribute")),
        };
        var cls = new ClassBuilder()
            .WithName("MyRecord")
            .WithNamespace("MyNamespace")
            .AddProperties(properties)
            .Build()
            .ToImmutableClass(new ImmutableClassSettings("System.Collections.Generic.IReadOnlyCollection"));
        var settings = new ImmutableBuilderClassSettings();
        var model = new[]
        {
            cls,
            cls.ToImmutableBuilderClass(settings)
        };
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
    public class MyRecord
    {
        [MyAttribute]
        public string Property1
        {
            get;
        }

        public MyRecord(string property1)
        {
            this.Property1 = property1;
        }
    }

    public class MyRecordBuilder
    {
        [MyAttribute]
        public string Property1
        {
            get;
            set;
        }

        public MyNamespace.MyRecord Build()
        {
            return new MyNamespace.MyRecord(Property1);
        }

        public MyRecordBuilder WithProperty1(string property1)
        {
            Property1 = property1;
            return this;
        }

        public MyRecordBuilder()
        {
            Property1 = string.Empty;
        }
    }
}
");
    }

    [Fact]
    public void GeneratesImmutableBuilderClassWithNullableProperty()
    {
        // Arrange
        var properties = new[]
        {
            new ClassPropertyBuilder().WithName("Property1").WithType(typeof(string)).WithIsNullable()
        };
        var cls = new ClassBuilder()
            .WithName("MyRecord")
            .WithNamespace("MyNamespace")
            .AddProperties(properties)
            .Build()
            .ToImmutableClass(new ImmutableClassSettings("System.Collections.Generic.IReadOnlyCollection"));
        var settings = new ImmutableBuilderClassSettings();
        var model = new[]
        {
            cls,
            cls.ToImmutableBuilderClass(settings)
        };
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model, additionalParameters: new { EnableNullableContext = true, CreateCodeGenerationHeader = true, EnvironmentVersion = "1.0" });

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 1.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
#nullable enable
    public class MyRecord
    {
        public string? Property1
        {
            get;
        }

        public MyRecord(string? property1)
        {
            this.Property1 = property1;
        }
    }
#nullable restore

#nullable enable
    public class MyRecordBuilder
    {
        public string? Property1
        {
            get;
            set;
        }

        public MyNamespace.MyRecord Build()
        {
            return new MyNamespace.MyRecord(Property1);
        }

        public MyRecordBuilder WithProperty1(string? property1)
        {
            Property1 = property1;
            return this;
        }

        public MyRecordBuilder()
        {
        }
    }
#nullable restore
}
");
    }

    [Fact]
    public void GeneratesImmutableBuilderClassWithObservableProperty()
    {
        // Arrange
        var properties = new[]
        {
            new ClassPropertyBuilder().WithName("Property1").WithType(typeof(string))
        };
        var cls = new ClassBuilder()
            .WithName("MyRecord")
            .WithNamespace("MyNamespace")
            .AddProperties(properties)
            .Build()
            .ToObservableClass();
        var settings = new ImmutableBuilderClassSettings();
        var model = new[]
        {
            cls,
            cls.ToImmutableBuilderClass(settings)
        };
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
    public class MyRecord
    {
        public string Property1
        {
            get
            {
                return _property1;
            }
            set
            {
                _property1 = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(""Property1""));
            }
        }

        public MyRecord()
        {
        }

        private string _property1;
    }

    public class MyRecordBuilder
    {
        public string Property1
        {
            get
            {
                return _property1;
            }
            set
            {
                _property1 = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(""Property1""));
            }
        }

        public MyNamespace.MyRecord Build()
        {
            return new MyNamespace.MyRecord { Property1 = Property1 };
        }

        public MyRecordBuilder WithProperty1(string property1)
        {
            Property1 = property1;
            return this;
        }

        public MyRecordBuilder()
        {
            Property1 = string.Empty;
        }

        protected string _property1;
    }
}
");
    }

    [Fact]
    public void GeneratesImmutableBuilderClassWithAdditionalConstructorParameter()
    {
        // Arrange
        var properties = new[]
        {
            new ClassPropertyBuilder().WithName("Property1").WithType(typeof(string)).WithIsNullable()
        };
        var cls = new ClassBuilder()
            .WithName("MyRecord")
            .WithNamespace("MyNamespace")
            .AddProperties(properties)
            .Build()
            .ToImmutableClassBuilder(new ImmutableClassSettings("System.Collections.Generic.IReadOnlyCollection"))
            .AddBuilderCopyConstructorAdditionalArguments(new ParameterBuilder().WithType(typeof(string)).WithName("additionalArgument"))
            .Build();
        var settings = new ImmutableBuilderClassSettings(constructorSettings: new ImmutableBuilderClassConstructorSettings(addCopyConstructor: true));
        var model = new[]
        {
            cls,
            cls.ToImmutableBuilderClass(settings)
        };
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model, additionalParameters: new { EnableNullableContext = true });

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
    public class MyRecord
    {
        public string? Property1
        {
            get;
        }

        public MyRecord(string? property1)
        {
            this.Property1 = property1;
        }
    }

    public class MyRecordBuilder
    {
        public string? Property1
        {
            get;
            set;
        }

        public MyNamespace.MyRecord Build()
        {
            return new MyNamespace.MyRecord(Property1);
        }

        public MyRecordBuilder WithProperty1(string? property1)
        {
            Property1 = property1;
            return this;
        }

        public MyRecordBuilder()
        {
        }

        public MyRecordBuilder(MyNamespace.MyRecord source, string additionalArgument)
        {
            Property1 = source.Property1;
        }
    }
}
");
    }

    [Fact]
    public void GeneratesImmutableBuilderClassWithEntityInheritance()
    {
        // Arrange
        var baseClass = typeof(BaseClass).ToClass().ToImmutableClass(new ImmutableClassSettings());
        var immutableClassSettings = new ImmutableClassSettings(
            constructorSettings: new ImmutableClassConstructorSettings(validateArguments: ArgumentValidationType.DomainOnly, addNullChecks: true),
            inheritanceSettings: new ImmutableClassInheritanceSettings(enableInheritance: true, baseClass: baseClass),
            addPrivateSetters: true
        );
        var cls = typeof(InheritedClass)
            .ToClass()
            .ToImmutableClass(immutableClassSettings);
        var settings = new ImmutableBuilderClassSettings(
            constructorSettings: new ImmutableBuilderClassConstructorSettings(
                addCopyConstructor: true,
                addNullChecks: true),
            inheritanceSettings: new ImmutableBuilderClassInheritanceSettings(enableEntityInheritance: true, baseClass: baseClass)
        );
        var model = new[]
        {
            cls,
            cls.ToImmutableBuilderClass(settings)
        };
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model, additionalParameters: new { EnableNullableContext = true });

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelFramework.Generators.Objects.Tests.POC
{
    public class InheritedClass : ModelFramework.Generators.Objects.Tests.POC.BaseClass
    {
        public string AdditionalProperty
        {
            get;
            private set;
        }

        public InheritedClass(string additionalProperty, string baseProperty) : base(baseProperty)
        {
            if (additionalProperty == null) throw new System.ArgumentNullException(""additionalProperty"");
            this.AdditionalProperty = additionalProperty;
            System.ComponentModel.DataAnnotations.Validator.ValidateObject(this, new System.ComponentModel.DataAnnotations.ValidationContext(this, null, null), true);
        }
    }

    public class InheritedClassBuilder : BaseClassBuilder<InheritedClassBuilder, ModelFramework.Generators.Objects.Tests.POC.InheritedClass>
    {
        public string AdditionalProperty
        {
            get;
            set;
        }

        public override ModelFramework.Generators.Objects.Tests.POC.InheritedClass BuildTyped()
        {
            return new ModelFramework.Generators.Objects.Tests.POC.InheritedClass(AdditionalProperty, BaseProperty);
        }

        public override ModelFramework.Generators.Objects.Tests.POC.BaseClass Build()
        {
            return BuildTyped();
        }

        public InheritedClassBuilder WithAdditionalProperty(string additionalProperty)
        {
            AdditionalProperty = additionalProperty;
            return this;
        }

        public InheritedClassBuilder() : base()
        {
            AdditionalProperty = string.Empty;
        }

        public InheritedClassBuilder(ModelFramework.Generators.Objects.Tests.POC.InheritedClass source) : base(source)
        {
            if (source == null)
            {
                throw new System.ArgumentNullException(""source"");
            }
            AdditionalProperty = source.AdditionalProperty;
        }
    }
}
");
    }

    [Fact]
    public void GeneratesImmutableBuilderClassWithEntityAndBuilderInheritance()
    {
        // Arrange
        var baseClass = typeof(BaseClass).ToClass().ToImmutableClass(new ImmutableClassSettings());
        var immutableClassSettings = new ImmutableClassSettings(
            constructorSettings: new ImmutableClassConstructorSettings(validateArguments: ArgumentValidationType.DomainOnly, addNullChecks: true),
            inheritanceSettings: new ImmutableClassInheritanceSettings(enableInheritance: true, baseClass: baseClass),
            addPrivateSetters: true
        );
        var cls = typeof(InheritedClass)
            .ToClass()
            .ToImmutableClass(immutableClassSettings);
        var settings = new ImmutableBuilderClassSettings(
            constructorSettings: new ImmutableBuilderClassConstructorSettings(
                addCopyConstructor: true,
                addNullChecks: true),
            inheritanceSettings: new ImmutableBuilderClassInheritanceSettings(enableEntityInheritance: true, enableBuilderInheritance: true, baseClass: baseClass)
        );
        var model = new[]
        {
            cls,
            cls.ToImmutableBuilderClass(settings)
        };
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model, additionalParameters: new { EnableNullableContext = true });

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelFramework.Generators.Objects.Tests.POC
{
    public class InheritedClass : ModelFramework.Generators.Objects.Tests.POC.BaseClass
    {
        public string AdditionalProperty
        {
            get;
            private set;
        }

        public InheritedClass(string additionalProperty, string baseProperty) : base(baseProperty)
        {
            if (additionalProperty == null) throw new System.ArgumentNullException(""additionalProperty"");
            this.AdditionalProperty = additionalProperty;
            System.ComponentModel.DataAnnotations.Validator.ValidateObject(this, new System.ComponentModel.DataAnnotations.ValidationContext(this, null, null), true);
        }
    }

    public class InheritedClassBuilder : BaseClassBuilder
    {
        public string AdditionalProperty
        {
            get;
            set;
        }

        public override ModelFramework.Generators.Objects.Tests.POC.InheritedClass BuildTyped()
        {
            return new ModelFramework.Generators.Objects.Tests.POC.InheritedClass(AdditionalProperty, BaseProperty);
        }

        public override ModelFramework.Generators.Objects.Tests.POC.BaseClass Build()
        {
            return BuildTyped();
        }

        public InheritedClassBuilder WithAdditionalProperty(string additionalProperty)
        {
            AdditionalProperty = additionalProperty;
            return this;
        }

        public InheritedClassBuilder WithBaseProperty(string baseProperty)
        {
            BaseProperty = baseProperty;
            return this;
        }

        public InheritedClassBuilder() : base()
        {
            AdditionalProperty = string.Empty;
        }

        public InheritedClassBuilder(ModelFramework.Generators.Objects.Tests.POC.InheritedClass source) : base(source)
        {
            if (source == null)
            {
                throw new System.ArgumentNullException(""source"");
            }
            AdditionalProperty = source.AdditionalProperty;
        }
    }
}
");
    }

    [Fact]
    public void GeneratesImmutableBuilderClassFromAbstractType()
    {
        // Arrange
        var immutableClassSettings = new ImmutableClassSettings(
            constructorSettings: new ImmutableClassConstructorSettings(validateArguments: ArgumentValidationType.DomainOnly, addNullChecks: true),
            inheritanceSettings: new ImmutableClassInheritanceSettings(enableInheritance: true),
            addPrivateSetters: true
        );
        var cls = typeof(BaseClass)
            .ToClass()
            .ToImmutableClass(immutableClassSettings);
        var settings = new ImmutableBuilderClassSettings(
            constructorSettings: new ImmutableBuilderClassConstructorSettings(
                addCopyConstructor: true,
                addNullChecks: true),
            inheritanceSettings: new ImmutableBuilderClassInheritanceSettings(enableEntityInheritance: true, baseClass: null)
        );
        var model = new[]
        {
            cls,
            cls.ToImmutableBuilderClass(settings)
        };
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model, additionalParameters: new { EnableNullableContext = true });

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelFramework.Generators.Objects.Tests.POC
{
    public abstract class BaseClass
    {
        public string BaseProperty
        {
            get;
            private set;
        }

        protected BaseClass(string baseProperty)
        {
            if (baseProperty == null) throw new System.ArgumentNullException(""baseProperty"");
            this.BaseProperty = baseProperty;
        }
    }

    public abstract class BaseClassBuilder<TBuilder, TEntity>
        where TEntity : ModelFramework.Generators.Objects.Tests.POC.BaseClass
        where TBuilder : BaseClassBuilder<TBuilder, TEntity>
    {
        public string BaseProperty
        {
            get;
            set;
        }

        public abstract TEntity BuildTyped();

        public override ModelFramework.Generators.Objects.Tests.POC.BaseClass Build()
        {
            return BuildTyped();
        }

        public TBuilder WithBaseProperty(string baseProperty)
        {
            BaseProperty = baseProperty;
            return (TBuilder)this;
        }

        protected BaseClassBuilder()
        {
            BaseProperty = string.Empty;
        }

        protected BaseClassBuilder(ModelFramework.Generators.Objects.Tests.POC.BaseClass source)
        {
            if (source == null)
            {
                throw new System.ArgumentNullException(""source"");
            }
            BaseProperty = source.BaseProperty;
        }
    }
}
");
    }

    [Fact]
    public void GeneratesImmutableBuilderClassFromAbstractTypeWithBaseClass()
    {
        // Arrange
        var baseClass = typeof(BaseClass).ToClass().ToImmutableClass(new ImmutableClassSettings());
        var immutableClassSettings = new ImmutableClassSettings(
            constructorSettings: new ImmutableClassConstructorSettings(validateArguments: ArgumentValidationType.DomainOnly, addNullChecks: true),
            inheritanceSettings: new ImmutableClassInheritanceSettings(enableInheritance: true, baseClass: baseClass, isAbstract: true),
            addPrivateSetters: true
        );
        var cls = typeof(MiddleClass)
            .ToClass()
            .ToImmutableClass(immutableClassSettings);
        var settings = new ImmutableBuilderClassSettings(
            constructorSettings: new ImmutableBuilderClassConstructorSettings(
                addCopyConstructor: true,
                addNullChecks: true),
            inheritanceSettings: new ImmutableBuilderClassInheritanceSettings(enableEntityInheritance: true, baseClass: baseClass, isAbstract: true)
        );
        var model = new[]
        {
            cls,
            cls.ToImmutableBuilderClass(settings)
        };
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model, additionalParameters: new { EnableNullableContext = true });

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelFramework.Generators.Objects.Tests.POC
{
    public abstract class MiddleClass : ModelFramework.Generators.Objects.Tests.POC.BaseClass
    {
        public string MiddleProperty
        {
            get;
            private set;
        }

        protected MiddleClass(string middleProperty, string baseProperty) : base(baseProperty)
        {
            if (middleProperty == null) throw new System.ArgumentNullException(""middleProperty"");
            this.MiddleProperty = middleProperty;
        }
    }

    public abstract class MiddleClassBuilder<TBuilder, TEntity> : BaseClassBuilder<MiddleClassBuilder, ModelFramework.Generators.Objects.Tests.POC.MiddleClass>
        where TEntity : ModelFramework.Generators.Objects.Tests.POC.MiddleClass
        where TBuilder : MiddleClassBuilder<TBuilder, TEntity>
    {
        public string MiddleProperty
        {
            get;
            set;
        }

        public abstract override TEntity BuildTyped();

        public override ModelFramework.Generators.Objects.Tests.POC.BaseClass Build()
        {
            return BuildTyped();
        }

        public TBuilder WithMiddleProperty(string middleProperty)
        {
            MiddleProperty = middleProperty;
            return (TBuilder)this;
        }

        protected MiddleClassBuilder() : base()
        {
            MiddleProperty = string.Empty;
        }

        protected MiddleClassBuilder(ModelFramework.Generators.Objects.Tests.POC.MiddleClass source) : base(source)
        {
            if (source == null)
            {
                throw new System.ArgumentNullException(""source"");
            }
            MiddleProperty = source.MiddleProperty;
        }
    }
}
");
    }

    [Fact]
    public void GeneratesImmutableBuilderClassFromAbstractTypeWithBuilderInheritance()
    {
        // Arrange
        var immutableClassSettings = new ImmutableClassSettings(
            constructorSettings: new ImmutableClassConstructorSettings(validateArguments: ArgumentValidationType.DomainOnly, addNullChecks: true),
            inheritanceSettings: new ImmutableClassInheritanceSettings(enableInheritance: true),
            addPrivateSetters: true
        );
        var cls = typeof(BaseClass)
            .ToClass()
            .ToImmutableClass(immutableClassSettings);
        var settings = new ImmutableBuilderClassSettings(
            constructorSettings: new ImmutableBuilderClassConstructorSettings(
                addCopyConstructor: true,
                addNullChecks: true),
            inheritanceSettings: new ImmutableBuilderClassInheritanceSettings(enableEntityInheritance: true, enableBuilderInheritance: true, baseClass: null)
        );
        var model = new[]
        {
            cls,
            cls.ToNonGenericImmutableBuilderClass(settings),
            cls.ToImmutableBuilderClass(settings)
        };
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model, additionalParameters: new { EnableNullableContext = true });

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelFramework.Generators.Objects.Tests.POC
{
    public abstract class BaseClass
    {
        public string BaseProperty
        {
            get;
            private set;
        }

        protected BaseClass(string baseProperty)
        {
            if (baseProperty == null) throw new System.ArgumentNullException(""baseProperty"");
            this.BaseProperty = baseProperty;
        }
    }

    public abstract class BaseClassBuilder
    {
        public string BaseProperty
        {
            get;
            set;
        }

        public abstract ModelFramework.Generators.Objects.Tests.POC.BaseClass Build();

        protected BaseClassBuilder()
        {
            BaseProperty = string.Empty;
        }

        protected BaseClassBuilder(ModelFramework.Generators.Objects.Tests.POC.BaseClass source)
        {
            if (source == null)
            {
                throw new System.ArgumentNullException(""source"");
            }
            BaseProperty = source.BaseProperty;
        }
    }

    public abstract class BaseClassBuilder<TBuilder, TEntity> : BaseClassBuilder
        where TEntity : ModelFramework.Generators.Objects.Tests.POC.BaseClass
        where TBuilder : BaseClassBuilder<TBuilder, TEntity>
    {
        public abstract TEntity BuildTyped();

        public override ModelFramework.Generators.Objects.Tests.POC.BaseClass Build()
        {
            return BuildTyped();
        }

        public TBuilder WithBaseProperty(string baseProperty)
        {
            BaseProperty = baseProperty;
            return (TBuilder)this;
        }

        protected BaseClassBuilder() : base()
        {
        }

        protected BaseClassBuilder(ModelFramework.Generators.Objects.Tests.POC.BaseClass source) : base(source)
        {
        }
    }
}
");
    }

    [Fact]
    public void GeneratesImmutableBuilderClassFromAbstractTypeWithBaseClassAndBuilderInheritance()
    {
        // Arrange
        var baseClass = typeof(BaseClass).ToClass().ToImmutableClass(new ImmutableClassSettings());
        var immutableClassSettings = new ImmutableClassSettings(
            constructorSettings: new ImmutableClassConstructorSettings(validateArguments: ArgumentValidationType.DomainOnly, addNullChecks: true),
            inheritanceSettings: new ImmutableClassInheritanceSettings(enableInheritance: true, baseClass: baseClass, isAbstract: true),
            addPrivateSetters: true
        );
        var cls = typeof(MiddleClass)
            .ToClass()
            .ToImmutableClass(immutableClassSettings);
        var settings = new ImmutableBuilderClassSettings(
            constructorSettings: new ImmutableBuilderClassConstructorSettings(
                addCopyConstructor: true,
                addNullChecks: true),
            inheritanceSettings: new ImmutableBuilderClassInheritanceSettings(enableEntityInheritance: true, enableBuilderInheritance: true, baseClass: baseClass, isAbstract: true)
        );
        var model = new[]
        {
            cls,
            cls.ToNonGenericImmutableBuilderClass(settings),
            cls.ToImmutableBuilderClass(settings)
        };
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model, additionalParameters: new { EnableNullableContext = true });

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelFramework.Generators.Objects.Tests.POC
{
    public abstract class MiddleClass : ModelFramework.Generators.Objects.Tests.POC.BaseClass
    {
        public string MiddleProperty
        {
            get;
            private set;
        }

        protected MiddleClass(string middleProperty, string baseProperty) : base(baseProperty)
        {
            if (middleProperty == null) throw new System.ArgumentNullException(""middleProperty"");
            this.MiddleProperty = middleProperty;
        }
    }

    public abstract class MiddleClassBuilder : BaseClassBuilder
    {
        public string MiddleProperty
        {
            get;
            set;
        }

        public abstract ModelFramework.Generators.Objects.Tests.POC.MiddleClass Build();

        protected MiddleClassBuilder() : base()
        {
            MiddleProperty = string.Empty;
        }

        protected MiddleClassBuilder(ModelFramework.Generators.Objects.Tests.POC.MiddleClass source) : base(source)
        {
            if (source == null)
            {
                throw new System.ArgumentNullException(""source"");
            }
            MiddleProperty = source.MiddleProperty;
        }
    }

    public abstract class MiddleClassBuilder<TBuilder, TEntity> : MiddleClassBuilder
        where TEntity : ModelFramework.Generators.Objects.Tests.POC.MiddleClass
        where TBuilder : MiddleClassBuilder<TBuilder, TEntity>
    {
        public TBuilder WithMiddleProperty(string middleProperty)
        {
            MiddleProperty = middleProperty;
            return (TBuilder)this;
        }

        public TBuilder WithBaseProperty(string baseProperty)
        {
            BaseProperty = baseProperty;
            return (TBuilder)this;
        }

        protected MiddleClassBuilder() : base()
        {
        }

        protected MiddleClassBuilder(ModelFramework.Generators.Objects.Tests.POC.MiddleClass source) : base(source)
        {
        }
    }
}
");
    }

    [Fact]
    public void GeneratesImmutableClassWithInjectedTemplates_Model_With_Method_Using_ExtensionMethod()
    {
        // Arrange
        var properties = new[]
        {
            new ClassPropertyBuilder().WithName("Property1").WithType(typeof(string)),
            new ClassPropertyBuilder().WithName("Property2").WithType(typeof(bool))
        };

        var cls = new ClassBuilder()
            .WithName("MyRecord")
            .WithNamespace("MyNamespace")
            .AddProperties(properties)
            .Build();

        var model = new[]
        {
            cls.ToImmutableClass(new ImmutableClassSettings(createWithMethod: false)),
            cls.ToImmutableExtensionClass(new ImmutableClassExtensionsSettings())
        };
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
    public class MyRecord
    {
        public string Property1
        {
            get;
        }

        public bool Property2
        {
            get;
        }

        public MyRecord(string property1, bool property2)
        {
            this.Property1 = property1;
            this.Property2 = property2;
        }
    }

    public static class MyRecordExtensions
    {
        public static MyRecord With(this MyRecord instance, string property1 = default(string), bool property2 = default(bool))
        {
            return new MyRecord
            (
                property1 == default(string) ? instance.Property1 : property1,
                property2 == default(bool) ? instance.Property2 : property2
            );
        }
    }
}
");
    }

    [Fact]
    public void Can_Generate_Builder_Extensions_Class()
    {
        // Arrange
        var properties = new[]
        {
            new ClassPropertyBuilder().WithName("Property1").WithType(typeof(string)),
            new ClassPropertyBuilder().WithName("Property2").WithType(typeof(bool))
        };

        var cls = new ClassBuilder()
            .WithName("MyRecord")
            .WithNamespace("MyNamespace")
            .AddProperties(properties)
            .Build();

        var model = new[]
        {
            cls.ToImmutableClass(new ImmutableClassSettings(createWithMethod: false))
                .ToImmutableBuilderClassBuilder(new ImmutableBuilderClassSettings())
                .Chain(x => x.Methods.RemoveAll(x => x.Name.ToString() != "Build"))
                .Build(),
            cls.ToBuilderExtensionsClass(new ImmutableBuilderClassSettings())
        };
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
    public class MyRecordBuilder
    {
        public string Property1
        {
            get;
            set;
        }

        public bool Property2
        {
            get;
            set;
        }

        public MyNamespace.MyRecord Build()
        {
            return new MyNamespace.MyRecord(Property1, Property2);
        }

        public MyRecordBuilder()
        {
            Property1 = string.Empty;
            Property2 = default;
        }
    }

    public static partial class MyRecordBuilderExtensions
    {
        public static MyRecordBuilder WithProperty1(this MyRecordBuilder instance, string property1)
        {
            instance.Property1 = property1;
            return instance;
        }

        public static MyRecordBuilder WithProperty2(this MyRecordBuilder instance, bool property2)
        {
            instance.Property2 = property2;
            return instance;
        }
    }
}
");
    }

    [Fact]
    public void GeneratesPocoClass_Model()
    {
        // Arrange
        var sut = new CSharpClassGenerator();
        var properties = new[]
        {
            new ClassPropertyBuilder().WithName("Property1").WithType(typeof(string)),
            new ClassPropertyBuilder().WithName("Property2").WithType(typeof(bool))
        };

        var model = new[]
        {
            new ClassBuilder()
                .WithName("MyPoco")
                .WithNamespace("MyNamespace")
                .AddProperties(properties)
                .Build()
                .ToPocoClass()
        };

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(PocoClassCode);
    }

    [Fact]
    public void GeneratesImmutableClassWithInjectedTemplates_Model_With_IEnumerable_Property()
    {
        // Arrange
        var properties = new[]
        {
            new ClassPropertyBuilder().WithName("Property1").WithType(typeof(string)),
            new ClassPropertyBuilder().WithName("Property2").WithType(typeof(bool)),
            new ClassPropertyBuilder().WithName("Property3").WithType(typeof(IEnumerable<string>))
        };

        var model = new[]
        {
            new ClassBuilder()
                .WithName("MyRecord")
                .WithNamespace("MyNamespace")
                .AddProperties(properties)
                .Build()
                .ToImmutableClass(new ImmutableClassSettings(createWithMethod: true))
        };
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(ImmutableClassCodeWithIEnumerable);
    }

    [Fact]
    public void GeneratesImmutableClassWithInjectedTemplates_Model_With_ICollection_Property()
    {
        // Arrange
        var properties = new[]
        {
            new ClassPropertyBuilder().WithName("Property1").WithType(typeof(string)),
            new ClassPropertyBuilder().WithName("Property2").WithType(typeof(bool)),
            new ClassPropertyBuilder().WithName("Property3").WithType(typeof(ICollection<string>))
        };

        var model = new[]
        {
            new ClassBuilder()
                .WithName("MyRecord")
                .WithNamespace("MyNamespace")
                .AddProperties(properties)
                .Build()
                .ToImmutableClass(new ImmutableClassSettings(createWithMethod: true))
        };
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(ImmutableClassCodeWithCollection);
    }

    [Fact]
    public void GeneratesImmutableClassWithInjectedTemplates_Model_With_Collection_Property()
    {
        // Arrange
        var properties = new[]
        {
            new ClassPropertyBuilder().WithName("Property1").WithType(typeof(string)),
            new ClassPropertyBuilder().WithName("Property2").WithType(typeof(bool)),
            new ClassPropertyBuilder().WithName("Property3").WithType(typeof(Collection<string>))
        };

        var model = new[]
        {
            new ClassBuilder()
                .WithName("MyRecord")
                .WithNamespace("MyNamespace")
                .AddProperties(properties)
                .Build()
                .ToImmutableClass(new ImmutableClassSettings(createWithMethod: true))
        };
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(ImmutableClassCodeWithCollection);
    }

    [Fact]
    public void GeneratesImmutableClassWithInjectedTemplates_Model_With_IList_Property()
    {
        // Arrange
        var properties = new[]
        {
            new ClassPropertyBuilder().WithName("Property1").WithType(typeof(string)),
            new ClassPropertyBuilder().WithName("Property2").WithType(typeof(bool)),
            new ClassPropertyBuilder().WithName("Property3").WithType(typeof(IList<string>))
        };

        var model = new[]
        {
                new ClassBuilder()
                    .WithName("MyRecord")
                    .WithNamespace("MyNamespace")
                    .AddProperties(properties)
                    .Build()
                    .ToImmutableClass(new ImmutableClassSettings(createWithMethod: true))
            };
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(ImmutableClassCodeWithCollection);
    }

    [Fact]
    public void GeneratesImmutableClassWithInjectedTemplates_Model_With_List_Property()
    {
        // Arrange
        var properties = new[]
        {
            new ClassPropertyBuilder().WithName("Property1").WithType(typeof(string)),
            new ClassPropertyBuilder().WithName("Property2").WithType(typeof(bool)),
            new ClassPropertyBuilder().WithName("Property3").WithType(typeof(List<string>))
        };

        var model = new[]
        {
            new ClassBuilder()
                .WithName("MyRecord")
                .WithNamespace("MyNamespace")
                .AddProperties(properties)
                .Build()
                .ToImmutableClass(new ImmutableClassSettings(createWithMethod: true))
        };
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(ImmutableClassCodeWithCollection);
    }

    [Fact]
    public void GeneratesPocoClass_Model_With_Collection_Property()
    {
        // Arrange
        var properties = new[]
        {
            new ClassPropertyBuilder().WithName("Property1").WithType(typeof(string)),
            new ClassPropertyBuilder().WithName("Property2").WithType(typeof(bool)),
            new ClassPropertyBuilder().WithName("Property3").WithType(typeof(ICollection<string>))
        };

        var model = new[]
        {
                new ClassBuilder()
                    .WithName("MyPoco")
                    .WithNamespace("MyNamespace")
                    .AddProperties(properties)
                    .Build()
                    .ToPocoClass()
            };
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
    public class MyPoco
    {
        public string Property1
        {
            get;
            set;
        }

        public bool Property2
        {
            get;
            set;
        }

        public System.Collections.Generic.ICollection<string> Property3
        {
            get;
            set;
        }
    }
}
");
    }

    [Fact]
    public void GeneratesObservableClass()
    {
        // Arrange
        var properties = new[]
        {
            new ClassPropertyBuilder().WithName("Property1").WithType(typeof(string)),
            new ClassPropertyBuilder().WithName("Property2").WithType(typeof(bool)),
            new ClassPropertyBuilder().WithName("Property3").WithType(typeof(IEnumerable<string>))
        };

        var model = new[]
        {
            new ClassBuilder()
                .WithName("MyRecordBuilder")
                .WithNamespace("MyNamespace")
                .AddProperties(properties)
                .Build()
                .ToObservableClass()
        };
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
    public class MyRecordBuilder
    {
        public string Property1
        {
            get
            {
                return _property1;
            }
            set
            {
                _property1 = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(""Property1""));
            }
        }

        public bool Property2
        {
            get
            {
                return _property2;
            }
            set
            {
                _property2 = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(""Property2""));
            }
        }

        public System.Collections.ObjectModel.ObservableCollection<string> Property3
        {
            get
            {
                return _property3;
            }
            set
            {
                _property3 = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(""Property3""));
            }
        }

        public MyRecordBuilder()
        {
            this.Property3 = new System.Collections.ObjectModel.ObservableCollection<string>();
        }

        private string _property1;

        private bool _property2;

        private System.Collections.Generic.IEnumerable<string> _property3;
    }
}
");
    }

    [Fact]
    public void GeneratesImmutableClassWithInjectedTemplates_Reflection()
    {
        // Arrange
        var model = new[]
        {
            new
            {
                Property1 = "Hello",
                Property2 = false
            }.GetType().ToClassBuilder()
                .WithName("MyRecord")
                .WithNamespace("MyNamespace")
                .Build()
                .ToImmutableClassBuilder(new ImmutableClassSettings())
                .Chain(x => x.Attributes.Clear()) // needed to exclude compiler generated attributes, which are not included in the expectation (shared with another test)
                .Build()
        };
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(ImmutableClassCodeNoWithMethod);
    }

    [Fact]
    public void GeneratesImmutableBuilderClassForRecord()
    {
        // Arrange
        var input = typeof(Person).ToClass(new ClassSettings(createConstructors: true));
        var settings = new ImmutableBuilderClassSettings(constructorSettings: new ImmutableBuilderClassConstructorSettings(addCopyConstructor: true));

        // Act
        var builder = input.ToImmutableBuilderClass(settings);
        var sut = new CSharpClassGenerator();
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, new[] { builder });

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelFramework.Generators.Objects.Tests
{
    public class PersonBuilder
    {
        public string FirstName
        {
            get;
            set;
        }

        public string LastName
        {
            get;
            set;
        }

        public ModelFramework.Generators.Objects.Tests.Person Build()
        {
            return new ModelFramework.Generators.Objects.Tests.Person { FirstName = FirstName, LastName = LastName };
        }

        public PersonBuilder WithFirstName(string firstName)
        {
            FirstName = firstName;
            return this;
        }

        public PersonBuilder WithLastName(string lastName)
        {
            LastName = lastName;
            return this;
        }

        public PersonBuilder()
        {
        }

        public PersonBuilder(ModelFramework.Generators.Objects.Tests.Person source)
        {
            FirstName = source.FirstName;
            LastName = source.LastName;
        }
    }
}
");
    }

    [Fact]
    public void Can_Generate_Class_With_Nullable_Reference_Types()
    {
        // Arrange
        var model = new[]
        {
            new ClassBuilder()
            .WithName("MyClass")
            .WithNamespace("MyNamespace")
            .AddProperties(new ClassPropertyBuilder().WithName("Property").WithType(typeof(string)).WithIsNullable())
            .Build()
        };
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model, additionalParameters: new { EnableNullableContext = true, CreateCodeGenerationHeader = true, EnvironmentVersion = "1.0" });

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 1.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
#nullable enable
    public class MyClass
    {
        public string? Property
        {
            get;
            set;
        }
    }
#nullable restore
}
");
    }

    [Fact]
    public void Can_Generate_ImmutableClass_From_Interface_Without_Coupling()
    {
        // Arrange
        var settings = new ImmutableClassSettings("CrossCutting.ReadOnlyValueCollection", constructorSettings: new ImmutableClassConstructorSettings(validateArguments: ArgumentValidationType.DomainOnly, addNullChecks: true));
        var model = new[]
        {
            new InterfaceBuilder()
                .WithName("IMyClass")
                .WithNamespace("MyNamespace")
                .AddProperties
                (
                    new ClassPropertyBuilder().WithName("Property1").WithType(typeof(string)),
                    new ClassPropertyBuilder().WithName("Property2").WithTypeName("MyNamespace.IClass")
                )
                .Build()
        }
        // Custom code for GetImmutableClasses method
        .Select
        (
            x => new ClassBuilder(x.ToClass())
                .WithName(x.Name.Substring(1))
                .WithNamespace("EntitiesNamespace")
                // here is where we normally .Chain(y => FixImmutableBuilderProperties(y))
                .Build()
                .ToImmutableClassBuilder(settings)
                .WithRecord()
                .WithPartial()
                // here is where we normally .AddInterfaces($"{x.Namespace}.{x.Name}")
                .Build()
        )
        .ToArray();
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntitiesNamespace
{
    public partial record MyClass
    {
        public string Property1
        {
            get;
        }

        public MyNamespace.IClass Property2
        {
            get;
        }

        public MyClass(string property1, MyNamespace.IClass property2)
        {
            if (property1 == null) throw new System.ArgumentNullException(""property1"");
            if (property2 == null) throw new System.ArgumentNullException(""property2"");
            this.Property1 = property1;
            this.Property2 = property2;
            System.ComponentModel.DataAnnotations.Validator.ValidateObject(this, new System.ComponentModel.DataAnnotations.ValidationContext(this, null, null), true);
        }
    }
}
");
    }

    [Fact]
    public void Can_Generate_ImmutableClass_With_Private_Property_Setters()
    {
        // Arrange
        var properties = new[]
        {
            new ClassPropertyBuilder().WithName("Property1").WithType(typeof(string)).WithIsNullable()
        };
        var cls = new ClassBuilder()
            .WithName("MyRecord")
            .WithNamespace("MyNamespace")
            .AddProperties(properties)
            .Build()
            .ToImmutableClassBuilder(new ImmutableClassSettings(newCollectionTypeName: "System.Collections.Generic.IReadOnlyCollection", addPrivateSetters: true))
            .WithRecord()
            .Build();
        var model = new[]
        {
            cls
        };
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model, additionalParameters: new { EnableNullableContext = true });

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
    public record MyRecord
    {
        public string? Property1
        {
            get;
            private set;
        }

        public MyRecord(string? property1)
        {
            this.Property1 = property1;
        }
    }
}
");
    }

    [Fact]
    public void Can_Generate_ImmutableClass_With_Shared_Validation()
    {
        // Arrange
        var properties = new[]
        {
            new ClassPropertyBuilder().WithName("Property1").WithType(typeof(string)).WithIsNullable()
        };
        var x = new ClassBuilder()
            .WithName("MyRecord")
            .WithNamespace("MyNamespace")
            .AddProperties(properties)
            .AsReadOnly()
            .Build();
        var baseClass = x
            .ToImmutableClassBuilder(new(newCollectionTypeName: "System.Collections.Generic.IReadOnlyCollection", addPrivateSetters: true, constructorSettings: new(ArgumentValidationType.Shared)))
            .WithRecord()
            .Build();
        var inheritedClass = x
            .ToImmutableClassValidateOverrideBuilder(new(newCollectionTypeName: "System.Collections.Generic.IReadOnlyCollection", addPrivateSetters: true, constructorSettings: new(ArgumentValidationType.Shared)))
            .WithRecord()
            .Build();
        var model = new[]
        {
            inheritedClass,
            baseClass,
        };
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model, additionalParameters: new { EnableNullableContext = true });

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
    public record MyRecord : MyRecordBase
    {
        public MyRecord(MyRecord original) : base(original)
        {
        }

        public MyRecord(string? property1) : base(property1)
        {
            System.ComponentModel.DataAnnotations.Validator.ValidateObject(this, new System.ComponentModel.DataAnnotations.ValidationContext(this, null, null), true);
        }
    }

    public record MyRecordBase
    {
        public string? Property1
        {
            get;
            private set;
        }

        public MyRecordBase(string? property1)
        {
            this.Property1 = property1;
        }
    }
}
");
    }

    [Fact]
    public void Can_Generate_ImmutableClass_With_NonShared_Validation()
    {
        // Arrange
        var properties = new[]
        {
            new ClassPropertyBuilder().WithName("Property1").WithType(typeof(string)).WithIsNullable()
        };
        var @class = new ClassBuilder()
            .WithName("MyRecord")
            .WithNamespace("MyNamespace")
            .AddProperties(properties)
            .AsReadOnly()
            .Build()
            .ToImmutableClassBuilder(new(newCollectionTypeName: "System.Collections.Generic.IReadOnlyCollection", addPrivateSetters: true, constructorSettings: new(ArgumentValidationType.DomainOnly)))
            .WithRecord()
            .WithPartial()
            .Build();
        var model = new[]
        {
            @class,
        };
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model, additionalParameters: new { EnableNullableContext = true });

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
    public partial record MyRecord
    {
        public string? Property1
        {
            get;
            private set;
        }

        public MyRecord(string? property1)
        {
            this.Property1 = property1;
            System.ComponentModel.DataAnnotations.Validator.ValidateObject(this, new System.ComponentModel.DataAnnotations.ValidationContext(this, null, null), true);
        }
    }
}
");
    }

    [Fact]
    public void Can_Generate_ImmutableClassBuilder_With_Shared_Validation()
    {
        // Arrange
        var properties = new[]
        {
            new ClassPropertyBuilder().WithName("Property1").WithType(typeof(string)).WithIsNullable()
        };
        var cls = new ClassBuilder()
            .WithName("MyRecord")
            .WithNamespace("MyNamespace")
            .AddProperties(properties)
            .AsReadOnly()
            .Build()
            .ToImmutableBuilderClassBuilder(new(
                constructorSettings: new(addCopyConstructor: true, addNullChecks: true),
                classSettings: new(newCollectionTypeName: "System.Collections.Generic.IReadOnlyCollection", addPrivateSetters: true, constructorSettings: new(ArgumentValidationType.Shared))))
            .Build();
        var model = new[]
        {
            cls
        };
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model, additionalParameters: new { EnableNullableContext = true });

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
    public class MyRecordBuilder : System.ComponentModel.DataAnnotations.IValidatableObject
    {
        public string? Property1
        {
            get;
            set;
        }

        public MyNamespace.MyRecord Build()
        {
            return new MyNamespace.MyRecord(Property1);
        }

        public System.Collections.Generic.IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> Validate(System.ComponentModel.DataAnnotations.ValidationContext validationContext)
        {
            var instance = new MyNamespace.MyRecordBase(Property1);
            var results = new System.Collections.Generic.List<System.ComponentModel.DataAnnotations.ValidationResult>();
            System.ComponentModel.DataAnnotations.Validator.TryValidateObject(instance, new System.ComponentModel.DataAnnotations.ValidationContext(instance, null, null), results, true);
            return results;
        }

        public MyRecordBuilder WithProperty1(string? property1)
        {
            Property1 = property1;
            return this;
        }

        public MyRecordBuilder()
        {
        }

        public MyRecordBuilder(MyNamespace.MyRecord source)
        {
            if (source == null)
            {
                throw new System.ArgumentNullException(""source"");
            }
            Property1 = source.Property1;
        }
    }
}
");
    }

    [Fact]
    public void Can_Generate_ImmutableClassBuilder_With_NonShared_Validation()
    {
        // Arrange
        var properties = new[]
        {
            new ClassPropertyBuilder().WithName("Property1").WithType(typeof(string)).WithIsNullable()
        };
        var cls = new ClassBuilder()
            .WithName("MyRecord")
            .WithNamespace("MyNamespace")
            .AddProperties(properties)
            .AsReadOnly()
            .Build()
            .ToImmutableBuilderClassBuilder(new(
                constructorSettings: new(addCopyConstructor: true, addNullChecks: true),
                classSettings: new(newCollectionTypeName: "System.Collections.Generic.IReadOnlyCollection", addPrivateSetters: true, constructorSettings: new(ArgumentValidationType.DomainOnly))))
            .Build();
        var model = new[]
        {
            cls
        };
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model, additionalParameters: new { EnableNullableContext = true });

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
    public class MyRecordBuilder
    {
        public string? Property1
        {
            get;
            set;
        }

        public MyNamespace.MyRecord Build()
        {
            return new MyNamespace.MyRecord(Property1);
        }

        public MyRecordBuilder WithProperty1(string? property1)
        {
            Property1 = property1;
            return this;
        }

        public MyRecordBuilder()
        {
        }

        public MyRecordBuilder(MyNamespace.MyRecord source)
        {
            if (source == null)
            {
                throw new System.ArgumentNullException(""source"");
            }
            Property1 = source.Property1;
        }
    }
}
");
    }

    [Fact]
    public void Can_Generate_ImmutableClassBuilder_With_Shared_Validation_On_Class_Without_Properties()
    {
        // Arrange
        var cls = new ClassBuilder()
            .WithName("MyRecord")
            .WithNamespace("MyNamespace")
            .AddConstructors(new ClassConstructorBuilder().AddParameters(new ParameterBuilder().WithName("validateInstance").WithType(typeof(bool)).WithDefaultValue(true)))
            .Build()
            .ToImmutableBuilderClassBuilder(new(
                generationSettings: new(allowGenerationWithoutProperties: true),
                constructorSettings: new(addCopyConstructor: true, addNullChecks: true),
                classSettings: new(newCollectionTypeName: "System.Collections.Generic.IReadOnlyCollection", addPrivateSetters: true, constructorSettings: new(ArgumentValidationType.Shared))))
            .Build();
        var model = new[]
        {
            cls
        };
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model, additionalParameters: new { EnableNullableContext = true });

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
    public class MyRecordBuilder : System.ComponentModel.DataAnnotations.IValidatableObject
    {
        public MyNamespace.MyRecord Build()
        {
            return new MyNamespace.MyRecord();
        }

        public System.Collections.Generic.IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> Validate(System.ComponentModel.DataAnnotations.ValidationContext validationContext)
        {
            var instance = new MyNamespace.MyRecordBase();
            var results = new System.Collections.Generic.List<System.ComponentModel.DataAnnotations.ValidationResult>();
            System.ComponentModel.DataAnnotations.Validator.TryValidateObject(instance, new System.ComponentModel.DataAnnotations.ValidationContext(instance, null, null), results, true);
            return results;
        }

        public MyRecordBuilder()
        {
        }

        public MyRecordBuilder(MyNamespace.MyRecord source)
        {
            if (source == null)
            {
                throw new System.ArgumentNullException(""source"");
            }
        }
    }
}
");
    }

    [Fact]
    public void Can_Generate_ImmutableClassBuilder_With_Shared_Validation_With_NullableContext()
    {
        // Arrange
        var properties = new[]
        {
            new ClassPropertyBuilder().WithName("Property1").WithType(typeof(string)).WithIsNullable()
        };
        var cls = new ClassBuilder()
            .WithName("MyRecord")
            .WithNamespace("MyNamespace")
            .AddProperties(properties)
            .AsReadOnly()
            .Build()
            .ToImmutableBuilderClassBuilder(new(
                typeSettings: new(enableNullableReferenceTypes: true),
                constructorSettings: new(addCopyConstructor: true, addNullChecks: true),
                classSettings: new(newCollectionTypeName: "System.Collections.Generic.IReadOnlyCollection", addPrivateSetters: true, constructorSettings: new(ArgumentValidationType.Shared))))
            .Build();
        var model = new[]
        {
            cls
        };
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model, additionalParameters: new { EnableNullableContext = true });

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
    public class MyRecordBuilder : System.ComponentModel.DataAnnotations.IValidatableObject
    {
        public string? Property1
        {
            get;
            set;
        }

        public MyNamespace.MyRecord Build()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            return new MyNamespace.MyRecord(Property1);
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public System.Collections.Generic.IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> Validate(System.ComponentModel.DataAnnotations.ValidationContext validationContext)
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            var instance = new MyNamespace.MyRecordBase(Property1);
            #pragma warning restore CS8604 // Possible null reference argument.
            var results = new System.Collections.Generic.List<System.ComponentModel.DataAnnotations.ValidationResult>();
            System.ComponentModel.DataAnnotations.Validator.TryValidateObject(instance, new System.ComponentModel.DataAnnotations.ValidationContext(instance, null, null), results, true);
            return results;
        }

        public MyRecordBuilder WithProperty1(string? property1)
        {
            Property1 = property1;
            return this;
        }

        public MyRecordBuilder()
        {
            #pragma warning disable CS8603 // Possible null reference return.
            #pragma warning restore CS8603 // Possible null reference return.
        }

        public MyRecordBuilder(MyNamespace.MyRecord source)
        {
            if (source == null)
            {
                throw new System.ArgumentNullException(""source"");
            }
            Property1 = source.Property1;
        }
    }
}
");
    }

    private static IEnumerable<ClassBuilder> GetSubClasses()
    {
        yield return new ClassBuilder()
            .WithName("MySubClass")
            .WithNamespace("MyNamespace")
            .AddFields(GetFields())
            .AddProperties(GetProperties())
            .AddSubClasses
            (
                new ClassBuilder()
                    .WithName("MySubSubClass")
                    .WithNamespace("MyNamespace")
                    .AddSubClasses(new ClassBuilder().WithName("MySubSubSubClass").WithNamespace("MyNamespace"))
            );
    }

    private static IEnumerable<EnumBuilder> GetEnums()
    {
        yield return new EnumBuilder()
            .WithName("MyEnum")
            .AddMembers
            (
                new EnumMemberBuilder().WithName("Member1").WithValue(1),
                new EnumMemberBuilder().WithName("Member2").WithValue(2)
            );
    }

    private static IEnumerable<ClassConstructorBuilder> GetConstructors()
    {
        yield return new ClassConstructorBuilder()
            .AddParameters
            (
                new ParameterBuilder().WithName("Parameter1").WithType(typeof(string)),
                new ParameterBuilder().WithName("Parameter2").WithType(typeof(int))
            ).AddCodeStatements
            (
                new LiteralCodeStatementBuilder("throw new NotImplementedException();")
            );
    }

    private static IEnumerable<ClassMethodBuilder> GetMethods()
    {
        yield return new ClassMethodBuilder()
            .WithName("Method1")
            .AddParameters
            (
                new ParameterBuilder().WithName("Parameter1").WithType(typeof(string)),
                new ParameterBuilder().WithName("Parameter2").WithType(typeof(int))
            ).AddCodeStatements(new LiteralCodeStatementBuilder("throw new NotImplementedException();"));
    }

    private static IEnumerable<ClassPropertyBuilder> GetProperties()
    {
        yield return new ClassPropertyBuilder().WithName("MyProperty1").WithType(typeof(string));
        yield return new ClassPropertyBuilder().WithName("MyProperty2").WithType(typeof(int)).AddAttributes(new AttributeBuilder().WithName("MyAttribute"));
    }

    private static IEnumerable<ClassFieldBuilder> GetFields()
    {
        yield return new ClassFieldBuilder().WithName("_myField1").WithType(typeof(string));
        yield return new ClassFieldBuilder().WithName("_myField2").WithType(typeof(string));
    }
}

public record Person
{
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
}
