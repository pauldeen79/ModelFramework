namespace ClassFramework.Pipelines.Builder.Features;

public class AddPropertiesFeatureBuilder : IBuilderFeatureBuilder
{
    private readonly IFormattableStringParser _formattableStringParser;

    public AddPropertiesFeatureBuilder(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public IPipelineFeature<IConcreteTypeBuilder, BuilderContext> Build()
        => new AddPropertiesFeature(_formattableStringParser);
}

public class AddPropertiesFeature : IPipelineFeature<IConcreteTypeBuilder, BuilderContext>
{
    private readonly IFormattableStringParser _formattableStringParser;

    public AddPropertiesFeature(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public Result<IConcreteTypeBuilder> Process(PipelineContext<IConcreteTypeBuilder, BuilderContext> context)
    {
        context = context.IsNotNull(nameof(context));

        if (context.Context.IsAbstractBuilder)
        {
            return Result.Continue<IConcreteTypeBuilder>();
        }

        foreach (var property in context.Context.SourceModel.Properties.Where(x => context.Context.SourceModel.IsMemberValidForBuilderClass(x, context.Context.Settings)))
        {
            var typeNameResult = property.GetBuilderArgumentType(context, _formattableStringParser);

            if (!typeNameResult.IsSuccessful())
            {
                return Result.FromExistingResult<IConcreteTypeBuilder>(typeNameResult);
            }

            /*var parentTypeNameResult = string.IsNullOrEmpty(property.ParentTypeFullName)
                ? Result.Success(string.Empty)
                : new PropertyBuilder().WithName("Dummy").WithTypeName(property.ParentTypeFullName).Build().GetBuilderArgumentType(context, _formattableStringParser);

            if (!parentTypeNameResult.IsSuccessful())
            {
                return Result.FromExistingResult<IConcreteTypeBuilder>(parentTypeNameResult);
            }*/

            Result<string> parentTypeNameResult;
            if (!string.IsNullOrEmpty(property.ParentTypeFullName))
            {
                var metadata = property.Metadata.WithMappingMetadata(property.ParentTypeFullName.GetCollectionItemType().WhenNullOrEmpty(property.ParentTypeFullName), context.Context.Settings.TypeSettings);
                var ns = metadata.GetStringValue(MetadataNames.CustomBuilderParentTypeNamespace);

                if (!string.IsNullOrEmpty(ns))
                {
                    var newTypeName = metadata.GetStringValue(MetadataNames.CustomBuilderParentTypeName, "{ParentTypeName.ClassName}");

                    if (property.TypeName.IsCollectionTypeName())
                    {
                        newTypeName = newTypeName.Replace("{TypeName.ClassName}", "{TypeName.GenericArguments.ClassName}");
                    }
                    var newFullName = $"{ns}.{newTypeName}";
                    /*if (property.TypeName.IsCollectionTypeName())
                    {
                        var idx = property.TypeName.IndexOf('<');
                        if (idx > -1)
                        {
                            newFullName = $"{property.TypeName.Substring(0, idx)}<{newFullName.Replace("{TypeName.ClassName}", "{TypeName.GenericArguments.ClassName}")}>";
                        }
                    }*/

                    parentTypeNameResult = _formattableStringParser.Parse
                    (
                        newFullName,
                        context.Context.FormatProvider,
                        new ParentChildContext<PipelineContext<IConcreteTypeBuilder, BuilderContext>, Property>(context, property, context.Context.Settings)
                    );
                }
                else
                {
                    parentTypeNameResult = Result.Success(context.Context.MapTypeName(property.ParentTypeFullName.FixTypeName()));
                }
            }
            else
            {
                parentTypeNameResult = Result.Success(property.ParentTypeFullName);
            }

            context.Model.AddProperties(new PropertyBuilder()
                .WithName(property.Name)
                .WithTypeName(typeNameResult.Value!
                    .FixCollectionTypeName(context.Context.Settings.TypeSettings.NewCollectionTypeName)
                    .FixNullableTypeName(property))
                .WithIsNullable(property.IsNullable)
                .WithIsValueType(property.IsValueType)
                .WithParentTypeFullName(parentTypeNameResult.Value!)
                .AddAttributes(property.Attributes
                    .Where(_ => context.Context.Settings.EntitySettings.CopySettings.CopyAttributes)
                    .Select(x => context.Context.MapAttribute(x).ToBuilder()))
                .AddMetadata(property.Metadata.Select(x => x.ToBuilder()))
                .AddGetterCodeStatements(CreateBuilderPropertyGetterStatements(property, context.Context))
                .AddSetterCodeStatements(CreateBuilderPropertySetterStatements(property, context.Context))
            );
        }

        // Note that we are not checking the result, because the same formattable string (CustomBuilderArgumentType) has already been checked earlier in this class
        // We can simple use GetValueOrThrow to keep the compiler happy (the value should be a string, and not be null)
        context.Model.AddFields(context.Context.SourceModel
            .GetBuilderClassFields(context, _formattableStringParser)
            .Select(x => x.GetValueOrThrow()));

        return Result.Continue<IConcreteTypeBuilder>();
    }

    public IBuilder<IPipelineFeature<IConcreteTypeBuilder, BuilderContext>> ToBuilder()
        => new AddPropertiesFeatureBuilder(_formattableStringParser);

    private static IEnumerable<CodeStatementBaseBuilder> CreateBuilderPropertyGetterStatements(
        Property property,
        BuilderContext context)
    {
        if (property.HasBackingFieldOnBuilder(context.Settings.EntitySettings.NullCheckSettings.AddNullChecks, context.Settings.TypeSettings.EnableNullableReferenceTypes, context.Settings.EntitySettings.ConstructorSettings.OriginalValidateArguments, context.Settings.EntitySettings.GenerationSettings.AddBackingFields))
        {
            yield return new StringCodeStatementBuilder().WithStatement($"return _{property.Name.ToPascalCase(context.FormatProvider.ToCultureInfo())};");
        }
    }

    private static IEnumerable<CodeStatementBaseBuilder> CreateBuilderPropertySetterStatements(
        Property property,
        BuilderContext context)
    {
        if (property.HasBackingFieldOnBuilder(context.Settings.EntitySettings.NullCheckSettings.AddNullChecks, context.Settings.TypeSettings.EnableNullableReferenceTypes, context.Settings.EntitySettings.ConstructorSettings.OriginalValidateArguments, context.Settings.EntitySettings.GenerationSettings.AddBackingFields))
        {
            yield return new StringCodeStatementBuilder().WithStatement($"_{property.Name.ToPascalCase(context.FormatProvider.ToCultureInfo())} = value{property.GetNullCheckSuffix("value", context.Settings.EntitySettings.NullCheckSettings.AddNullChecks)};");
            if (context.Settings.EntitySettings.GenerationSettings.CreateAsObservable)
            {
                yield return new StringCodeStatementBuilder().WithStatement($"HandlePropertyChanged(nameof({property.Name}));");
            }
        }
    }
}
