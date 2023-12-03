namespace ClassFramework.TemplateFramework.ViewModels;

public abstract class AttributeContainerViewModelBase<T> : CsharpClassGeneratorViewModelBase<T>
    where T : IAttributesContainer
{
    protected AttributeContainerViewModelBase(CsharpClassGeneratorSettings settings, ICsharpExpressionCreator csharpExpressionCreator)
        : base(settings, csharpExpressionCreator)
    {
    }

    public IEnumerable<Domain.Attribute> GetAttributeModels()
        => GetModel().Attributes;
}
