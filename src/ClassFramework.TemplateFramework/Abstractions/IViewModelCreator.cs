namespace ClassFramework.TemplateFramework.Abstractions;

public interface IViewModelCreator
{
    bool Supports(object model);
    object Create(object model, CsharpClassGeneratorSettings settings);
}
