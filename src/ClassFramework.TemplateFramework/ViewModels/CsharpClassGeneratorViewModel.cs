namespace ClassFramework.TemplateFramework.ViewModels;

public class CsharpClassGeneratorViewModel : CsharpClassGeneratorViewModelBase<IEnumerable<TypeBase>>
{
    public CsharpClassGeneratorViewModel(IEnumerable<TypeBase> data, CsharpClassGeneratorSettings settings, ICsharpExpressionCreator csharpExpressionCreator)
        : base(data, settings, csharpExpressionCreator)
    {
    }

    public CodeGenerationHeaderViewModel GetCodeGenerationHeaderModel()
        => new CodeGenerationHeaderViewModel(Settings);

    public UsingsViewModel GetUsingsModel()
        => new UsingsViewModel(Data, Settings, CsharpExpressionCreator);

    public IEnumerable<TypeBaseViewModel> GetTypeBaseModels(IEnumerable<TypeBase> @namespace)
        => @namespace
            .OrderBy(typeBase => typeBase.Name)
            .Select(typeBase => new TypeBaseViewModel(typeBase, Settings, CsharpExpressionCreator));
}
