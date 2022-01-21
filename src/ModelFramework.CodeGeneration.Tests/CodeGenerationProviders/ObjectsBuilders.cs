using ModelFramework.CodeGeneration.Tests.Helpers;

namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders
{
    public class ObjectsBuilders : CSharpClassBase, ICodeGenerationProvider
    {
        public override string Prefix => "ModelFramework.Objects\\Builders";

        public override string DefaultFileName => "Builders.generated.cs";

        public override object CreateModel()
            => GetImmutableBuilderClasses(GetObjectsModelTypes(),
                                          "ModelFramework.Objects",
                                          "ModelFramework.Objects.Builders");
    }
}
