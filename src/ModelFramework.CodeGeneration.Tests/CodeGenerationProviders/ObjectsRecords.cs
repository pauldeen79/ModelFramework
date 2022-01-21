using ModelFramework.CodeGeneration.Tests.Helpers;

namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders
{
    public class ObjectsRecords : CSharpClassBase, ICodeGenerationProvider
    {
        public override string Prefix => "ModelFramework.Objects";

        public override string DefaultFileName => "Entities.generated.cs";

        public override object CreateModel()
            => GetImmutableClasses(GetObjectsModelTypes(), "ModelFramework.Objects");
    }
}
