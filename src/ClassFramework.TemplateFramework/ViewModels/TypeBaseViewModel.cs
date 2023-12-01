namespace ClassFramework.TemplateFramework.ViewModels;

public class TypeBaseViewModel : AttributeContainerViewModelBase<TypeBase>
{
    public TypeBaseViewModel(TypeBase data, CsharpClassGeneratorSettings settings, ICsharpExpressionCreator csharpExpressionCreator)
        : base(data, settings, csharpExpressionCreator)
    {
    }

    public bool ShouldRenderNullablePragmas
        => Settings.EnableNullableContext && Settings.IndentCount == 1; // note: only for root level, because it gets rendered in the same file

    public bool ShouldRenderNamespaceScope
        => Settings.GenerateMultipleFiles && !string.IsNullOrEmpty(Data.Namespace);

    public string Name => Data.Name.Sanitize().GetCsharpFriendlyName();

    public CodeGenerationHeaderViewModel GetCodeGenerationHeaderModel() => new CodeGenerationHeaderViewModel(Settings);

    public UsingsViewModel GetUsingsModel() => new UsingsViewModel(new[] { Data }, Settings, CsharpExpressionCreator);

    public IEnumerable<CsharpClassGeneratorViewModelBase> GetMemberModels()
    {
        var items = new List<CsharpClassGeneratorViewModelBase>();

        var fieldsContainer = Data as IFieldsContainer;
        if (fieldsContainer is not null) items.AddRange(fieldsContainer.Fields.Select(x => new ClassFieldViewModel(x, Settings, CsharpExpressionCreator)));

        items.AddRange(Data.Properties.Select(x => new ClassPropertyViewModel(x, Settings, CsharpExpressionCreator)));

        var constructorsContainer = Data as IConstructorsContainer;
        if (constructorsContainer is not null) items.AddRange(constructorsContainer.Constructors.Select(x => new ClassConstructorViewModel(x, Settings, Data, CsharpExpressionCreator)));

        items.AddRange(Data.Methods.Select(x => new ClassMethodViewModel(x, Settings, Data, CsharpExpressionCreator)));

        // Quirk, enums as items below a class. There is no interface for this right now.
        var cls = Data as Class;
        if (cls is not null) items.AddRange(cls.Enums.Select(x => new EnumerationViewModel(x, Settings, CsharpExpressionCreator)));

        // Add separators (empty lines) between each item
        return items.SelectMany((item, index) => index + 1 < items.Count ? [item, new NewLineViewModel(Settings)] : new CsharpClassGeneratorViewModelBase[] { item });
    }

    public IEnumerable<CsharpClassGeneratorViewModelBase> GetSubClassModels()
    {
        var subClasses = (Data as Class)?.SubClasses;
        if (subClasses is null)
        {
            return Enumerable.Empty<CsharpClassGeneratorViewModelBase>();
        }

        return subClasses
            .Select(typeBase => new TypeBaseViewModel(typeBase, Settings.ForSubclasses(), CsharpExpressionCreator))
            .SelectMany((item, index) => index + 1 < subClasses.Count ? [item, new NewLineViewModel(Settings)] : new CsharpClassGeneratorViewModelBase[] { item });
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
}
