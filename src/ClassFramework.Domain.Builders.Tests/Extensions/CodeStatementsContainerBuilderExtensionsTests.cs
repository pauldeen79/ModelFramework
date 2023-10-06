namespace ClassFramework.Domain.Builders.Tests.Extensions;

public class CodeStatementsContainerBuilderExtensionsTests
{
    public class AddStringCodeStatements
    {
        [Fact]
        public void Can_Add_String_CodeStatements_Using_Array()
        {
            // Arrange
            var sut = new ClassConstructorBuilder();

            // Act
            var result = sut.AddStringCodeStatements("StatementOne();", "StatementTwo();");

            // Assert
            result.CodeStatements.Should().AllBeOfType<StringCodeStatementBuilder>();
            result.CodeStatements.OfType<StringCodeStatementBuilder>().Select(x => x.Statement).Should().BeEquivalentTo("StatementOne();", "StatementTwo();");
        }
        [Fact]
        public void Can_Add_String_CodeStatements_Using_Enumerable()
        {
            // Arrange
            var sut = new ClassConstructorBuilder();

            // Act
            var result = sut.AddStringCodeStatements(new[] { "StatementOne();", "StatementTwo();" }.AsEnumerable());

            // Assert
            result.CodeStatements.Should().AllBeOfType<StringCodeStatementBuilder>();
            result.CodeStatements.OfType<StringCodeStatementBuilder>().Select(x => x.Statement).Should().BeEquivalentTo("StatementOne();", "StatementTwo();");
        }
    }
}
