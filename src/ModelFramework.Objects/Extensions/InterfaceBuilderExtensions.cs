using System.CodeDom.Compiler;
using ModelFramework.Objects.Builders;

namespace ModelFramework.Objects.Extensions
{
    public static class InterfaceBuilderExtensions
    {
        public static InterfaceBuilder AddGeneratedCodeAttribute(this InterfaceBuilder instance, string generatorName, string generatorVersion)
            => instance.AddAttributes
            (
                new AttributeBuilder(typeof(GeneratedCodeAttribute).FullName).AddParameters
                (
                    new AttributeParameterBuilder(generatorName),
                    new AttributeParameterBuilder(generatorVersion)
                )
            );
    }
}
