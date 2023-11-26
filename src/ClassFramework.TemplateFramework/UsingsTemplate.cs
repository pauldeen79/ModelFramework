namespace ClassFramework.TemplateFramework;

public sealed class UsingsTemplate : CsharpClassGeneratorBase<CsharpClassGeneratorViewModel<IEnumerable<TypeBase>>>, IStringBuilderTemplate
{
    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);

        var anyUsings = false;
        foreach (var @using in Usings)
        {
            builder.AppendLine(Model.Settings.CultureInfo, $"using {@using};");
            anyUsings = true;
        }

        if (anyUsings)
        {
            builder.AppendLine();
        }
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
            .Union(Model?.Data.SelectMany(classItem => classItem.Metadata.GetStringValues(MetadataNames.CustomUsing)) ?? Enumerable.Empty<string>())
            .OrderBy(ns => ns)
            .Distinct();
}
