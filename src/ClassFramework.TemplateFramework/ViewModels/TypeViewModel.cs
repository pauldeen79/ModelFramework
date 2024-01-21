namespace ClassFramework.TemplateFramework.ViewModels;

public class TypeViewModel : AttributeContainerViewModelBase<IType>
{
    public TypeViewModel(ICsharpExpressionCreator csharpExpressionCreator)
        : base(csharpExpressionCreator)
    {
    }

    public bool ShouldRenderNullablePragmas
        => GetSettings().EnableNullableContext
        && GetContext().GetIndentCount() == 1; // note: only for root level, because it gets rendered in the same file

    public bool ShouldRenderNamespaceScope
        => GetSettings().GenerateMultipleFiles
        && !string.IsNullOrEmpty(GetModel().Namespace);

    public string Name
        => GetModel().Name.Sanitize().GetCsharpFriendlyName();

    public string Namespace
        => GetModel().Namespace;

    public string Modifiers
        => GetModel().GetModifiers(GetSettings().CultureInfo);

    public IReadOnlyCollection<string> SuppressWarningCodes
        => GetModel().SuppressWarningCodes;

    public CodeGenerationHeaderModel GetCodeGenerationHeaderModel()
        => new CodeGenerationHeaderModel(Settings.CreateCodeGenerationHeader, Settings.EnvironmentVersion);

    public UsingsModel GetUsingsModel()
        => new UsingsModel([GetModel()]);

    public IEnumerable<object> GetMemberModels()
    {
        var items = new List<object?>();
        var model = GetModel();

        items.AddRange(model.Fields);
        items.AddRange(model.Properties);

        var constructorsContainer = Model as IConstructorsContainer;
        if (constructorsContainer is not null)
        {
            items.AddRange(constructorsContainer.Constructors);
        }

        items.AddRange(model.Methods);

        // Quirk, enums as items below a class. There is no interface for this right now.
        var cls = model as Class;
        if (cls is not null)
        {
            items.AddRange(cls.Enums);
        }

        // Add separators (empty lines) between each item
        return items.SelectMany((item, index) => index + 1 < items.Count ? [item!, new NewLineModel()] : new object[] { item! });
    }

    public IEnumerable<object> GetSubClassModels()
    {
        var subClasses = (GetModel() as Class)?.SubClasses;
        if (subClasses is null)
        {
            return Enumerable.Empty<object>();
        }

        return subClasses.SelectMany(item => new object[] { new NewLineModel(), item });
    }

    public string ContainerType
        => GetModel() switch
        {
            Class cls when cls.Record => "record",
            Class cls when !cls.Record => "class",
            Struct str when str.Record => "record struct",
            Struct str when !str.Record => "struct",
            Interface => "interface",
            _ => throw new NotSupportedException($"Unknown container type: [{Model!.GetType().FullName}]")
        };

    public string InheritedClasses
    {
        get
        {
            var baseClassContainer = GetModel() as IBaseClassContainer;

            var lst = new List<string>();

            if (!string.IsNullOrEmpty(baseClassContainer?.BaseClass))
            {
                lst.Add(baseClassContainer.BaseClass);
            }

            lst.AddRange(Model!.Interfaces);

            return lst.Count == 0
                ? string.Empty
                : $" : {string.Join(", ", lst.Select(x => x.GetCsharpFriendlyTypeName()))}";
        }
    }

    public string GenericTypeArguments
        => GetModel().GenericTypeArguments.Count > 0
            ? $"<{string.Join(", ", Model!.GenericTypeArguments)}>"
            : string.Empty;

    public string GenericTypeArgumentConstraints
        => GetModel().GenericTypeArgumentConstraints.Count > 0
            ? string.Concat(Environment.NewLine,
                            "        ",
                            string.Join(string.Concat(Environment.NewLine, "        "), Model!.GenericTypeArgumentConstraints))
            : string.Empty;

    public string FilenamePrefix
        => string.IsNullOrEmpty(GetSettings().Path)
            ? string.Empty
            : $"{Settings.Path}{Path.DirectorySeparatorChar}";
}
