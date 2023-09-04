namespace ModelFramework.CodeGeneration;

public class MultipleContentTemplateCSharpClassGeneratorProxy : MultipleContentTemplateProxy
{
    public MultipleContentTemplateCSharpClassGeneratorProxy() : base(new CSharpClassGenerator())
    {
    }
}
