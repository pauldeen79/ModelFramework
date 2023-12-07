namespace ClassFramework.TemplateFramework.ViewModels;

public class TypeBaseViewModel : AttributeContainerViewModelBase<TypeBase>
{
    public TypeBaseViewModel(ICsharpExpressionCreator csharpExpressionCreator)
        : base(csharpExpressionCreator)
    {
    }

    public bool ShouldRenderNullablePragmas
        => Settings.EnableNullableContext
        && Context.GetIndentCount() == 1; // note: only for root level, because it gets rendered in the same file

    public bool ShouldRenderNamespaceScope
        => Settings.GenerateMultipleFiles
        && !string.IsNullOrEmpty(GetModel().Namespace);

    public string Name
        => GetModel().Name.Sanitize().GetCsharpFriendlyName();

    public string Namespace
        => GetModel().Namespace;

    public string Modifiers
        => GetModel().GetModifiers(Settings.CultureInfo);

    public IReadOnlyCollection<string> SuppressWarningCodes
        => GetModel().SuppressWarningCodes;

    public CodeGenerationHeaderViewModel GetCodeGenerationHeaderModel()
        => new CodeGenerationHeaderViewModel(CsharpExpressionCreator) { Model = new CodeGenerationHeaderModel(Settings.CreateCodeGenerationHeader, Settings.EnvironmentVersion), Settings = Settings };

    public UsingsViewModel GetUsingsModel()
        => new UsingsViewModel(CsharpExpressionCreator) { Model = new UsingsModel([GetModel()]), Settings = Settings };

    public IEnumerable<object> GetMemberModels()
    {
        var items = new List<object?>();

        var fieldsContainer = GetModel() as IFieldsContainer;
        if (fieldsContainer is not null) items.AddRange(fieldsContainer.Fields.Select(x => new ClassFieldViewModel(CsharpExpressionCreator) { Model = x, Settings = Settings }));

        items.AddRange(Model!.Properties.Select(x => new ClassPropertyViewModel(CsharpExpressionCreator) { Model = x, Settings = Settings }));

        var constructorsContainer = Model as IConstructorsContainer;
        if (constructorsContainer is not null) items.AddRange(constructorsContainer.Constructors.Select(x => new ClassConstructorViewModel(CsharpExpressionCreator) { Model = x, Settings = Settings }));

        items.AddRange(Model.Methods.Select(x => new ClassMethodViewModel(CsharpExpressionCreator) { Model = x, Settings = Settings }));

        // Quirk, enums as items below a class. There is no interface for this right now.
        var cls = Model as Class;
        if (cls is not null) items.AddRange(cls.Enums.Select(x => new EnumerationViewModel(CsharpExpressionCreator) { Model = x, Settings = Settings }));

        // Add separators (empty lines) between each item
        return items.SelectMany((item, index) => index + 1 < items.Count ? [item!, new NewLineViewModel(CsharpExpressionCreator) { Model = new NewLineModel(), Settings = Settings }] : new object[] { item! });
    }

    public IEnumerable<object> GetSubClassModels()
    {
        var subClasses = (GetModel() as Class)?.SubClasses;
        if (subClasses is null)
        {
            return Enumerable.Empty<object>();
        }

        return subClasses
            .Select(x => new TypeBaseViewModel(CsharpExpressionCreator) { Model = x, Settings = Settings })
            .SelectMany(item => new object[] { new NewLineViewModel(CsharpExpressionCreator) { Model = new NewLineModel(), Settings = Settings }, item });
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

    public string FilenamePrefix
        => string.IsNullOrEmpty(Settings.Path)
            ? string.Empty
            : Settings.Path + Path.DirectorySeparatorChar;
}
