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
