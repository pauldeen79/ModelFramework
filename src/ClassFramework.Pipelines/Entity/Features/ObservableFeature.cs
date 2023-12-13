namespace ClassFramework.Pipelines.Entity.Features;

public class ObservableFeatureBuilder : IEntityFeatureBuilder
{
    public IPipelineFeature<TypeBaseBuilder, EntityContext> Build()
        => new ObservableFeature();
}

public class ObservableFeature : IPipelineFeature<TypeBaseBuilder, EntityContext>
{
    public Result<TypeBaseBuilder> Process(PipelineContext<TypeBaseBuilder, EntityContext> context)
    {
        context = context.IsNotNull(nameof(context));

        if (!context.Context.Settings.GenerationSettings.CreateAsObservable)
        {
            return Result.Continue<TypeBaseBuilder>();
        }

        if (context.Model is not IFieldsContainerBuilder fieldsContainerBuilder)
        {
            return Result.Invalid<TypeBaseBuilder>("Context model must implement IFieldsContainerBuilder");
        }

        context.Model.Interfaces.Add(typeof(INotifyPropertyChanged).FullName!);
        fieldsContainerBuilder.Fields.Add(new ClassFieldBuilder()
            .WithName(nameof(INotifyPropertyChanged.PropertyChanged))
            .WithType(typeof(PropertyChangedEventHandler))
            .WithEvent()
            .WithIsNullable()
            .WithVisibility(Visibility.Public)
            );

        return Result.Continue<TypeBaseBuilder>();
    }

    public IBuilder<IPipelineFeature<TypeBaseBuilder, EntityContext>> ToBuilder()
        => new ObservableFeatureBuilder();
}
