namespace ClassFramework.TemplateFramework.ViewModels;

public class UsingsViewModel : CsharpClassGeneratorViewModelBase<IEnumerable<TypeBase>>
{
    public UsingsViewModel(CsharpClassGeneratorSettings settings, ICsharpExpressionCreator csharpExpressionCreator)
        : base(settings, csharpExpressionCreator)
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

public class UsingsViewModelCreator : IViewModelCreator
{
    private readonly ICsharpExpressionCreator _csharpExpressionCreator;

    public UsingsViewModelCreator(ICsharpExpressionCreator csharpExpressionCreator)
    {
        Guard.IsNotNull(csharpExpressionCreator);

        _csharpExpressionCreator = csharpExpressionCreator;
    }

    public object Create(object model, CsharpClassGeneratorSettings settings)
        => new UsingsViewModel(settings, _csharpExpressionCreator);

    public bool Supports(object model)
        => model is UsingsModel;
}
