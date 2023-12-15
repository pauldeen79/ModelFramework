namespace ClassFramework.Pipelines.Entity.Features;

public class ObservableFeatureBuilder : IEntityFeatureBuilder
{
    public IPipelineFeature<IConcreteTypeBuilder, EntityContext> Build()
        => new ObservableFeature();
}

public class ObservableFeature : IPipelineFeature<IConcreteTypeBuilder, EntityContext>
{
    public Result<IConcreteTypeBuilder> Process(PipelineContext<IConcreteTypeBuilder, EntityContext> context)
    {
        context = context.IsNotNull(nameof(context));

        if (!context.Context.Settings.GenerationSettings.CreateAsObservable)
        {
            return Result.Continue<IConcreteTypeBuilder>();
        }

        if (context.Model is not IFieldsContainerBuilder fieldsContainerBuilder)
        {
            return Result.Invalid<IConcreteTypeBuilder>("Context model must implement IFieldsContainerBuilder");
        }

        context.Model.Interfaces.Add(typeof(INotifyPropertyChanged).FullName);
        fieldsContainerBuilder.Fields.Add(new ClassFieldBuilder()
            .WithName(nameof(INotifyPropertyChanged.PropertyChanged))
            .WithType(typeof(PropertyChangedEventHandler))
            .WithEvent()
            .WithIsNullable()
            .WithVisibility(Visibility.Public)
            );

        return Result.Continue<IConcreteTypeBuilder>();
    }

    public IBuilder<IPipelineFeature<IConcreteTypeBuilder, EntityContext>> ToBuilder()
        => new ObservableFeatureBuilder();
}
