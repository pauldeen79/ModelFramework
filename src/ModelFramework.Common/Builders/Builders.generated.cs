using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelFramework.Common.Builders
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute]
    public partial class MetadataBuilder
    {
        public object Value
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public ModelFramework.Common.Contracts.IMetadata Build()
        {
            return new ModelFramework.Common.Default.Metadata(Name, Value);
        }

        public MetadataBuilder WithValue(object value)
        {
            Value = value;
            return this;
        }

        public MetadataBuilder WithName(string name)
        {
            Name = name;
            return this;
        }

        public MetadataBuilder()
        {
            Name = string.Empty;
        }

        public MetadataBuilder(ModelFramework.Common.Contracts.IMetadata source)
        {
            Value = source.Value;
            Name = source.Name;
        }
    }
}
