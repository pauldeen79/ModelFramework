namespace ClassFramework.TemplateFramework.ViewModels;

public class CsharpClassGeneratorViewModel : CsharpClassGeneratorViewModelBase<IEnumerable<TypeBase>>
{
    public CsharpClassGeneratorViewModel(ICsharpExpressionCreator csharpExpressionCreator)
        : base(csharpExpressionCreator)
    {
    }

    public IOrderedEnumerable<IGrouping<string, TypeBase>> Namespaces
        => GetModel().GroupBy(x => x.Namespace).OrderBy(x => x.Key);

    public CodeGenerationHeaderViewModel GetCodeGenerationHeaderModel()
        => new CodeGenerationHeaderViewModel();

    public UsingsModel GetUsingsModel()
        => new UsingsModel(GetModel());

    public IEnumerable<TypeBase> GetTypeBaseModels(IEnumerable<TypeBase> @namespace)
        => @namespace.OrderBy(typeBase => typeBase.Name);
}
