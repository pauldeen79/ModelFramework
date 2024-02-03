namespace ClassFramework.Pipelines.Builder.Features;

public class ObservableFeatureBuilder : IBuilderFeatureBuilder
{
    public IPipelineFeature<IConcreteTypeBuilder, BuilderContext> Build()
        => new ObservableFeature();
}

public class ObservableFeature : IPipelineFeature<IConcreteTypeBuilder, BuilderContext>
{
    public Result<IConcreteTypeBuilder> Process(PipelineContext<IConcreteTypeBuilder, BuilderContext> context)
    {
        context = context.IsNotNull(nameof(context));

        if (!context.Context.Settings.EntitySettings.GenerationSettings.CreateAsObservable
            && !context.Context.SourceModel.Interfaces.Any(x => x == typeof(INotifyPropertyChanged).FullName))
        {
            return Result.Continue<IConcreteTypeBuilder>();
        }

        if (context.Context.Settings.EntitySettings.InheritanceSettings.EnableInheritance
            && context.Context.Settings.EntitySettings.InheritanceSettings.BaseClass is not null)
        {
            // Already present in base class
            return Result.Continue<IConcreteTypeBuilder>();
        }

        if (context.Context.IsBuilderForAbstractEntity && context.Context.IsAbstractBuilder)
        {
            // Already present in non-generic base class
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

        context.Model
            .AddMethods(new MethodBuilder()
                .WithName("HandlePropertyChanged")
                .AddParameter("propertyName", typeof(string))
                .WithProtected()
                .AddStringCodeStatements("PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));")
            );

        return Result.Continue<IConcreteTypeBuilder>();
    }

    public IBuilder<IPipelineFeature<IConcreteTypeBuilder, BuilderContext>> ToBuilder()
        => new ObservableFeatureBuilder();
}
