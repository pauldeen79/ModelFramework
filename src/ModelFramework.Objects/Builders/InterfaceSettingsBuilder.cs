using System;
using System.Collections.Generic;
using ModelFramework.Common.Contracts;
using ModelFramework.Objects.Contracts;
using ModelFramework.Objects.Settings;

namespace ModelFramework.Objects.Builders
{
    public class InterfaceSettingsBuilder
    {
        public Func<IClassProperty, bool>? PropertyFilter { get; set; }
        public Func<IClassMethod, bool>? MethodFilter { get; set; }
        public Func<IMetadata, bool>? MetadataFilter { get; set; }
        public Func<IAttribute, bool>? AttributeFilter { get; set; }
        public IDictionary<string, string>? ApplyGenericTypes { get; set; }
        public bool ChangePropertiesToReadOnly { get; set; }

        public InterfaceSettings Build()
            => new InterfaceSettings(PropertyFilter,
                                     MethodFilter,
                                     MetadataFilter,
                                     AttributeFilter,
                                     ApplyGenericTypes,
                                     ChangePropertiesToReadOnly);

        public InterfaceSettingsBuilder Clear()
        {
            PropertyFilter = null;
            MethodFilter = null;
            MetadataFilter = null;
            AttributeFilter = null;
            ApplyGenericTypes = null;
            ChangePropertiesToReadOnly = false;
            return this;
        }

        public InterfaceSettingsBuilder WithPropertyFilter(Func<IClassProperty, bool>? propertyFilter)
        {
            PropertyFilter = propertyFilter;
            return this;
        }

        public InterfaceSettingsBuilder WithMethodFilter(Func<IClassMethod, bool>? methodFilter)
        {
            MethodFilter = methodFilter;
            return this;
        }

        public InterfaceSettingsBuilder WithMetadataFilter(Func<IMetadata, bool>? metadataFilter)
        {
            MetadataFilter = metadataFilter;
            return this;
        }

        public InterfaceSettingsBuilder WithAttributeFilter(Func<IAttribute, bool>? attributeFilter)
        {
            AttributeFilter = attributeFilter;
            return this;
        }

        public InterfaceSettingsBuilder WithApplyGenericTypes(IDictionary<string, string> applyGenericTypes)
        {
            ApplyGenericTypes = applyGenericTypes;
            return this;
        }

        public InterfaceSettingsBuilder WithChangePropertiesToReadOnly(bool changePropertiesToReadOnly = true)
        {
            ChangePropertiesToReadOnly = changePropertiesToReadOnly;
            return this;
        }

        public InterfaceSettingsBuilder AddApplyGenericTypes(string key, string value)
        {
            if (ApplyGenericTypes == null)
            {
                ApplyGenericTypes = new Dictionary<string, string>();
            }
            ApplyGenericTypes.Add(key, value);
            return this;
        }

        public InterfaceSettingsBuilder()
        {
        }

        public InterfaceSettingsBuilder(InterfaceSettings source)
        {
            PropertyFilter = source.PropertyFilter;
            MethodFilter = source.MethodFilter;
            MetadataFilter = source.MetadataFilter;
            AttributeFilter = source.AttributeFilter;
            ApplyGenericTypes = source.ApplyGenericTypes;
            ChangePropertiesToReadOnly = source.ChangePropertiesToReadOnly;
        }
    }
}
