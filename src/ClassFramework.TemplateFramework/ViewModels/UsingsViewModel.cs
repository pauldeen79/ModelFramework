namespace ClassFramework.TemplateFramework.ViewModels;

public class UsingsViewModel : CsharpClassGeneratorViewModelBase<IEnumerable<TypeBase>>
{
    public UsingsViewModel(ICsharpExpressionCreator csharpExpressionCreator)
        : base(csharpExpressionCreator)
    {
    }

    private readonly static string[] DefaultUsings =
    [
        "System",
        "System.Collections.Generic",
        "System.Linq",
        "System.Text"
    ];

    public IEnumerable<string> Usings
        => DefaultUsings
            .Union(GetModel().SelectMany(classItem => classItem.Metadata.GetStringValues(MetadataNames.CustomUsing)))
            .OrderBy(ns => ns)
            .Distinct();
}

public class UsingsViewModelFactoryComponent : IViewModelFactoryComponent
{
    private readonly ICsharpExpressionCreator _csharpExpressionCreator;

    public UsingsViewModelFactoryComponent(ICsharpExpressionCreator csharpExpressionCreator)
    {
        Guard.IsNotNull(csharpExpressionCreator);

        _csharpExpressionCreator = csharpExpressionCreator;
    }

    public object Create()
        => new UsingsViewModel(_csharpExpressionCreator);

    public bool Supports(object model)
        => model is UsingsModel;
}
