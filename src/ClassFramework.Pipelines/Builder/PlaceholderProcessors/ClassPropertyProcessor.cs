namespace ClassFramework.Pipelines.Builder.PlaceholderProcessors;

public class ClassPropertyProcessor : IPlaceholderProcessor
{
    public int Order => 20;

    public Result<string> Process(string value, IFormatProvider formatProvider, object? context, IFormattableStringParser formattableStringParser)
    {
        //if (context is ClassProperty property)
        if (context is PipelineContext<ClassProperty, PipelineBuilderContext> pipelineContext)
        {
            return value switch
            {
                nameof(ClassProperty.TypeName) => Result<string>.Success(pipelineContext.Model.TypeName.FixTypeName()),
                $"{nameof(ClassProperty.TypeName)}.GenericArguments" => Result<string>.Success(pipelineContext.Model.TypeName.FixTypeName().GetGenericArguments()),
                $"{nameof(ClassProperty.TypeName)}.ClassName" => Result<string>.Success(pipelineContext.Model.TypeName.FixTypeName().GetClassName()),
                $"{nameof(ClassProperty.TypeName)}.GenericArguments.ClassName" => Result<string>.Success(pipelineContext.Model.TypeName.FixTypeName().GetGenericArguments().GetClassName()),
                _ => Result<string>.Continue()
            };
        }

        return Result<string>.Continue();
    }
}
