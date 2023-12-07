namespace ClassFramework.TemplateFramework.ViewModels;

public abstract class AttributeContainerViewModelBase<T> : CsharpClassGeneratorViewModelBase<T>
    where T : IAttributesContainer
{
    protected AttributeContainerViewModelBase(ICsharpExpressionCreator csharpExpressionCreator)
        : base(csharpExpressionCreator)
    {
    }

    public IEnumerable<AttributeViewModel> GetAttributeModels()
        => GetModel().Attributes.Select(x => new AttributeViewModel(CsharpExpressionCreator) { Model = x });
}
