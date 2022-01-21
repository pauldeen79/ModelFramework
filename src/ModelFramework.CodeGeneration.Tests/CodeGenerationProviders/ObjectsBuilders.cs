using ModelFramework.CodeGeneration.Tests.Helpers;

namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders
{
    public class ObjectsBuilders : CSharpClassBase, ICodeGenerationProvider
    {
        public override object CreateModel()
            => GetImmutableBuilderClasses(GetObjectsModelTypes(),
                                          "ModelFramework.Objects",
                                          "ModelFramework.Objects.Builders");
    }
}
