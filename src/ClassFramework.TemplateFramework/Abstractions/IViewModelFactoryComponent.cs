namespace ClassFramework.TemplateFramework.Abstractions;

public interface IViewModelFactoryComponent
{
    bool Supports(object model);
    object Create();
}
