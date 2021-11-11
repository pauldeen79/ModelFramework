using System.Diagnostics.CodeAnalysis;

namespace ModelFramework.Generators.Tests.POC
{
    [ExcludeFromCodeCoverage]
    public class ImmutableClass
    {
        public System.String Property1
        {
            get;
        }

        public System.Boolean Property2
        {
            get;
        }

        public ImmutableClass With(System.String property1 = default, System.Boolean property2 = default)
        {
            return new ImmutableClass
            (
                property1 == default ? this.Property1 : property1,
                property2 == default ? this.Property2 : property2
            );
        }

        public ImmutableClass(System.String property1, System.Boolean property2)
        {
            this.Property1 = property1;
            this.Property2 = property2;
        }
    }
}
