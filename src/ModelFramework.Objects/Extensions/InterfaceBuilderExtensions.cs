using System.CodeDom.Compiler;
using ModelFramework.Objects.Builders;

namespace ModelFramework.Objects.Extensions
{
    public static class InterfaceBuilderExtensions
    {
        public static InterfaceBuilder AddGeneratedCodeAttribute(this InterfaceBuilder instance, string generatorName, string generatorVersion)
            => instance.AddAttributes
            (
                new AttributeBuilder()
                    .WithName(typeof(GeneratedCodeAttribute).FullName)
                    .AddParameters
                    (
                        new AttributeParameterBuilder().WithValue(generatorName),
                        new AttributeParameterBuilder().WithValue(generatorVersion)
                    )
            );
    }
}
