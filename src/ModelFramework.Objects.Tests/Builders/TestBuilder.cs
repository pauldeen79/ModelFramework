using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelFramework.Objects.Tests.Builders
{
//#nullable enable
    public partial class TestBuilder
    {
        private Lazy<object> _valueDelegate;
        public object Value //note that single properties should be optional.
        {
            get { return _valueDelegate.Value; }
            set { _valueDelegate = new Lazy<object>(() => value); } // target-typed new expression
        }

        public System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder> Metadata //note that collection properties should always be generated
        {
            get;
            set;
        }

        private Lazy<string> _nameDelegate;
        public string Name
        {
            get { return _nameDelegate.Value; }
            set { _nameDelegate = new (() => value); }
        }

        public ModelFramework.Objects.Contracts.IAttributeParameter Build()
        {
            return new ModelFramework.Objects.AttributeParameter(_valueDelegate.Value, Metadata.Select(x => x.Build()), _nameDelegate.Value);
        }

        public TestBuilder WithValue(object value)
        {
            _valueDelegate = new Lazy<object>(() => value);
            return this;
        }

        public TestBuilder WithValue(Func<object> valueDelegate)
        {
            _valueDelegate = new Lazy<object>(valueDelegate);
            return this;
        }

        public TestBuilder AddMetadata(System.Collections.Generic.IEnumerable<ModelFramework.Common.Builders.MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }

        public TestBuilder AddMetadata(params ModelFramework.Common.Builders.MetadataBuilder[] metadata)
        {
            Metadata.AddRange(metadata);
            return this;
        }

        public TestBuilder AddMetadata(string name, object? value)
        {
            AddMetadata(new ModelFramework.Common.Builders.MetadataBuilder().WithName(name).WithValue(value));
            return this;
        }

        public TestBuilder WithName(string name)
        {
            _nameDelegate = new Lazy<string>(() => name);
            return this;
        }

        public TestBuilder WithName(Func<string> nameDelegate)
        {
            _nameDelegate = new Lazy<string>(nameDelegate);
            return this;
        }

        public TestBuilder()
        {
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            _valueDelegate = new Lazy<object>(() => new object());
            _nameDelegate = new (() => string.Empty); // target-typed new expression
        }

        public TestBuilder(ModelFramework.Objects.Contracts.IAttributeParameter source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            Metadata.AddRange(source.Metadata.Select(x => new ModelFramework.Common.Builders.MetadataBuilder(x)));
            _valueDelegate = new Lazy<object>(() => source.Value);
            _nameDelegate = new (() => source.Name); // target-typed new expression
        }
    }
//#nullable restore
}
