using ClassFramework.CodeGeneration.Models.Domains;

namespace ClassFramework.CodeGeneration.Models.Abstractions;

public interface IVisibilityContainer
{
    Visibility Visibility { get; }
}
