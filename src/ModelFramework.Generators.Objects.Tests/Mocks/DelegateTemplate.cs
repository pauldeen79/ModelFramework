namespace ModelFramework.Generators.Objects.Tests.Mocks;

public class DelegateTemplate : CSharpClassGeneratorBase
{
    public Action<StringBuilder> RenderDelegate { get; set; } = new Action<StringBuilder>(_ => { });

    public virtual void Render(StringBuilder builder) => RenderDelegate.Invoke(builder);
}
