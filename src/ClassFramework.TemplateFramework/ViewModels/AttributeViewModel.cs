﻿namespace ClassFramework.TemplateFramework.ViewModels;

public class AttributeViewModel : CsharpClassGeneratorViewModelBase<Domain.Attribute>
{
    public AttributeViewModel(ICsharpExpressionCreator csharpExpressionCreator)
        : base(csharpExpressionCreator)
    {
    }

    public bool IsSingleLineAttributeContainer => GetParentModel() is Parameter;

    public string Name
        => GetModel().Name;

    public string Parameters
        => GetModel().Parameters.Count == 0
            ? string.Empty
            : string.Concat("(", string.Join(", ", Model!.Parameters.Select(p =>
                string.IsNullOrEmpty(p.Name)
                    ? CsharpExpressionCreator.Create(p.Value)
                    : string.Format("{0} = {1}", p.Name, CsharpExpressionCreator.Create(p.Value))
            )), ")");

    public int AdditionalIndents
    {
        get
        {
            if (IsSingleLineAttributeContainer || GetParentModel() is TypeBase)
            {
                return 0;
            }

            return 1;
        }
    }
}

public class AttributeViewModelFactoryComponent : IViewModelFactoryComponent
{
    private readonly ICsharpExpressionCreator _csharpExpressionCreator;

    public AttributeViewModelFactoryComponent(ICsharpExpressionCreator csharpExpressionCreator)
    {
        Guard.IsNotNull(csharpExpressionCreator);

        _csharpExpressionCreator = csharpExpressionCreator;
    }

    public object Create()
        => new AttributeViewModel(_csharpExpressionCreator);

    public bool Supports(object model)
        => model is Domain.Attribute;
}
