using CrossCutting.Common.Extensions;

namespace ModelFramework.Objects.Default
{
    public partial record AttributeParameter
    {
        public override string ToString() => $"{Name.WhenNullOrEmpty(() => Value.ToStringWithDefault())}";
    }
}
