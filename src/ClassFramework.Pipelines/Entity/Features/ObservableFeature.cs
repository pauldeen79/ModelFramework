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

        if (!context.Context.Settings.GenerationSettings.CreateAsObservable
            && !context.Context.SourceModel.Interfaces.Any(x => x == typeof(INotifyPropertyChanged).FullName))
        {
            return Result.Continue<IConcreteTypeBuilder>();
        }

        if (context.Context.Settings.InheritanceSettings.EnableInheritance
            && context.Context.Settings.InheritanceSettings.BaseClass is not null)
        {
            // Already present in base class
            return Result.Continue<IConcreteTypeBuilder>();
        }

        if (!context.Context.SourceModel.Interfaces.Any(x => x == typeof(INotifyPropertyChanged).FullName))
        {
            // Only add the interface when it's not present yet :)
            context.Model.AddInterfaces(typeof(INotifyPropertyChanged));
        }

        context.Model
            .AddFields(new FieldBuilder()
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
