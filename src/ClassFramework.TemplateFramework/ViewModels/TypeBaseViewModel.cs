namespace ClassFramework.TemplateFramework.ViewModels;

public class TypeBaseViewModel : AttributeContainerViewModelBase<TypeBase>
{
    public TypeBaseViewModel(CsharpClassGeneratorSettings settings, ICsharpExpressionCreator csharpExpressionCreator)
        : base(settings, csharpExpressionCreator)
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

    public CodeGenerationHeaderViewModel GetCodeGenerationHeaderModel()
        => new CodeGenerationHeaderViewModel(Settings);

    public UsingsViewModel GetUsingsModel()
        => new UsingsViewModel(Settings, CsharpExpressionCreator)
        {
            Model = [GetModel()],
            Context = Context.CreateChildContext(new ChildTemplateContext(new EmptyTemplateIdentifier(), Model))
        };

    public IEnumerable<CsharpClassGeneratorViewModelBase> GetMemberModels()
    {
        var items = new List<CsharpClassGeneratorViewModelBase>();

        var fieldsContainer = GetModel() as IFieldsContainer;
        if (fieldsContainer is not null) items.AddRange(fieldsContainer.Fields.Select((field, index) => new ClassFieldViewModel(Settings, CsharpExpressionCreator)
        {
            Model = field,
            Context = Context.CreateChildContext(new ChildTemplateContext(new EmptyTemplateIdentifier(), field, index, fieldsContainer.Fields.Count))
        }));

        items.AddRange(Model!.Properties.Select((property, index) => new ClassPropertyViewModel(Settings, CsharpExpressionCreator)
        {
            Model = property,
            Context = Context.CreateChildContext(new ChildTemplateContext(new EmptyTemplateIdentifier(), property, index, Model.Properties.Count))
        }));

        var constructorsContainer = Model as IConstructorsContainer;
        if (constructorsContainer is not null) items.AddRange(constructorsContainer.Constructors.Select((ctor, index) => new ClassConstructorViewModel(Settings, CsharpExpressionCreator)
        {
            Model = ctor,
            Context = Context.CreateChildContext(new ChildTemplateContext(new EmptyTemplateIdentifier(), ctor, index, constructorsContainer.Constructors.Count))
        }));

        items.AddRange(Model.Methods.Select((method, index) => new ClassMethodViewModel(Settings, CsharpExpressionCreator)
        {
            Model = method,
            Context = Context.CreateChildContext(new ChildTemplateContext(new EmptyTemplateIdentifier(), method, index, Model.Methods.Count))
        }));

        // Quirk, enums as items below a class. There is no interface for this right now.
        var cls = Model as Class;
        if (cls is not null) items.AddRange(cls.Enums.Select((enumeration, index) => new EnumerationViewModel(Settings, CsharpExpressionCreator)
        {
            Model = enumeration,
            Context = Context.CreateChildContext(new ChildTemplateContext(new EmptyTemplateIdentifier(), enumeration, index, cls.Enums.Count))
        }));

        // Add separators (empty lines) between each item
        return items.SelectMany((item, index) => index + 1 < items.Count ? [item, new NewLineViewModel(Settings)] : new CsharpClassGeneratorViewModelBase[] { item });
    }

    public IEnumerable<CsharpClassGeneratorViewModelBase> GetSubClassModels()
    {
        var subClasses = (GetModel() as Class)?.SubClasses;
        if (subClasses is null)
        {
            return Enumerable.Empty<CsharpClassGeneratorViewModelBase>();
        }

        return subClasses
            .Select((typeBase, index) => new TypeBaseViewModel(Settings.ForSubclasses(), CsharpExpressionCreator)
            {
                Model = typeBase,
                Context = Context.CreateChildContext(new ChildTemplateContext(new EmptyTemplateIdentifier(), typeBase, index, subClasses.Count))
            })
            .SelectMany((item, index) => index + 1 < subClasses.Count ? [item, new NewLineViewModel(Settings)] : new CsharpClassGeneratorViewModelBase[] { item });
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
