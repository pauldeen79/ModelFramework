namespace ModelFramework.Generators.Objects.Tests.Extensions;

public class TypeExtensionsTests
{
    [Fact]
    public void ToWrapperClass_Generates_Class_Correctly()
    {
        // Arrange
        var methodCodeStatementsDelegate = new Func<MethodInfo, IEnumerable<ICodeStatement>>(mi => CreateMethodCodeStatements(mi));
        var propertyCodeStatementsDelegate = new Func<PropertyInfo, IEnumerable<ICodeStatement>>(pi => CreatePropertyCodeStatements(pi));

        // Act
        var model = new[]
        {
            typeof(TestClass).ToWrapperClassBuilder(new WrapperClassSettings(methodCodeStatementsDelegate: methodCodeStatementsDelegate,
                                                                                propertyCodeStatementsDelegate: propertyCodeStatementsDelegate))
            .WithNamespace("MyNamespace")
            .WithName("GeneratedTestClass")
            .Build()
        };

        // Assert
        var template = new CSharpClassGenerator();
        var actual = TemplateRenderHelper.GetTemplateOutput(template, model);

        actual.Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
    public class GeneratedTestClass
    {
        public string Property
        {
            get
            {
                return _wrappedInstance.Property;
            }
        }

        public void DoSomething(string parameter)
        {
            _wrappedInstance.DoSomething(parameter);
        }

        public int MyFunction(int parameter)
        {
            return _wrappedInstance.MyFunction(parameter);
        }

        public override string ToString()
        {
            return _wrappedInstance.ToString();
        }

        public override bool Equals(object obj)
        {
            return _wrappedInstance.Equals(obj);
        }

        public override int GetHashCode()
        {
            return _wrappedInstance.GetHashCode();
        }

        public GeneratedTestClass(ModelFramework.Generators.Objects.Tests.Extensions.TestClass wrappedInstance)
        {
            _wrappedInstance = wrappedInstance;
        }

        private readonly ModelFramework.Generators.Objects.Tests.Extensions.TestClass _wrappedInstance;
    }
}
");
    }

    [Fact]
    public void Can_Use_ToClass_And_Then_Add_Data()
    {
        // Act
        var cls = new ClassBuilder(typeof(TestClass).ToClassBuilder(new ClassSettings())
            .WithName("GeneratedTestClass")
            .WithNamespace("MyNamespace")
            .Build());
        cls.Methods.ForEach(x => x.AddCodeStatements(new LiteralCodeStatementBuilder("Hello world!")));
        var model = new[]
        {
            cls.Build()
        };

        // Assert
        var template = new CSharpClassGenerator();
        var actual = TemplateRenderHelper.GetTemplateOutput(template, model);

        actual.Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
    public class GeneratedTestClass
    {
        public string Property
        {
            get;
        }

        public void DoSomething(string parameter)
        {
            Hello world!
        }

        public int MyFunction(int parameter)
        {
            Hello world!
        }

        public virtual string ToString()
        {
            Hello world!
        }

        public virtual bool Equals(object obj)
        {
            Hello world!
        }

        public virtual int GetHashCode()
        {
            Hello world!
        }
    }
}
");
    }

    private IEnumerable<ICodeStatement> CreateMethodCodeStatements(MethodInfo mi)
    {
        yield return new LiteralCodeStatementBuilder($"{GetPrefix(mi)}_wrappedInstance.{mi.Name}({GetArguments(mi)});").Build();
    }

    private string GetPrefix(MethodInfo mi)
        => mi.ReturnType == null || mi.ReturnType == typeof(void)
            ? ""
            : "return ";

    private string GetArguments(MethodInfo mi)
        => string.Join(", ", mi.GetParameters().Select(pi => pi.Name));

    private IEnumerable<ICodeStatement> CreatePropertyCodeStatements(PropertyInfo pi)
    {
        yield return new LiteralCodeStatementBuilder($"return _wrappedInstance.{pi.Name};").Build();
    }
}

public class TestClass
{
    public void DoSomething(string parameter)
    {
        Debug.WriteLine("Do something called");
    }

    public int MyFunction(int parameter)
    {
        Debug.WriteLine("My function called");
        return 4;
    }

    public string Property { get { Debug.WriteLine("Property called"); return "Hello world"; } }
}

public class ExampleWrapper
{
    private readonly TestClass _wrappedInstance;

    public ExampleWrapper(TestClass wrappedInstance)
    {
        _wrappedInstance = wrappedInstance;
    }

    public void DoSomething(string parameter)
    {
        //header
        _wrappedInstance.DoSomething(parameter);
        //footer
    }

    public int MyFunction(int parameter)
    {
        //header
        var returnValue = _wrappedInstance.MyFunction(parameter);
        //footer
        return returnValue;
    }

    public string Property
    {
        get
        {
            //header
            var returnValue = _wrappedInstance.Property;
            //footer
            return returnValue;
        }
    }
}

public class GeneratedTestClass
{
    public System.String Property
    {
        get { return _wrappedInstance.Property; }
    }

    public void DoSomething(System.String parameter)
    {
        _wrappedInstance.DoSomething(parameter);
    }

    public System.Int32 MyFunction(System.Int32 parameter)
    {
        return _wrappedInstance.MyFunction(parameter);
    }

    public override System.String? ToString()
    {
        return _wrappedInstance.ToString();
    }

    public override System.Boolean Equals(System.Object? obj)
    {
        return _wrappedInstance.Equals(obj);
    }

    public override System.Int32 GetHashCode()
    {
        return _wrappedInstance.GetHashCode();
    }

    public GeneratedTestClass(TestClass wrappedInstance)
    {
        _wrappedInstance = wrappedInstance;
    }

    private readonly TestClass _wrappedInstance;
}
