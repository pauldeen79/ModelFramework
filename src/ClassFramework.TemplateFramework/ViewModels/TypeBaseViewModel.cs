namespace ClassFramework.TemplateFramework.ViewModels;

public class TypeBaseViewModel : CsharpClassGeneratorViewModel<TypeBase>
{
    private readonly ICsharpExpressionCreator _csharpExpressionCreator;

    public TypeBaseViewModel(TypeBase data, CsharpClassGeneratorSettings settings, ICsharpExpressionCreator csharpExpressionCreator) : base(data, settings)
    {
        Guard.IsNotNull(csharpExpressionCreator);
        _csharpExpressionCreator = csharpExpressionCreator;
    }

    public bool ShouldRenderNullablePragmas
        => Settings.EnableNullableContext && Settings.IndentCount == 1; // note: only for root level, because it gets rendered in the same file

    public bool ShouldRenderNamespaceScope
        => Settings.GenerateMultipleFiles && !string.IsNullOrEmpty(Data.Namespace);

    public string Name => Data.Name.Sanitize().GetCsharpFriendlyName();

    public IEnumerable<CsharpClassGeneratorViewModel> GetMemberModels()
    {
        var items = new List<CsharpClassGeneratorViewModel>();

        var fieldsContainer = Data as IFieldsContainer;
        if (fieldsContainer is not null) items.AddRange(fieldsContainer.Fields.Select(x => new ClassFieldViewModel(x, Settings, _csharpExpressionCreator)));

        items.AddRange(Data.Properties.Select(x => new ClassPropertyViewModel(x, Settings)));

        var constructorsContainer = Data as IConstructorsContainer;
        if (constructorsContainer is not null) items.AddRange(constructorsContainer.Constructors.Select(x => new ClassConstructorViewModel(x, Settings, Data, _csharpExpressionCreator)));

        items.AddRange(Data.Methods.Select(x => new ClassMethodViewModel(x, Settings, Data, _csharpExpressionCreator)));

        // Quirk, enums as items below a class. There is no interface for this right now.
        var cls = Data as Class;
        if (cls is not null) items.AddRange(cls.Enums.Select(x => new EnumerationViewModel(x, Settings)));

        // Add separators (empty lines) between each item
        return items.SelectMany((item, index) => index + 1 < items.Count ? [item, new NewLineViewModel(Settings)] : new CsharpClassGeneratorViewModel[] { item });
    }

    public IEnumerable<CsharpClassGeneratorViewModel> GetSubClassModels()
    {
        var subClasses = (Data as Class)?.SubClasses;
        if (subClasses is null)
        {
            return Enumerable.Empty<CsharpClassGeneratorViewModel>();
        }

        return subClasses
            .Select(typeBase => new TypeBaseViewModel(typeBase, Settings.ForSubclasses(), _csharpExpressionCreator))
            .SelectMany((item, index) => index + 1 < subClasses.Count ? [item, new NewLineViewModel(Settings)] : new CsharpClassGeneratorViewModel[] { item });
    }

    public string GetContainerType()
        => Data switch
        {
            Class cls when cls.Record => "record",
            Class cls when !cls.Record => "class",
            Struct str when str.Record => "record struct",
            Struct str when !str.Record => "struct",
            Interface => "interface",
            _ => throw new InvalidOperationException($"Unknown container type: [{Data.GetType().FullName}]")
        };

    public string GetInheritedClasses()
    {
        var lst = new List<string>();

        var baseClassContainer = Data as IBaseClassContainer;
        if (!string.IsNullOrEmpty(baseClassContainer?.BaseClass))
        {
            lst.Add(baseClassContainer.BaseClass);
        }

        lst.AddRange(Data.Interfaces);

        return lst.Count == 0
            ? string.Empty
            : $" : {string.Join(", ", lst.Select(x => x.GetCsharpFriendlyTypeName()))}";
    }

    public IEnumerable GetAttributeModels()
        => Data.Attributes.Select(attribute => new AttributeViewModel(attribute, Settings, _csharpExpressionCreator, Data));
}
