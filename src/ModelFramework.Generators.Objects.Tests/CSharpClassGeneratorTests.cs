using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using FluentAssertions;
using ModelFramework.Common.Default;
using ModelFramework.Common.Extensions;
using ModelFramework.Generators.Objects.Tests.Mocks;
using ModelFramework.Objects.Builders;
using ModelFramework.Objects.CodeStatements;
using ModelFramework.Objects.Contracts;
using ModelFramework.Objects.Default;
using ModelFramework.Objects.Extensions;
using ModelFramework.Objects.Settings;
using TextTemplateTransformationFramework.Runtime;
using Xunit;

namespace ModelFramework.Generators.Objects.Tests
{
    [ExcludeFromCodeCoverage]
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
        private const string ImmutableClassCodeWithMethodExtensionMethod = @"using System;
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

        public MyRecord(string property1, bool property2, System.Collections.Immutable.IImmutableList<string> property3)
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

        public MyRecord(string property1, bool property2, System.Collections.Immutable.IImmutableList<string> property3)
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
            this.Property2 = new System.Collections.Generic.List<System.String>(property2 ?? new Enumerable.Empty<System.String>());
            this.Property3 = property3;
            this.Property4 = new System.Collections.Generic.List<MyCustomType>(property4 ?? new Enumerable.Empty<MyCustomType>());
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
            return new MyNamespace.MyRecord(Property1, Property2, Property3.Build(), Property4.Select(x => x.Build()));
        }

        public MyRecordBuilder Clear()
        {
            Property1 = default;
            Property2.Clear();
            Property3 = default;
            Property4.Clear();
            return this;
        }

        public MyRecordBuilder Update(MyNamespace.MyRecord source)
        {
            Property2 = new System.Collections.Generic.List<string>();
            Property4 = new System.Collections.Generic.List<MyCustomTypeBuilder>();
            Property1 = source.Property1;
            if (source.Property2 != null) foreach (var x in source.Property2) Property2.Add(x);
            Property3 = new MyCustomTypeBuilder(source.Property3);
            if (source.Property4 != null) Property4.AddRange(source.Property4.Select(x => new MyCustomTypeBuilder(x)));
            return this;
        }

        public MyRecordBuilder WithProperty1(string property1)
        {
            Property1 = property1;
            return this;
        }

        public MyRecordBuilder ClearProperty2()
        {
            Property2.Clear();
            return this;
        }

        public MyRecordBuilder AddProperty2(System.Collections.Generic.IEnumerable<string> property2)
        {
            return AddProperty2(property2.ToArray());
        }

        public MyRecordBuilder AddProperty2(params System.Collections.Generic.IReadOnlyCollection<string>[] property2)
        {
            if (property2 != null)
            {
                foreach(var itemToAdd in property2)
                {
                    Property2.Add(itemToAdd);
                }
            }
            return this;
        }

        public MyRecordBuilder WithProperty3(MyCustomTypeBuilder property3)
        {
            Property3 = property3;
            return this;
        }

        public MyRecordBuilder WithProperty3(MyCustomType property3)
        {
            property3 = new MyCustomTypeBuilder(property3);
            return this;
        }

        public MyRecordBuilder ClearProperty4()
        {
            Property4.Clear();
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
                foreach(var itemToAdd in property4)
                {
                    Property4.Add(itemToAdd);
                }
            }
            return this;
        }

        public MyRecordBuilder AddProperty4(System.Collections.Generic.IEnumerable<MyCustomType> property4)
        {
            return AddProperty4(property4.ToArray());
        }

        public MyRecordBuilder AddProperty4(params System.Collections.Generic.IReadOnlyCollection<MyCustomType>[] property4)
        {
            if (property4 != null)
            {
                foreach(var itemToAdd in property4)
                {
                    property4.Add(new MyCustomTypeBuilder(itemToAdd));
                }
            }
            return this;
        }

        public MyRecordBuilder()
        {
            Property2 = new System.Collections.Generic.List<string>();
            Property4 = new System.Collections.Generic.List<MyCustomTypeBuilder>();
        }

        public MyRecordBuilder(MyNamespace.MyRecord source)
        {
            Property2 = new System.Collections.Generic.List<string>();
            Property4 = new System.Collections.Generic.List<MyCustomTypeBuilder>();
            Property1 = source.Property1;
            if (source.Property2 != null) foreach (var x in source.Property2) Property2.Add(x);
            Property3 = new MyCustomTypeBuilder(source.Property3);
            if (source.Property4 != null) Property4.AddRange(source.Property4.Select(x => new MyCustomTypeBuilder(x)));
        }

        public MyRecordBuilder(string property1, System.Collections.Generic.IEnumerable<string> property2, MyCustomType property3, System.Collections.Generic.IEnumerable<MyCustomType> property4)
        {
            Property2 = new System.Collections.Generic.List<string>();
            Property4 = new System.Collections.Generic.List<MyCustomTypeBuilder>();
            Property1 = property1;
            if (property2 != null) foreach (var x in property2) Property2.Add(x);
            Property3 = property3;
            if (property4 != null) foreach (var x in property4) Property4.Add(x);
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
            this.Property2 = new System.Collections.Generic.List<System.String>(property2 ?? new Enumerable.Empty<System.String>());
            this.Property3 = property3;
            this.Property4 = new System.Collections.Generic.List<MyCustomType>(property4 ?? new Enumerable.Empty<MyCustomType>());
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
            return new MyNamespace.MyRecord(Property1, Property2, Property3.Build(), Property4.Select(x => x.Build()));
        }

        public MyRecordBuilder Clear()
        {
            Property1 = default;
            Property2.Clear();
            Property3 = default;
            Property4.Clear();
            return this;
        }

        public MyRecordBuilder Update(MyNamespace.MyRecord source)
        {
            Property2 = new System.Collections.Generic.List<string>();
            Property4 = new System.Collections.Generic.List<MyCustomTypeBuilder>();
            Property1 = source.Property1;
            foreach (var x in source.Property2) Property2.Add(x);
            Property3 = new MyCustomTypeBuilder(source.Property3);
            if (source.Property4 != null) Property4.AddRange(source.Property4.Select(x => new MyCustomTypeBuilder(x)));
            return this;
        }

        public MyRecordBuilder WithProperty1(string property1)
        {
            Property1 = property1;
            return this;
        }

        public MyRecordBuilder ClearProperty2()
        {
            Property2.Clear();
            return this;
        }

        public MyRecordBuilder AddProperty2(System.Collections.Generic.IEnumerable<string> property2)
        {
            return AddProperty2(property2.ToArray());
        }

        public MyRecordBuilder AddProperty2(params System.Collections.Generic.IReadOnlyCollection<string>[] property2)
        {
            foreach(var itemToAdd in property2)
            {
                Property2.Add(itemToAdd);
            }
            return this;
        }

        public MyRecordBuilder WithProperty3(MyCustomTypeBuilder property3)
        {
            Property3 = property3;
            return this;
        }

        public MyRecordBuilder WithProperty3(MyCustomType property3)
        {
            property3 = new MyCustomTypeBuilder(property3);
            return this;
        }

        public MyRecordBuilder ClearProperty4()
        {
            Property4.Clear();
            return this;
        }

        public MyRecordBuilder AddProperty4(System.Collections.Generic.IEnumerable<MyCustomTypeBuilder> property4)
        {
            return AddProperty4(property4.ToArray());
        }

        public MyRecordBuilder AddProperty4(params MyCustomTypeBuilder[] property4)
        {
            foreach(var itemToAdd in property4)
            {
                Property4.Add(itemToAdd);
            }
            return this;
        }

        public MyRecordBuilder AddProperty4(System.Collections.Generic.IEnumerable<MyCustomType> property4)
        {
            return AddProperty4(property4.ToArray());
        }

        public MyRecordBuilder AddProperty4(params System.Collections.Generic.IReadOnlyCollection<MyCustomType>[] property4)
        {
            foreach(var itemToAdd in property4)
            {
                    property4.Add(new MyCustomTypeBuilder(itemToAdd));
            }
            return this;
        }

        public MyRecordBuilder()
        {
            Property2 = new System.Collections.Generic.List<string>();
            Property4 = new System.Collections.Generic.List<MyCustomTypeBuilder>();
        }

        public MyRecordBuilder(MyNamespace.MyRecord source)
        {
            Property2 = new System.Collections.Generic.List<string>();
            Property4 = new System.Collections.Generic.List<MyCustomTypeBuilder>();
            Property1 = source.Property1;
            foreach (var x in source.Property2) Property2.Add(x);
            Property3 = new MyCustomTypeBuilder(source.Property3);
            if (source.Property4 != null) Property4.AddRange(source.Property4.Select(x => new MyCustomTypeBuilder(x)));
        }

        public MyRecordBuilder(string property1, System.Collections.Generic.IEnumerable<string> property2, MyCustomType property3, System.Collections.Generic.IEnumerable<MyCustomType> property4)
        {
            Property2 = new System.Collections.Generic.List<string>();
            Property4 = new System.Collections.Generic.List<MyCustomTypeBuilder>();
            Property1 = property1;
            foreach (var x in property2) Property2.Add(x);
            Property3 = property3;
            foreach (var x in property4) Property4.Add(x);
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
                new Class
                (
                    "MyClass",
                    "MyNamespace",
                    fields: GetFields(),
                    properties: GetProperties(),
                    methods: GetMethods(),
                    constructors: GetConstructors(),
                    enums: GetEnums(),
                    subClasses: GetSubClasses(),
                    attributes: new[]
                    {
                        new Attribute
                        (
                            "MyAttribute", new[]
                            {
                                new AttributeParameter(1, "Name1"), new AttributeParameter(2, "Name2")
                            }
                        )
                    }
                )
            };
            var sut = new CSharpClassGenerator();

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"using System;
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
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, null);

            // Assert
            actual.Should().Be(@"using System;
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
            actual.Should().Be(@"using System;
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
            actual.Should().Be(@"using System;
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
            actual.Should().Be(@"using System;
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
        public void GeneratesClassWithAutoGeneratedInterface()
        {
            // Arrange
            var model = new[]
            {
                new ClassBuilder()
                    .WithName("MyClass")
                    .WithNamespace("MyNamespace")
                    .AddMethods(new ClassMethod("DoSomething", "System.Boolean"))
                    .WithAutoGenerateInterface(true).Build()
            };
            var sut = new CSharpClassGenerator();

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
    public interface IMyClass
    {
        bool DoSomething();
    }

    public class MyClass : IMyClass
    {
        public bool DoSomething()
        {
        }
    }
}
");
        }

        [Fact]
        public void GeneratesClassWithAutoGeneratedInterfaceWhenClassInheritsInterfaces()
        {
            // Arrange
            var model = new[]
            {
                new ClassBuilder()
                    .WithName("MyClass")
                    .WithNamespace("MyNamespace")
                    .AddMethods(new ClassMethod("DoSomething", "System.Boolean"))
                    .AddInterfaces("IInterface1", "IInterface2")
                    .WithAutoGenerateInterface(true)
                    .Build()
            };
            var sut = new CSharpClassGenerator();

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
    public interface IMyClass : IInterface1, IInterface2
    {
        bool DoSomething();
    }

    public class MyClass : IMyClass
    {
        public bool DoSomething()
        {
        }
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
                new InterfaceBuilder { Name = "IMyInterface", Namespace = "MyNamespace" }.Build()
            };
            var sut = new CSharpClassGenerator();

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"using System;
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
            actual.Should().Be(@"using System;
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
                new Class("MyRecord", "MyNamespace", record: true, properties: new[] { new ClassProperty("Property1", typeof(string).FullName, hasInit: true) })
            };
            var sut = new CSharpClassGenerator();

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"using System;
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
            var actual = input.ToClass(new ClassSettings());
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
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute]
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
                new Interface("IMyInterface", "MyNamespace", partial: true)
            };
            var sut = new CSharpClassGenerator();

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"using System;
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
            rootTemplate.TemplateContext.Model = new Class("ParentClass", "MyNamespace");
            var sut = new CSharpClassGenerator();
            var model = new[]
            {
                new Class("MySubClass", "MyNamespace")
            };
            sut.TemplateContext = TemplateInstanceContext.CreateRootContext(rootTemplate).CreateChildContext(sut, sut.Model);

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"    public class MySubClass
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
                new Class("MyClass", "MyNamespace", Visibility.Private)
            };
            var sut = new CSharpClassGenerator();

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"using System;
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
                new Class("MyClass", "MyNamespace", Visibility.Internal)
            };
            var sut = new CSharpClassGenerator();

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"using System;
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
                new Class("MyClass", "MyNamespace", @static: true)
            };
            var sut = new CSharpClassGenerator();

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"using System;
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
                new Class("MyClass", "MyNamespace", partial: true)
            };
            var sut = new CSharpClassGenerator();

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"using System;
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
                new Class("MyClass", "MyNamespace", @static: true, partial: true)
            };
            var sut = new CSharpClassGenerator();

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"using System;
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
                new Class("MyClass", "MyNamespace", @sealed: true)
            };
            var sut = new CSharpClassGenerator();

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"using System;
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
                            new Class
                            (
                                "MyClass",
                                "MyNamespace",
                                attributes: new[]
                                {
                                    new Attribute
                                    ("MyAttribute", metadata: new[]
                                        {
                                            new Metadata(ModelFramework.Common.MetadataNames.CustomTemplateName, "MyTemplate")
                                        }
                                    )
                                }
                            )
                        }
                    }
                }
            };
            var builder = new StringBuilder();
            sut.Initialize();
            sut.RegisterChildTemplate("MyTemplate", () => new DelegateTemplate { RenderDelegate = new System.Action<StringBuilder>(b => b.AppendLine("    [MyCustomTemplate]")) });

            // Act
            sut.Render(builder);
            var actual = builder.ToString();

            // Assert
            actual.Should().Be(@"using System;
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
                            new Class
                            (
                                "MyClass",
                                "MyNamespace",
                                properties: new[]
                                {
                                    new ClassProperty
                                    (
                                        "MyAttribute",
                                        "string",
                                        metadata: new[]
                                        {
                                            new Metadata(ModelFramework.Common.MetadataNames.CustomTemplateName, "MyTemplate")
                                        }
                                    )
                                }
                            )
                        }
                    }
                }
            };
            sut.Initialize();
            sut.RegisterChildTemplate("MyTemplate", () => new DelegateTemplate { RenderDelegate = new System.Action<StringBuilder>(b => b.AppendLine("        public string MyCustomTemplateGeneratedProperty { get; set; }")) });
            var builder = new StringBuilder();

            // Act
            sut.Render(builder);
            var actual = builder.ToString();

            // Assert
            actual.Should().Be(@"using System;
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
                            new Class
                            (
                                "MyClass",
                                "MyNamespace",
                                methods: new[]
                                {
                                    new ClassMethod
                                    (
                                        "MyMethod",
                                        "string",
                                        metadata: new[]
                                        {
                                            new Metadata(ModelFramework.Common.MetadataNames.CustomTemplateName, "MyTemplate")
                                        }
                                    )
                                }
                            )
                        }
                    }
                }
            };
            sut.Initialize();
            sut.RegisterChildTemplate("MyTemplate", () => new DelegateTemplate { RenderDelegate = new System.Action<StringBuilder>(b => b.AppendLine("        public string MyCustomTemplateGeneratedMethod() { throw new NotImplementedException; }")) });
            var builder = new StringBuilder();

            // Act
            sut.Render(builder);
            var actual = builder.ToString();

            // Assert
            actual.Should().Be(@"using System;
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
                            new Class
                            (
                                "MyClass",
                                "MyNamespace",
                                constructors: new[]
                                {
                                    new ClassConstructor
                                    (
                                        metadata: new[]
                                        {
                                            new Metadata(ModelFramework.Common.MetadataNames.CustomTemplateName, "MyTemplate")
                                        }
                                    )
                                }
                            )
                        }
                    }
                }
            };
            sut.Initialize();
            sut.RegisterChildTemplate("MyTemplate", () => new DelegateTemplate { RenderDelegate = new System.Action<StringBuilder>(b => b.AppendLine("        public MyClass() { throw new NotImplementedException; }")) });
            var builder = new StringBuilder();

            // Act
            sut.Render(builder);
            var actual = builder.ToString();

            // Assert
            actual.Should().Be(@"using System;
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
                            new Class
                            (
                                "MyClass",
                                "MyNamespace",
                                fields: new[]
                                {
                                    new ClassField
                                    (
                                        "MyField",
                                        "string",
                                        metadata: new[]
                                        {
                                            new Metadata(ModelFramework.Common.MetadataNames.CustomTemplateName, "MyTemplate")
                                        }
                                    )
                                }
                            )
                        }
                    }
                }
            };
            sut.Initialize();
            sut.RegisterChildTemplate("MyTemplate", () => new DelegateTemplate { RenderDelegate = new System.Action<StringBuilder>(b => b.AppendLine("        public string myCustomFieldInjectedFromTemplate;")) });
            var builder = new StringBuilder();

            // Act
            sut.Render(builder);
            var actual = builder.ToString();

            // Assert
            actual.Should().Be(@"using System;
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
                            new Class
                            (
                                "MyClass",
                                "MyNamespace",
                                enums: new[]
                                {
                                    new Enum
                                    (
                                        "MyEnum",
                                        System.Array.Empty<IEnumMember>(),
                                        metadata: new[]
                                        {
                                            new Metadata(ModelFramework.Common.MetadataNames.CustomTemplateName, "MyTemplate")
                                        }
                                    )
                                }
                            )
                        }
                    }
                }
            };
            sut.Initialize();
            sut.RegisterChildTemplate("MyTemplate", () => new DelegateTemplate { RenderDelegate = new System.Action<StringBuilder>(b => b.AppendLine("        public enum MyEnum { One, Two, Three };")) });
            var builder = new StringBuilder();

            // Act
            sut.Render(builder);
            var actual = builder.ToString();

            // Assert
            actual.Should().Be(@"using System;
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
                new Class("MyClass2", "Namespace1"),
                new Class("MyClass1", "Namespace2"),
                new Class("MyClass2", "Namespace2"),
                new Class("MyClass1", "Namespace1"),
            };
            var sut = new CSharpClassGenerator();

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"using System;
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
                new Class("MyClass1", "MyNamespace"),
                new Class("MyClass2", "MyNamespace")
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
            actual.Should().Be(@"<?xml version=""1.0"" encoding=""utf-16""?>
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
                new ClassProperty("Property1", typeof(string).FullName),
                new ClassProperty("Property2", typeof(bool).FullName)
            };

            var model = new[]
            {
                new Class
                (
                    "MyRecord",
                    "MyNamespace",
                    properties: properties
                ).ToImmutableClass(new ImmutableClassSettings(createWithMethod: true))
            };
            var sut = new CSharpClassGenerator();

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(ImmutableClassCode);
        }

        [Fact]
        public void GeneratesImmutableClassWithIEquatable()
        {
            // Arrange
            var properties = new[]
            {
                new ClassProperty("Property1", typeof(string).FullName),
                new ClassProperty("Property2", typeof(bool).FullName)
            };

            var model = new[]
            {
                new Class
                (
                    "MyRecord",
                    "MyNamespace",
                    properties: properties
                ).ToImmutableClass(new ImmutableClassSettings(implementIEquatable: true))
            };
            var sut = new CSharpClassGenerator();

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"using System;
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
                new ClassProperty("Property1", typeof(string).FullName),
                new ClassProperty("Property2", typeof(bool).FullName)
            };

            var model = new[]
            {
                new Class
                (
                    "MyRecord",
                    "MyNamespace",
                    properties: properties
                ).ToImmutableClass(new ImmutableClassSettings(createWithMethod: false))
            };
            var sut = new CSharpClassGenerator();

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(ImmutableClassCodeNoWithMethod);
        }

        [Fact]
        public void GeneratesImmutableClassWithInjectedTemplates_Model_With_Method_Using_ExtensionMethod()
        {
            // Arrange
            var properties = new[]
            {
                new ClassProperty("Property1", typeof(string).FullName),
                new ClassProperty("Property2", typeof(bool).FullName)
            };

            var cls = new Class
            (
                "MyRecord",
                "MyNamespace",
                properties: properties
            );

            var model = new[]
            {
                cls.ToImmutableClass(new ImmutableClassSettings(createWithMethod: false)),
                cls.ToImmutableExtensionClass()
            };
            var sut = new CSharpClassGenerator();

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(ImmutableClassCodeWithMethodExtensionMethod);
        }

        [Fact]
        public void GeneratesImmutableBuilderClassWithNullChecks()
        {
            // Arrange
            var properties = new[]
            {
                new ClassPropertyBuilder { Name = "Property1", TypeName = typeof(string).FullName }.Build(),
                new ClassPropertyBuilder { Name = "Property2", TypeName = typeof(ICollection<string>).FullName }.ConvertCollectionToEnumerable().Build(),
                new ClassPropertyBuilder { Name = "Property3", TypeName = "MyCustomType" }.ConvertSinglePropertyToBuilder().Build(),
                new ClassPropertyBuilder { Name = "Property4", TypeName = typeof(ICollection<string>).FullName.Replace("System.String","MyCustomType") }.ConvertCollectionPropertyToBuilder().Build()
            };
            var cls = new ClassBuilder()
                .WithName("MyRecord")
                .WithNamespace("MyNamespace")
                .AddProperties(properties)
                .Build()
                .ToImmutableClass(new ImmutableClassSettings("System.Collections.Generic.IReadOnlyCollection"));
            var settings = new ImmutableBuilderClassSettings(addCopyConstructor: true, addNullChecks: true);
            var model = new[]
            {
                cls,
                cls.ToImmutableBuilderClass(settings)
            };
            var sut = new CSharpClassGenerator();

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(ImmutableBuilderClassCodeWithNullChecks);
        }

        [Fact]
        public void GeneratesImmutableBuilderClassWithoutNullChecks()
        {
            // Arrange
            var properties = new[]
            {
                new ClassPropertyBuilder { Name = "Property1", TypeName = typeof(string).FullName }.Build(),
                new ClassPropertyBuilder { Name = "Property2", TypeName = typeof(ICollection<string>).FullName }.ConvertCollectionToEnumerable().Build(),
                new ClassPropertyBuilder { Name = "Property3", TypeName = "MyCustomType" }.ConvertSinglePropertyToBuilder().Build(),
                new ClassPropertyBuilder { Name = "Property4", TypeName = typeof(ICollection<string>).FullName.Replace("System.String","MyCustomType") }.ConvertCollectionPropertyToBuilder().Build()
            };
            var cls = new ClassBuilder()
                .WithName("MyRecord")
                .WithNamespace("MyNamespace")
                .AddProperties(properties)
                .Build()
                .ToImmutableClass(new ImmutableClassSettings("System.Collections.Generic.IReadOnlyCollection"));
            var settings = new ImmutableBuilderClassSettings(addCopyConstructor: true, addNullChecks: false);
            var model = new[]
            {
                cls,
                cls.ToImmutableBuilderClass(settings)
            };
            var sut = new CSharpClassGenerator();

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(ImmutableBuilderClassCodeWithoutNullChecks);
        }

        [Fact]
        public void GeneratesImmutableBuilderClassWithReservedKeyword()
        {
            // Arrange
            var cls = new ClassBuilder()
                .WithName("MyRecord")
                .WithNamespace("MyNamespace")
                .AddProperties(new ClassPropertyBuilder { Name = "Static", TypeName = typeof(bool).FullName } )
                .Build()
                .ToImmutableClass(new ImmutableClassSettings());
            var settings = new ImmutableBuilderClassSettings(addCopyConstructor: true);
            var model = new[]
            {
                cls,
                cls.ToImmutableBuilderClass(settings)
            };
            var sut = new CSharpClassGenerator();

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"using System;
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

        public MyRecordBuilder Clear()
        {
            Static = default;
            return this;
        }

        public MyRecordBuilder Update(MyNamespace.MyRecord source)
        {
            Static = source.Static;
            return this;
        }

        public MyRecordBuilder WithStatic(bool @static)
        {
            Static = @static;
            return this;
        }

        public MyRecordBuilder()
        {
        }

        public MyRecordBuilder(MyNamespace.MyRecord source)
        {
            Static = source.Static;
        }

        public MyRecordBuilder(bool @static)
        {
            Static = @static;
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
                new ClassPropertyBuilder { Name = "Property1", TypeName = typeof(string).FullName }.Build(),
            };
            var cls = new ClassBuilder()
                .WithName("MyRecord")
                .WithNamespace("MyNamespace")
                .AddProperties(properties)
                .Build()
                .ToImmutableClass(new ImmutableClassSettings("System.Collections.Generic.IReadOnlyCollection"));
            var settings = new ImmutableBuilderClassSettings(setMethodNameFormatString: "Set{0}");
            var model = new[]
            {
                cls,
                cls.ToImmutableBuilderClass(settings)
            };
            var sut = new CSharpClassGenerator();

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"using System;
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

        public MyRecordBuilder Clear()
        {
            Property1 = default;
            return this;
        }

        public MyRecordBuilder SetProperty1(string property1)
        {
            Property1 = property1;
            return this;
        }

        public MyRecordBuilder()
        {
        }

        public MyRecordBuilder(MyNamespace.MyRecord source)
        {
            Property1 = source.Property1;
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
                new ClassPropertyBuilder { Name = "Property1", TypeName = typeof(string).FullName }.AddAttributes(new AttributeBuilder().WithName("MyAttribute")).Build(),
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
            actual.Should().Be(@"using System;
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

        public MyRecordBuilder Clear()
        {
            Property1 = default;
            return this;
        }

        public MyRecordBuilder WithProperty1(string property1)
        {
            Property1 = property1;
            return this;
        }

        public MyRecordBuilder()
        {
        }

        public MyRecordBuilder(MyNamespace.MyRecord source)
        {
            Property1 = source.Property1;
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
                new ClassProperty("Property1", typeof(string).FullName),
                new ClassProperty("Property2", typeof(bool).FullName)
            };

            var model = new[]
            {
                new Class
                (
                    "MyPoco",
                    "MyNamespace",
                    properties: properties
                ).ToPocoClass()
            };

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(PocoClassCode);
        }

        [Fact]
        public void GeneratesImmutableClassWithInjectedTemplates_Model_With_IEnumerable_Property()
        {
            // Arrange
            var properties = new[]
            {
                new ClassProperty("Property1", typeof(string).FullName),
                new ClassProperty("Property2", typeof(bool).FullName),
                new ClassProperty
                (
                    "Property3",
                    typeof(IEnumerable<string>).FullName
                )
            };

            var model = new[]
            {
                new Class
                (
                    "MyRecord",
                    "MyNamespace",
                    properties: properties
                ).ToImmutableClass(new ImmutableClassSettings(createWithMethod: true))
            };
            var sut = new CSharpClassGenerator();

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(ImmutableClassCodeWithIEnumerable);
        }

        [Fact]
        public void GeneratesImmutableClassWithInjectedTemplates_Model_With_ICollection_Property()
        {
            // Arrange
            var properties = new[]
            {
                new ClassProperty("Property1", typeof(string).FullName),
                new ClassProperty("Property2", typeof(bool).FullName),
                new ClassProperty
                (
                    "Property3",
                    typeof(ICollection<string>).FullName
                )
            };

            var model = new[]
            {
                new Class
                (
                    "MyRecord",
                    "MyNamespace",
                    properties: properties
                ).ToImmutableClass(new ImmutableClassSettings(createWithMethod: true))
            };
            var sut = new CSharpClassGenerator();

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(ImmutableClassCodeWithCollection);
        }

        [Fact]
        public void GeneratesImmutableClassWithInjectedTemplates_Model_With_Collection_Property()
        {
            // Arrange
            var properties = new[]
            {
                new ClassProperty("Property1", typeof(string).FullName),
                new ClassProperty("Property2", typeof(bool).FullName),
                new ClassProperty
                (
                    "Property3",
                    typeof(Collection<string>).FullName
                )
            };

            var model = new[]
            {
                new Class
                (
                    "MyRecord",
                    "MyNamespace",
                    properties: properties
                ).ToImmutableClass(new ImmutableClassSettings(createWithMethod: true))
            };
            var sut = new CSharpClassGenerator();

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(ImmutableClassCodeWithCollection);
        }

        [Fact]
        public void GeneratesImmutableClassWithInjectedTemplates_Model_With_IList_Property()
        {
            // Arrange
            var properties = new[]
            {
                new ClassProperty("Property1", typeof(string).FullName),
                new ClassProperty("Property2", typeof(bool).FullName),
                new ClassProperty
                (
                    "Property3",
                    typeof(IList<string>).FullName
                )
            };

            var model = new[]
            {
                new Class
                (
                    "MyRecord",
                    "MyNamespace",
                    properties: properties
                ).ToImmutableClass(new ImmutableClassSettings(createWithMethod: true))
            };
            var sut = new CSharpClassGenerator();

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(ImmutableClassCodeWithCollection);
        }

        [Fact]
        public void GeneratesImmutableClassWithInjectedTemplates_Model_With_List_Property()
        {
            // Arrange
            var properties = new[]
            {
                new ClassProperty("Property1", typeof(string).FullName),
                new ClassProperty("Property2", typeof(bool).FullName),
                new ClassProperty
                (
                    "Property3",
                    typeof(List<string>).FullName
                )
            };

            var model = new[]
            {
                new Class
                (
                    "MyRecord",
                    "MyNamespace",
                    properties: properties
                ).ToImmutableClass(new ImmutableClassSettings(createWithMethod: true))
            };
            var sut = new CSharpClassGenerator();

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(ImmutableClassCodeWithCollection);
        }

        [Fact]
        public void Generating_ImmutableClass_From_Class_Without_Properties_Throws_Exception()
        {
            // Arrange
            var input = new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").Build();

            // Act & Assert
            input.Invoking(x => x.ToImmutableClass(new ImmutableClassSettings()))
                 .Should().Throw<System.InvalidOperationException>()
                 .WithMessage("To create an immutable class, there must be at least one property");
        }

        [Fact]
        public void GeneratesPocoClass_Model_With_Collection_Property()
        {
            // Arrange
            var properties = new[]
            {
                new ClassProperty("Property1", typeof(string).FullName),
                new ClassProperty("Property2", typeof(bool).FullName),
                new ClassProperty
                (
                    "Property3",
                    typeof(ICollection<string>).FullName
                )
            };

            var model = new[]
            {
                new Class
                (
                    "MyPoco",
                    "MyNamespace",
                    properties: properties
                ).ToPocoClass()
            };
            var sut = new CSharpClassGenerator();

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"using System;
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
                new ClassProperty("Property1", typeof(string).FullName),
                new ClassProperty("Property2", typeof(bool).FullName),
                new ClassProperty
                (
                    "Property3",
                    typeof(IEnumerable<string>).FullName
                )
            };

            var model = new[]
            {
                new Class
                (
                    "MyRecordBuilder",
                    "MyNamespace",
                    properties: properties
                ).ToObservableClass()
            };
            var sut = new CSharpClassGenerator();

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"using System;
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
            this.Property3 = Utilities.Extensions.InitializeObservableCollection(default(System.Collections.ObjectModel.ObservableCollection<string>));
        }

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
                }.GetType().ToClassBuilder(new ClassSettings())
                    .WithName("MyRecord")
                    .WithNamespace("MyNamespace")
                    .Build()
                    .ToImmutableClass(new ImmutableClassSettings())
            };
            var sut = new CSharpClassGenerator();

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(ImmutableClassCodeNoWithMethod);
        }

        [Fact]
        public void GeneratesImmutableBuilderClassesForAllCsharpModelEntities()
        {
            // Arrange
            var models = typeof(Class).Assembly.GetExportedTypes()
                .Where(t => t.FullName.StartsWith("ModelFramework.Objects.Default."))
                .Select(t => t.ToClassBuilder(new ClassSettings(createConstructors: true)).WithName(t.Name).WithNamespace(FixNamespace(t)))
                .ToArray();

            FixImmutableBuilderProperties(models);
            var settings = new ImmutableBuilderClassSettings(addCopyConstructor: true,
                                                             formatInstanceTypeNameDelegate: FormatInstanceTypeName);
            // Act
            var builderModels = models.SelectMany(c => new[] { c.Build().ToImmutableBuilderClassBuilder(settings).WithNamespace("ModelFramework.Objects.Builders").Build() }).ToArray();
            var sut = new CSharpClassGenerator();
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, builderModels);

            actual.Should().NotBeNullOrEmpty().And.NotStartWith("Error:");
        }

        [Fact]
        public void GeneratesImmutableBuilderClassesForAllCommonModelEntities()
        {
            // Arrange
            var models = typeof(Metadata).Assembly.GetExportedTypes()
                .Where(t => t.FullName.StartsWith("ModelFramework.Common.Default."))
                .Select(t => t.ToClassBuilder(new ClassSettings(createConstructors: true)).WithName(t.Name).WithNamespace(FixNamespace(t)))
                .ToArray();
            FixImmutableBuilderProperties(models);
            var settings = new ImmutableBuilderClassSettings(newCollectionTypeName: "System.Collections.Generic.IReadOnlyCollection",
                                                             addCopyConstructor: true,
                                                             formatInstanceTypeNameDelegate: FormatInstanceTypeName);

            // Act
            var builderModels = models.SelectMany(c => new[] { c.Build().ToImmutableBuilderClassBuilder(settings).WithNamespace("ModelFramework.Common.Builders").Build() }).ToArray();
            var sut = new CSharpClassGenerator();
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, builderModels);

            // Assert
            actual.Should().NotBeNullOrEmpty().And.NotStartWith("Error:");
        }

        [Fact]
        public void GeneratesImmutableBuilderClassForRecord()
        {
            // Arrange
            var input = typeof(Person).ToClass(new ClassSettings(createConstructors: true));
            var settings = new ImmutableBuilderClassSettings(addCopyConstructor: true);

            // Act
            var builder = input.ToImmutableBuilderClass(settings);
            var sut = new CSharpClassGenerator();
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, new[] { builder });

            // Assert
            actual.Should().Be(@"using System;
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

        public PersonBuilder Clear()
        {
            FirstName = default;
            LastName = default;
            return this;
        }

        public PersonBuilder Update(ModelFramework.Generators.Objects.Tests.Person source)
        {
            FirstName = source.FirstName;
            LastName = source.LastName;
            return this;
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

        public PersonBuilder(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
");
        }

        [Fact]
        public void Generating_ImmutableBuilderClass_From_Class_Without_Properties_Throws_Exception()
        {
            // Arrange
            var input = new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").Build();

            // Act & Assert
            input.Invoking(x => x.ToImmutableBuilderClass(new ImmutableBuilderClassSettings()))
                 .Should().Throw<System.InvalidOperationException>()
                 .WithMessage("To create an immutable builder class, there must be at least one property");
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
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model, additionalParameters: new { EnableNullableContext = true });

            // Assert
            actual.Should().Be(@"using System;
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

        private static void FixImmutableBuilderProperties(ClassBuilder[] models)
        {
            foreach (var classBuilder in models)
            {
                foreach (var property in classBuilder.Properties)
                {
                    var typeName = property.TypeName.FixTypeName();
                    if (typeName.StartsWith("ModelFramework.Objects.Default.")
                        || typeName.StartsWith("ModelFramework.Database.Default.")
                        || typeName.StartsWith("ModelFramework.Common.Default."))
                    {
                        property.ConvertSinglePropertyToBuilder();
                    }
                    else if (typeName.Contains("IReadOnlyCollection<ModelFramework."))
                    {
                        property.ConvertCollectionPropertyToBuilder();
                    }
                }
            }
        }

        private static string FixNamespace(System.Type t)
        {
            if (t.FullName.StartsWith("ModelFramework.Common.Default"))
            {
                return t.FullName.Replace("ModelFramework.Common.Default", "ModelFramework.Common.Contracts").GetNamespaceWithDefault(string.Empty);
            }
            return t.FullName.GetNamespaceWithDefault(string.Empty);
        }

        private static string FormatInstanceTypeName(ITypeBase instance, bool forCreate)
        {
            if (instance.Namespace == "ModelFramework.Common.Contracts")
            {
                return forCreate
                    ? "ModelFramework.Common.Default." + instance.Name
                    : "ModelFramework.Common.Contracts.I" + instance.Name;
            }

            if (instance.Namespace == "ModelFramework.Objects.Contracts")
            {
                return forCreate
                    ? "ModelFramework.Objects.Default." + instance.Name
                    : "ModelFramework.Objects.Contracts.I" + instance.Name;
            }

            if (instance.Namespace == "ModelFramework.Database.Contracts")
            {
                return forCreate
                    ? "ModelFramework.Database.Default." + instance.Name
                    : "ModelFramework.Database.Contracts.I" + instance.Name;
            }

            return null;
        }

        private static IEnumerable<IClass> GetSubClasses()
        {
            yield return new Class("MySubClass", "MyNamespace", fields: GetFields(), properties: GetProperties(), subClasses: new[] { new Class("MySubSubClass", "MyNamespace", subClasses: new[] { new Class("MySubSubSubClass", "MyNamespace") }) });
        }

        private static IEnumerable<IEnum> GetEnums()
        {
            yield return new Enum("MyEnum", new[] { new EnumMember("Member1", 1), new EnumMember("Member2", 2) });
        }

        private static IEnumerable<IClassConstructor> GetConstructors()
        {
            yield return new ClassConstructor(parameters: new[] { new Parameter("Parameter1", typeof(string).FullName), new Parameter("Parameter2", typeof(int).FullName) }, codeStatements: new[] { new LiteralCodeStatement("throw new NotImplementedException();") });
        }

        private static IEnumerable<IClassMethod> GetMethods()
        {
            yield return new ClassMethod("Method1", null, parameters: new[] { new Parameter("Parameter1", typeof(string).FullName), new Parameter("Parameter2", typeof(int).FullName) }, codeStatements: new[] { new LiteralCodeStatement("throw new NotImplementedException();") });
        }

        private static IEnumerable<IClassProperty> GetProperties()
        {
            yield return new ClassProperty("MyProperty1", typeof(string).FullName);
            yield return new ClassProperty("MyProperty2", typeof(int).FullName, attributes: new[] { new Attribute("MyAttribute") });
        }

        private static IEnumerable<IClassField> GetFields()
        {
            yield return new ClassField("_myField1", typeof(string).FullName);
            yield return new ClassField("_myField2", typeof(string).FullName);
        }
    }

    [ExcludeFromCodeCoverage]
    public record Person
    {
        public string FirstName { get; init; }
        public string LastName { get; init; }
    }
}
