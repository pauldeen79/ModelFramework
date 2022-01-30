﻿using TextTemplateTransformationFramework.Runtime.CodeGeneration;

namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders
{
    public class ObjectsBuilders : ModelFrameworkCSharpClassBase, ICodeGenerationProvider
    {
        public override string Path => "ModelFramework.Objects\\Builders";

        public override string DefaultFileName => "Builders.generated.cs";

        public override bool RecurseOnDeleteGeneratedFiles => false;

        public override object CreateModel()
            => GetImmutableBuilderClasses(GetObjectsModelTypeBases(),
                                          "ModelFramework.Objects",
                                          "ModelFramework.Objects.Builders");
    }
}
