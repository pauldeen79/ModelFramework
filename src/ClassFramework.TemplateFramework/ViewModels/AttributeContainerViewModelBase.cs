namespace ClassFramework.TemplateFramework.ViewModels;

public abstract class AttributeContainerViewModelBase<T> : CsharpClassGeneratorViewModel<T>
    where T : IAttributesContainer
{

    protected AttributeContainerViewModelBase(T data, CsharpClassGeneratorSettings settings, ICsharpExpressionCreator csharpExpressionCreator) : base(data, settings, csharpExpressionCreator)
    {
    }

    public IEnumerable<CsharpClassGeneratorViewModelBase> GetAttributeModels()
        => Data.Attributes.Select(attribute => new AttributeViewModel(attribute, Settings, CsharpExpressionCreator, Data));
}
