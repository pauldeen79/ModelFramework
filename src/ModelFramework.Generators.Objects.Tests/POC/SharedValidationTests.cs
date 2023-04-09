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
        results.Select(x => x.ErrorMessage).Should().BeEquivalentTo("The Name field is required.", "The Age field must be between 1 and 100.");
    }

    [Fact]
    public void Can_Generate_Validation_Atttributes()
    {
        // Arrange
        //note for demonstration purposes, we've added the UrlAttribute here manually
#pragma warning disable S3358 // Ternary operators should not be nested
        AttributeBuilder.RegisterCustomInitializer(a => a is UrlAttribute x
            ? new AttributeBuilder(x.GetType()).AddParameters(
                !string.IsNullOrEmpty(x.ErrorMessage)
                    ? new[] { new AttributeParameterBuilder().WithValue(x.ErrorMessage!).WithName(nameof(UrlAttribute.ErrorMessage)) }
                    : Array.Empty<AttributeParameterBuilder>())
            : null);
#pragma warning restore S3358 // Ternary operators should not be nested
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
    }
}
");
    }
}
