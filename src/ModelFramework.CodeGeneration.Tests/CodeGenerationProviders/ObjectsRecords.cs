using ModelFramework.CodeGeneration.Tests.Helpers;

namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders
{
    public class ObjectsRecords : CSharpClassBase, ICodeGenerationProvider
    {
        public override object CreateModel()
            => GetImmutableClasses(GetObjectsModelTypes(), "ModelFramework.Objects");
    }
}
