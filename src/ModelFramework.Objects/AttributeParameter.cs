using CrossCutting.Common.Extensions;

namespace ModelFramework.Objects
{
    public partial record AttributeParameter
    {
        public override string ToString() => $"{Name.WhenNullOrEmpty(() => Value.ToStringWithDefault())}";
    }
}
