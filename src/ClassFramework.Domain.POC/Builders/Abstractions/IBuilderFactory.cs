namespace ClassFramework.Domain.Builders.Abstractions;

// Test code to create copy constructors on builders with support for inherited entities
public interface IBuilderFactory
{
    T Create<T>(object? source);
}
