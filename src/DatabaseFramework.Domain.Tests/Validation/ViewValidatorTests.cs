namespace DatabaseFramework.Domain.Tests.Validation;

public class ViewValidatorTests
{
    [CustomValidation(typeof(ViewValidator), nameof(ViewValidator.Validate))] // test: illegal placement, only works on View or ViewBuilder, obviously!
    public class Validate
    {
        [Fact]
        public void Returns_Success_When_Instance_Is_Null()
        {
            // Arrange
            object instance = null!;

            // Act
            var result = ViewValidator.Validate(instance);

            // Assert
            result.Should().BeEquivalentTo(ValidationResult.Success);
        }

        [Fact]
        public void Returns_Failure_When_Applied_To_Instance_That_It_Not_View_Or_ViewBuilder()
        {
            // Arrange
            object instance = this; //wrong type!

            // Act
            var result = ViewValidator.Validate(instance);

            // Assert
            result.ErrorMessage.Should().Be("The ViewValidator attribute can only be applied to View and ViewBuilder types");
        }

        [Fact]
        public void Returns_Success_When_Instance_Has_Only_Definition()
        {
            // Arrange
            object instance = new ViewBuilder().WithName("MyView").WithDefinition("SELECT * FROM MyTable");

            // Act
            var result = ViewValidator.Validate(instance);

            // Assert
            result.Should().BeEquivalentTo(ValidationResult.Success);
        }

        [Fact]
        public void Returns_Success_When_Instance_Has_SelectFields_And_Sources()
        {
            // Arrange
            object instance = new ViewBuilder().WithName("MyView").AddSelectFields(new ViewFieldBuilder().WithName("MyField")).AddSources(new ViewSourceBuilder().WithName("MyTable"));

            // Act
            var result = ViewValidator.Validate(instance);

            // Assert
            result.Should().BeEquivalentTo(ValidationResult.Success);
        }

        [Fact]
        public void Returns_Failure_When_Instance_Has_Only_SelectFields()
        {
            // Arrange
            object instance = new ViewBuilder().WithName("MyView").AddSelectFields(new ViewFieldBuilder().WithName("MyField")); // missing sources here!

            // Act
            var result = ViewValidator.Validate(instance);

            // Assert
            result.ErrorMessage.Should().Be("When SelectFields or Sources is filled, then both fields are required");
        }

        [Fact]
        public void Returns_Failure_When_Instance_Has_Only_Sources()
        {
            // Arrange
            object instance = new ViewBuilder().WithName("MyView").AddSources(new ViewSourceBuilder().WithName("MyTable")); // missing select fields here!

            // Act
            var result = ViewValidator.Validate(instance);

            // Assert
            result.ErrorMessage.Should().Be("When SelectFields or Sources is filled, then both fields are required");
        }

        [Fact]
        public void Returns_Failure_When_Instance_Has_Sources_And_Definition()
        {
            // Arrange
            object instance = new ViewBuilder().WithName("MyView").WithDefinition("SELECT * FROM MyTable").AddSources(new ViewSourceBuilder().WithName("MyTable"));

            // Act
            var result = ViewValidator.Validate(instance);

            // Assert
            result.ErrorMessage.Should().Be("When Definition is filled, then SelectFields and Sources need to be empty");
        }

        [Fact]
        public void Returns_Failure_When_Instance_Has_SelectFields_And_Definition()
        {
            // Arrange
            object instance = new ViewBuilder().WithName("MyView").WithDefinition("SELECT * FROM MyTable").AddSources(new ViewSourceBuilder().WithName("MyTable")).AddSelectFields(new ViewFieldBuilder().WithName("MyField"));

            // Act
            var result = ViewValidator.Validate(instance);

            // Assert
            result.ErrorMessage.Should().Be("When Definition is filled, then SelectFields and Sources need to be empty");
        }

        [Fact]
        public void Returns_Failure_When_Instance_Has_Sources_And_SelectFields_And_Definition()
        {
            // Arrange
            object instance = new ViewBuilder().WithName("MyView").WithDefinition("SELECT * FROM MyTable").AddSelectFields(new ViewFieldBuilder().WithName("MyField"));

            // Act
            var result = ViewValidator.Validate(instance);

            // Assert
            result.ErrorMessage.Should().Be("When Definition is filled, then SelectFields and Sources need to be empty");
        }
    }
}
