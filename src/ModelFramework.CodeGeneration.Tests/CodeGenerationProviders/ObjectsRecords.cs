namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders
{
    public class ObjectsRecords : ModelFrameworkCSharpClassBase, ICodeGenerationProvider
    {
        public override string Prefix => "ModelFramework.Objects";

        public override string DefaultFileName => "Entities.generated.cs";

        protected override object CreateModel()
            => GetImmutableClasses(GetObjectsModelTypes(), "ModelFramework.Objects");
    }
}
