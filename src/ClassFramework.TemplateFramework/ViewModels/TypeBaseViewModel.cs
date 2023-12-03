namespace ClassFramework.TemplateFramework.ViewModels;

public class TypeBaseViewModel : AttributeContainerViewModelBase<TypeBase>
{
    public TypeBaseViewModel(ICsharpExpressionCreator csharpExpressionCreator)
        : base(csharpExpressionCreator)
    {
    }

    public bool ShouldRenderNullablePragmas
        => Settings.EnableNullableContext && Settings.IndentCount == 1; // note: only for root level, because it gets rendered in the same file

    public bool ShouldRenderNamespaceScope
        => Settings.GenerateMultipleFiles && !string.IsNullOrEmpty(GetModel().Namespace);

    public string Name
        => GetModel().Name.Sanitize().GetCsharpFriendlyName();

    public string Namespace
        => GetModel().Namespace;

    public string Modifiers
        => GetModel().GetModifiers();

    public IReadOnlyCollection<string> SuppressWarningCodes
        => GetModel().SuppressWarningCodes;

    public CodeGenerationHeaderModel GetCodeGenerationHeaderModel()
        => new CodeGenerationHeaderModel(Settings.CreateCodeGenerationHeader, Settings.EnvironmentVersion);

    public UsingsModel GetUsingsModel()
        => new UsingsModel([GetModel()]);

    public IEnumerable GetMemberModels()
    {
        var items = new List<object?>();

        var fieldsContainer = GetModel() as IFieldsContainer;
        if (fieldsContainer is not null) items.AddRange(fieldsContainer.Fields);

        items.AddRange(Model!.Properties);

        var constructorsContainer = Model as IConstructorsContainer;
        if (constructorsContainer is not null) items.AddRange(constructorsContainer.Constructors);

        items.AddRange(Model.Methods);

        // Quirk, enums as items below a class. There is no interface for this right now.
        var cls = Model as Class;
        if (cls is not null) items.AddRange(cls.Enums);

        // Add separators (empty lines) between each item
        return items.SelectMany((item, index) => index + 1 < items.Count ? [item!, new NewLineModel()] : new object[] { item! });
    }

    public IEnumerable GetSubClassModels()
    {
        var subClasses = (GetModel() as Class)?.SubClasses;
        if (subClasses is null)
        {
            return Enumerable.Empty<object>();
        }

        return subClasses
            .SelectMany((item, index) => index + 1 < subClasses.Count ? [item, new NewLineModel()] : new object[] { item });
    }

    public string ContainerType
        => GetModel() switch
        {
            Class cls when cls.Record => "record",
            Class cls when !cls.Record => "class",
            Struct str when str.Record => "record struct",
            Struct str when !str.Record => "struct",
            Interface => "interface",
            _ => throw new InvalidOperationException($"Unknown container type: [{Model!.GetType().FullName}]")
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
}

public class TypeBaseViewModelFactoryComponent : IViewModel
{
    private readonly ICsharpExpressionCreator _csharpExpressionCreator;

    public TypeBaseViewModelFactoryComponent(ICsharpExpressionCreator csharpExpressionCreator)
    {
        Guard.IsNotNull(csharpExpressionCreator);

        _csharpExpressionCreator = csharpExpressionCreator;
    }

    public object Create()
        => new TypeBaseViewModel(_csharpExpressionCreator);

    public bool Supports(object model)
        => model is TypeBase;
}
