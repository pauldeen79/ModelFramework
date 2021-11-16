using System.CodeDom.Compiler;
using ModelFramework.Objects.Builders;

namespace ModelFramework.Objects.Extensions
{
    public static class InterfaceBuilderExtensions
    {
        public static InterfaceBuilder AddGeneratedCodeAttribute(this InterfaceBuilder instance, string generatorName, string generatorVersion)
            => instance.AddAttributes
            (
                new AttributeBuilder { Name = typeof(GeneratedCodeAttribute).FullName }.AddParameters
                (
                    new AttributeParameterBuilder { Value = generatorName },
                    new AttributeParameterBuilder { Value = generatorVersion }
                )
            );
    }
}
