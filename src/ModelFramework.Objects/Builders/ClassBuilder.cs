using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ModelFramework.Common.Builders;

namespace ModelFramework.Objects.Builders
{
    public partial class ClassBuilder
    {
        public ClassBuilder WithBaseClass(Type baseClassType)
            => WithBaseClass(baseClassType.FullName);

        public ClassBuilder AddUsings(params string[] usings)
            => AddMetadata(usings.Select(x => new MetadataBuilder().WithName(MetadataNames.CustomUsing).WithValue(x)));

        public ClassBuilder AddInterfaces(params Type[] types)
            => AddInterfaces(types.Select(x => x.FullName));

        public ClassBuilder AddGeneratedCodeAttribute(string generatorName, string generatorVersion)
            => AddAttributes(new AttributeBuilder().ForCodeGenerator(generatorName, generatorVersion));

        public ClassBuilder AddExcludeFromCodeCoverageAttribute()
            => AddAttributes(new AttributeBuilder().WithName(typeof(ExcludeFromCodeCoverageAttribute).FullName));
    }
}
