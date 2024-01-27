namespace ClassFramework.Pipelines.Entity.Features;

public class AddToBuilderMethodFeatureBuilder : IEntityFeatureBuilder
{
    private readonly IFormattableStringParser _formattableStringParser;

    public AddToBuilderMethodFeatureBuilder(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public IPipelineFeature<IConcreteTypeBuilder, EntityContext> Build()
        => new AddToBuilderMethodFeature(_formattableStringParser);
}

public class AddToBuilderMethodFeature : IPipelineFeature<IConcreteTypeBuilder, EntityContext>
{
    private readonly IFormattableStringParser _formattableStringParser;

    public AddToBuilderMethodFeature(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public Result<IConcreteTypeBuilder> Process(PipelineContext<IConcreteTypeBuilder, EntityContext> context)
    {
        context = context.IsNotNull(nameof(context));

        if (context.Context.IsSharedBaseClass)
        {
            return Result.Continue<IConcreteTypeBuilder>();
        }

        var results = new[]
        {
            new { Name = "Name", LazyResult = new Lazy<Result<string>>(() => _formattableStringParser.Parse(context.Context.Settings.NameSettings.EntityNameFormatString, context.Context.FormatProvider, context)) },
            new { Name = "Namespace", LazyResult = new Lazy<Result<string>>(() => context.Context.SourceModel.Metadata.WithMappingMetadata(context.Context.SourceModel.GetFullName().GetCollectionItemType().WhenNullOrEmpty(context.Context.SourceModel.GetFullName), context.Context.Settings.TypeSettings).GetStringResult(MetadataNames.CustomEntityNamespace, () => _formattableStringParser.Parse(context.Context.Settings.NameSettings.EntityNamespaceFormatString, context.Context.FormatProvider, context))) },
            new { Name = "ToBuilderMethodName", LazyResult = new Lazy<Result<string>>(() => _formattableStringParser.Parse(context.Context.Settings.NameSettings.ToBuilderFormatString, context.Context.FormatProvider, context)) },
            new { Name = "ToTypedBuilderMethodName", LazyResult = new Lazy<Result<string>>(() => _formattableStringParser.Parse(context.Context.Settings.NameSettings.ToTypedBuilderFormatString, context.Context.FormatProvider, context)) }
        }.TakeWhileWithFirstNonMatching(x => x.LazyResult.Value.IsSuccessful()).ToArray();

        var error = Array.Find(results, x => !x.LazyResult.Value.IsSuccessful());
        if (error is not null)
        {
            // Error in formattable string parsing
            return Result.FromExistingResult<IConcreteTypeBuilder>(error.LazyResult.Value);
        }

        var methodName = results.First(x => x.Name == "ToBuilderMethodName").LazyResult.Value.Value!;
        if (string.IsNullOrEmpty(methodName))
        {
            return Result.Continue<IConcreteTypeBuilder>();
        }

        var typedMethodName = results.First(x => x.Name == "ToTypedBuilderMethodName").LazyResult.Value.Value!;

        var entityFullName = string.IsNullOrEmpty(results.First(x => x.Name == "Namespace").LazyResult.Value.Value)
            ? results.First(x => x.Name == "Name").LazyResult.Value.Value!
            : $"{results.First(x => x.Name == "Namespace").LazyResult.Value.Value}.{results.First(x => x.Name == "Name").LazyResult.Value.Value}";

        if (context.Context.Settings.InheritanceSettings.EnableInheritance && context.Context.Settings.InheritanceSettings.BaseClass is not null)
        {
            entityFullName = entityFullName.ReplaceSuffix("Base", string.Empty, StringComparison.Ordinal);
        }

        var entityConcreteFullName = context.Context.Settings.InheritanceSettings.EnableInheritance && context.Context.Settings.InheritanceSettings.BaseClass is not null
            ? context.Context.Settings.InheritanceSettings.BaseClass.GetFullName()
            : entityFullName;

        var builderNamespaceResult = context.Context.SourceModel.Metadata.WithMappingMetadata(entityFullName.GetCollectionItemType().WhenNullOrEmpty(entityFullName), context.Context.Settings.TypeSettings).GetStringResult(MetadataNames.CustomBuilderNamespace, () => Result.Success(string.IsNullOrEmpty(results.First(x => x.Name == "Namespace").LazyResult.Value.Value)
            ? "Builders"
            : $"{results.First(x => x.Name == "Namespace").LazyResult.Value.Value}.Builders"));

        var concreteBuilderNamespaceResult = context.Context.SourceModel.Metadata.WithMappingMetadata(entityConcreteFullName.GetCollectionItemType().WhenNullOrEmpty(entityConcreteFullName), context.Context.Settings.TypeSettings).GetStringResult(MetadataNames.CustomBuilderNamespace, () => Result.Success(string.IsNullOrEmpty(results.First(x => x.Name == "Namespace").LazyResult.Value.Value)
            ? "Builders"
            : $"{results.First(x => x.Name == "Namespace").LazyResult.Value.Value}.Builders"));

        var name = context.Context.Settings.InheritanceSettings.EnableInheritance && context.Context.Settings.InheritanceSettings.BaseClass is null
            ? results.First(x => x.Name == "Name").LazyResult.Value.Value!
            : results.First(x => x.Name == "Name").LazyResult.Value.Value!.ReplaceSuffix("Base", string.Empty, StringComparison.Ordinal);

        var builderConcreteTypeName = $"{builderNamespaceResult.Value}.{name}Builder";

        var builderTypeName = context.Context.Settings.InheritanceSettings.EnableInheritance && context.Context.Settings.InheritanceSettings.BaseClass is not null
            ? $"{concreteBuilderNamespaceResult.Value}.{context.Context.Settings.InheritanceSettings.BaseClass.Name}Builder"
            : builderConcreteTypeName;

        var returnStatement = context.Context.Settings.InheritanceSettings.EnableInheritance && context.Context.Settings.InheritanceSettings.BaseClass is not null && !string.IsNullOrEmpty(typedMethodName)
            ? $"return {typedMethodName}();"
            : $"return new {builderConcreteTypeName}(this);";

        context.Model
            .AddMethods(new MethodBuilder()
                .WithName(methodName)
                .WithAbstract(context.Context.IsAbstract)
                .WithOverride(context.Context.Settings.InheritanceSettings.BaseClass is not null)
                .WithReturnTypeName(builderTypeName)
                .AddStringCodeStatements(returnStatement));

        if (context.Context.Settings.InheritanceSettings.EnableInheritance
            && context.Context.Settings.InheritanceSettings.BaseClass is not null
            && !string.IsNullOrEmpty(typedMethodName))
        {
            context.Model
                .AddMethods(new MethodBuilder()
                    .WithName(typedMethodName)
                    .WithReturnTypeName(builderConcreteTypeName)
                    .AddStringCodeStatements($"return new {builderConcreteTypeName}(this);"));
        }

        return Result.Continue<IConcreteTypeBuilder>();
    }

    public IBuilder<IPipelineFeature<IConcreteTypeBuilder, EntityContext>> ToBuilder()
        => new AddToBuilderMethodFeatureBuilder(_formattableStringParser);
}
