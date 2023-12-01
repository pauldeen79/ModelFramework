namespace ClassFramework.TemplateFramework.ViewModels;

public abstract class AttributeContainerViewModelBase<T> : CsharpClassGeneratorViewModelBase<T>
    where T : IAttributesContainer
{

    protected AttributeContainerViewModelBase(CsharpClassGeneratorSettings settings, ICsharpExpressionCreator csharpExpressionCreator)
        : base(settings, csharpExpressionCreator)
    {
    }

    public IEnumerable<CsharpClassGeneratorViewModelBase> GetAttributeModels()
        => GetModel().Attributes.Select((attribute, index) => new AttributeViewModel(Settings, CsharpExpressionCreator)
        {
            Model = attribute,
            Context = Context.CreateChildContext(new ChildTemplateContext(new EmptyTemplateIdentifier(), attribute, index, Model!.Attributes.Count))
        });
}
