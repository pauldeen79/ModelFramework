using ModelFramework.Common.Contracts;
using System;

namespace ModelFramework.Common.Default
{
    public record Metadata : IMetadata
    {
        public Metadata(string name, object value)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentOutOfRangeException(nameof(name), "Name cannot be null or whitespace");
            Name = name;
            Value = value;
        }

        public object Value { get; }

        public string Name { get; }

        public override string ToString() => $"[{Name}] = [{Value}]";
    }
}
