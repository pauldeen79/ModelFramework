﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 6.0.2
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelFramework.Database.Builders
{
#nullable enable
    public partial class PrimaryKeyConstraintFieldBuilder
    {
        public bool IsDescending
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder> Metadata
        {
            get;
            set;
        }

        public ModelFramework.Database.Contracts.IPrimaryKeyConstraintField Build()
        {
            return new ModelFramework.Database.PrimaryKeyConstraintField(IsDescending, Name, Metadata.Select(x => x.Build()));
        }

        public PrimaryKeyConstraintFieldBuilder WithIsDescending(bool isDescending = true)
        {
            IsDescending = isDescending;
            return this;
        }

        public PrimaryKeyConstraintFieldBuilder WithName(string name)
        {
            Name = name;
            return this;
        }

        public PrimaryKeyConstraintFieldBuilder AddMetadata(System.Collections.Generic.IEnumerable<ModelFramework.Common.Builders.MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }

        public PrimaryKeyConstraintFieldBuilder AddMetadata(params ModelFramework.Common.Builders.MetadataBuilder[] metadata)
        {
            Metadata.AddRange(metadata);
            return this;
        }

        public PrimaryKeyConstraintFieldBuilder AddMetadata(string name, object? value)
        {
            AddMetadata(new ModelFramework.Common.Builders.MetadataBuilder().WithName(name).WithValue(value));
            return this;
        }

        public PrimaryKeyConstraintFieldBuilder()
        {
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            IsDescending = default;
            Name = string.Empty;
        }

        public PrimaryKeyConstraintFieldBuilder(ModelFramework.Database.Contracts.IPrimaryKeyConstraintField source)
        {
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            IsDescending = source.IsDescending;
            Name = source.Name;
            Metadata.AddRange(source.Metadata.Select(x => new ModelFramework.Common.Builders.MetadataBuilder(x)));
        }
    }
#nullable restore
}

