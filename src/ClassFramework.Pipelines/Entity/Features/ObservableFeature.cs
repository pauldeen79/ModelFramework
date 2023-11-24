namespace ClassFramework.Pipelines.Entity.Features;

public class ObservableFeatureBuilder : IEntityFeatureBuilder
{
    public IPipelineFeature<ClassBuilder, EntityContext> Build()
        => new ObservableFeature();
}

public class ObservableFeature : IPipelineFeature<ClassBuilder, EntityContext>
{
    public Result<ClassBuilder> Process(PipelineContext<ClassBuilder, EntityContext> context)
    {
        context = context.IsNotNull(nameof(context));

        if (!context.Context.Settings.GenerationSettings.CreateAsObservable)
        {
            return Result.Continue<ClassBuilder>();
        }

        context.Model.AddInterfaces(typeof(INotifyPropertyChanged));
        context.Model.AddFields(new ClassFieldBuilder()
            .WithName(nameof(INotifyPropertyChanged.PropertyChanged))
            .WithType(typeof(PropertyChangedEventHandler))
            .WithEvent()
            .WithIsNullable()
            .WithVisibility(Visibility.Public)
            );

        return Result.Continue<ClassBuilder>();
    }

    public IBuilder<IPipelineFeature<ClassBuilder, EntityContext>> ToBuilder()
        => new ObservableFeatureBuilder();
}
