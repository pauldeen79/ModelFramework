namespace ClassFramework.TemplateFramework.Models;

public class PropertyCodeBodyModel
{
    public PropertyCodeBodyModel(bool isAvailable, string verb, Visibility visibilty, Visibility? subVisibility, object? parentModel, IReadOnlyCollection<CodeStatementBase> codeStatementModels, CultureInfo cultureInfo)
    {
        Guard.IsNotNull(verb);
        Guard.IsNotNull(codeStatementModels);

        IsAvailable = isAvailable;
        Verb = verb;
        Modifiers = GetModifiers(visibilty, subVisibility, cultureInfo);
        OmitCode = codeStatementModels.Count == 0 || parentModel is Interface;
        CodeStatementModels = codeStatementModels;
    }

    public bool IsAvailable { get; }
    public string Verb { get; }
    public string Modifiers { get; }
    public bool OmitCode { get; }
    public IReadOnlyCollection<CodeStatementBase> CodeStatementModels { get; }

    private static string GetModifiers(Visibility visibility, Visibility? subVisibility, CultureInfo cultureInfo)
    {
        var builder = new StringBuilder();

        if (subVisibility is not null && subVisibility != visibility)
        {
            builder.Append(subVisibility.ToString()!.ToLower(cultureInfo));
        }

        if (builder.Length > 0)
        {
            builder.Append(" ");
        }

        return builder.ToString();
    }
}
