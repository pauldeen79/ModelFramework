using System.Collections.Generic;
using System.Linq;
using ModelFramework.Objects.Builders;
using ModelFramework.Objects.Contracts;
using ModelFramework.Objects.Extensions;
using TextTemplateTransformationFramework.Runtime.CodeGeneration;

namespace ModelFramework.CodeGeneration.Tests.QueryFramework.CodeGenerationProviders
{
    public class AbstractionsInterfaces : QueryFrameworkCSharpClassBase, ICodeGenerationProvider
    {
        public override string Path => "QueryFramework.Abstractions";

        public override string DefaultFileName => "Interfaces.generated.cs";

        public override bool RecurseOnDeleteGeneratedFiles => false;

        public override object CreateModel()
            => GetModels().Select(x => x.ToInterfaceBuilder().WithPartial().Build());

        protected override IEnumerable<ClassMethodBuilder> CreateExtraOverloads(IClass c)
            => Enumerable.Empty<ClassMethodBuilder>();
    }
}
