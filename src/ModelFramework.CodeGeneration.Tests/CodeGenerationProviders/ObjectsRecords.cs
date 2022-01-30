using TextTemplateTransformationFramework.Runtime.CodeGeneration;

namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders
{
    public class ObjectsRecords : ModelFrameworkCSharpClassBase, ICodeGenerationProvider
    {
        public override string Path => "ModelFramework.Objects";

        public override string DefaultFileName => "Entities.generated.cs";

        public override bool RecurseOnDeleteGeneratedFiles => false;

        public override object CreateModel()
            => GetImmutableClasses(GetObjectsModelTypes(), "ModelFramework.Objects");
    }
}
