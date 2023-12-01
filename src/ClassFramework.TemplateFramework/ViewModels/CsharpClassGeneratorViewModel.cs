namespace ClassFramework.TemplateFramework.ViewModels;

public class CsharpClassGeneratorViewModel : CsharpClassGeneratorViewModelBase<IEnumerable<TypeBase>>
{
    public CsharpClassGeneratorViewModel(CsharpClassGeneratorSettings settings, ICsharpExpressionCreator csharpExpressionCreator)
        : base(settings, csharpExpressionCreator)
    {
    }

    public IOrderedEnumerable<IGrouping<string, TypeBase>> Namespaces
        => GetModel().GroupBy(x => x.Namespace).OrderBy(x => x.Key);

    public CodeGenerationHeaderViewModel GetCodeGenerationHeaderModel()
        => new CodeGenerationHeaderViewModel(Settings);

    public UsingsViewModel GetUsingsModel()
        => new UsingsViewModel(Settings, CsharpExpressionCreator)
        {
            Model = GetModel(),
            Context = Context.CreateChildContext(new ChildTemplateContext(new EmptyTemplateIdentifier(), Model))
        };

    public IEnumerable<TypeBaseViewModel> GetTypeBaseModels(IEnumerable<TypeBase> @namespace)
        => @namespace
            .OrderBy(typeBase => typeBase.Name)
            .Select((typeBase, index) => new TypeBaseViewModel(Settings, CsharpExpressionCreator)
            {
                Model = typeBase,
                Context = Context.CreateChildContext(new ChildTemplateContext(new EmptyTemplateIdentifier(), typeBase, index, @namespace.Count()))
            });
}
