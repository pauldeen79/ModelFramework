namespace ModelFramework.Generators.Objects.Tests.POC;

public class SharedValidationTests
{
    [Fact]
    public void Can_Validate_Builder_With_Shared_Domain_Logic()
    {
        // Arrange
        var builder = new MySharedValidationDomainEntityBuilder();

        // Act
        var results = builder.Validate(new ValidationContext(builder));

        // Assert
        results.Select(x => x.ErrorMessage).Should().BeEquivalentTo("The Name field is required.", "The Age field must be between 1 and 100.", "The PhoneNumber field is not a valid phone number.", "The EmailAddress field is not a valid e-mail address.");
    }

    [Fact]
    public void Can_Generate_Validation_Attributes()
    {
        // Arrange
        var model = new[]
        {
            typeof(MySharedValidationDomainEntity1Base).ToClass()
        };
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelFramework.Generators.Objects.Tests.POC
{
    public record MySharedValidationDomainEntity1Base
    {
        [System.ComponentModel.DataAnnotations.RequiredAttribute]
        [System.ComponentModel.DataAnnotations.StringLengthAttribute(10, MinimumLength = 1)]
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1, ErrorMessage = @""boohoo1"")]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(10, ErrorMessage = @""boohoo2"")]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute(@""dog$"", ErrorMessage = @""boohoo3"")]
        [System.ComponentModel.DataAnnotations.UrlAttribute(ErrorMessage = @""boohoo4"")]
        public string Name
        {
            get;
        }

        [System.ComponentModel.DataAnnotations.RangeAttribute(1, 100, ErrorMessage = @""The Age field must be between 1 and 100."")]
        public int Age
        {
            get;
        }

        [System.ComponentModel.DataAnnotations.PhoneAttribute(ErrorMessage = @""The {0} field is not a valid phone number."")]
        public string PhoneNumber
        {
            get;
        }

        [System.ComponentModel.DataAnnotations.EmailAddressAttribute(ErrorMessage = @""The {0} field is not a valid e-mail address."")]
        public string EmailAddress
        {
            get;
        }
    }
}
");
    }

    [Fact]
    public void Can_Generate_Custom_Validator()
    {
        // Arrange
        var model = new[]
        {
            typeof(MySharedValidationDomainEntity1Base).ToClassBuilder()
                .WithCustomValidationCode("new MyNamespace.Validator<{0}>().Validate(this);")
                .Build()
                .ToImmutableClass(new(constructorSettings: new(validateArguments: ArgumentValidationType.DomainOnly)))
        };
        var sut = new CSharpClassGenerator();

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelFramework.Generators.Objects.Tests.POC
{
    public class MySharedValidationDomainEntity1Base
    {
        [System.ComponentModel.DataAnnotations.RequiredAttribute]
        [System.ComponentModel.DataAnnotations.StringLengthAttribute(10, MinimumLength = 1)]
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1, ErrorMessage = @""boohoo1"")]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(10, ErrorMessage = @""boohoo2"")]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute(@""dog$"", ErrorMessage = @""boohoo3"")]
        [System.ComponentModel.DataAnnotations.UrlAttribute(ErrorMessage = @""boohoo4"")]
        public string Name
        {
            get;
        }

        [System.ComponentModel.DataAnnotations.RangeAttribute(1, 100, ErrorMessage = @""The Age field must be between 1 and 100."")]
        public int Age
        {
            get;
        }

        [System.ComponentModel.DataAnnotations.PhoneAttribute(ErrorMessage = @""The {0} field is not a valid phone number."")]
        public string PhoneNumber
        {
            get;
        }

        [System.ComponentModel.DataAnnotations.EmailAddressAttribute(ErrorMessage = @""The {0} field is not a valid e-mail address."")]
        public string EmailAddress
        {
            get;
        }

        public MySharedValidationDomainEntity1Base(string name, int age, string phoneNumber, string emailAddress)
        {
            this.Name = name;
            this.Age = age;
            this.PhoneNumber = phoneNumber;
            this.EmailAddress = emailAddress;
            new MyNamespace.Validator<ModelFramework.Generators.Objects.Tests.POC.MySharedValidationDomainEntity1Base>().Validate(this);
        }
    }
}
");
    }
}
