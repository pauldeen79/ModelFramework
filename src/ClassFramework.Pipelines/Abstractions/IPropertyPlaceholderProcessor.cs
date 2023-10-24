namespace ClassFramework.Pipelines.Abstractions;

public interface IPropertyPlaceholderProcessor
{
    Result<string> Process(string value, IFormatProvider formatProvider, object? context, IFormattableStringParser formattableStringParser);
}
