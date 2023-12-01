namespace ClassFramework.TemplateFramework.ViewModels;

public class ClassConstructorViewModel : MethodViewModelBase<ClassConstructor>
{
    private readonly TypeBase _parent;

    public ClassConstructorViewModel(ClassConstructor data, CsharpClassGeneratorSettings settings, TypeBase parent, ICsharpExpressionCreator csharpExpressionCreator)
        : base(data, settings, csharpExpressionCreator)
    {
        Guard.IsNotNull(parent);

        _parent = parent;
    }

    public string Name => _parent.Name.Sanitize().GetCsharpFriendlyName();

    public string ChainCall => string.IsNullOrEmpty(Data.ChainCall)
        ? string.Empty
        : $" : {Data.ChainCall}";

    public bool OmitCode => _parent is Interface || Data.Abstract;
}
