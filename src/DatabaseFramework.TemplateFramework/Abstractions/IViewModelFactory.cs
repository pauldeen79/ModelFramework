namespace DatabaseFramework.TemplateFramework.Abstractions;

public interface IViewModelFactory
{
    object Create(object model);
}
