﻿using TextTemplateTransformationFramework.Runtime.CodeGeneration;

namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders
{
    public class DatabaseRecords : ModelFrameworkCSharpClassBase, ICodeGenerationProvider
    {
        public override string Path => "ModelFramework.Database";

        public override string DefaultFileName => "Entities.generated.cs";

        public override bool RecurseOnDeleteGeneratedFiles => false;

        public override object CreateModel()
            => GetImmutableClasses(GetDatabaseModelTypes(), "ModelFramework.Database");
    }
}