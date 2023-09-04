namespace ModelFramework.CodeGeneration;

public class SingleContentTemplateCSharpClassGeneratorProxy : SingleContentTemplateProxy
{
    public SingleContentTemplateCSharpClassGeneratorProxy() : base(new CSharpClassGenerator())
    {
    }
}
