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
        => new CodeGenerationHeaderViewModel(CsharpExpressionCreator) { Model = new CodeGenerationHeaderModel(Settings.CreateCodeGenerationHeader, Settings.EnvironmentVersion), Settings = Settings };

    public UsingsViewModel GetUsingsModel()
        => new UsingsViewModel(CsharpExpressionCreator) { Model = new UsingsModel(GetModel()), Settings = Settings };

    public IEnumerable<TypeBaseViewModel> GetTypeBaseModels(IEnumerable<TypeBase> @namespace)
        => @namespace.OrderBy(typeBase => typeBase.Name).Select(x => new TypeBaseViewModel(CsharpExpressionCreator) { Model = x, Settings = Settings });
}
