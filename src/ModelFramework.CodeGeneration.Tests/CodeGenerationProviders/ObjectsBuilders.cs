namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders
{
    public class ObjectsBuilders : ModelFrameworkCSharpClassBase, ICodeGenerationProvider
    {
        public override string Prefix => "ModelFramework.Objects\\Builders";

        public override string DefaultFileName => "Builders.generated.cs";

        protected override object CreateModel()
            => GetImmutableBuilderClasses(GetObjectsModelTypes(),
                                          "ModelFramework.Objects",
                                          "ModelFramework.Objects.Builders");
    }
}
