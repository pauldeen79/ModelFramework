namespace ClassFramework.TemplateFramework.ViewModels;

public class CsharpClassGeneratorViewModel : CsharpClassGeneratorViewModelBase<IEnumerable<TypeBase>>
{
    public CsharpClassGeneratorViewModel(CsharpClassGeneratorSettings settings, ICsharpExpressionCreator csharpExpressionCreator)
        : base(settings, csharpExpressionCreator)
    {
    }

    public CodeGenerationHeaderViewModel GetCodeGenerationHeaderModel()
        => new CodeGenerationHeaderViewModel(Settings);

    public UsingsViewModel GetUsingsModel()
    {
        Guard.IsNotNull(Model);

        return new UsingsViewModel(Settings, CsharpExpressionCreator)
        {
            Model = Model,
            Context = Context.CreateChildContext(new ChildTemplateContext(new EmptyTemplateIdentifier(), Model))
        };
    }

    public IEnumerable<TypeBaseViewModel> GetTypeBaseModels(IEnumerable<TypeBase> @namespace)
        => @namespace
            .OrderBy(typeBase => typeBase.Name)
            .Select((typeBase, index) => new TypeBaseViewModel(Settings, CsharpExpressionCreator)
            {
                Model = typeBase,
                Context = Context.CreateChildContext(new ChildTemplateContext(new EmptyTemplateIdentifier(), typeBase, index, @namespace.Count()))
            });
}
