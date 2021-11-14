using System.CodeDom.Compiler;
using System.Diagnostics.CodeAnalysis;
using ModelFramework.Objects.Builders;

namespace ModelFramework.Objects.Extensions
{
    public static class ClassBuilderExtensions
    {
        public static ClassBuilder AddGeneratedCodeAttribute(this ClassBuilder instance, string generatorName, string generatorVersion)
            => instance.AddAttributes
            (
                new AttributeBuilder(typeof(GeneratedCodeAttribute).FullName).AddParameters
                (
                    new AttributeParameterBuilder(generatorName),
                    new AttributeParameterBuilder(generatorVersion)
                )
            );

        public static ClassBuilder AddExcludeFromCodeCoverageAttribute(this ClassBuilder instance)
            => instance.AddAttributes(new AttributeBuilder(typeof(ExcludeFromCodeCoverageAttribute).FullName));
    }
}
