﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 6.0.3
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
    public partial class ViewOrderByFieldBuilder
    {
        public bool IsDescending
        {
            get;
            set;
        }

        public string SourceSchemaName
        {
            get;
            set;
        }

        public string SourceObjectName
        {
            get;
            set;
        }

        public string Expression
        {
            get;
            set;
        }

        public string Alias
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

        public ViewOrderByFieldBuilder AddMetadata(System.Collections.Generic.IEnumerable<ModelFramework.Common.Builders.MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }

        public ViewOrderByFieldBuilder AddMetadata(params ModelFramework.Common.Builders.MetadataBuilder[] metadata)
        {
            Metadata.AddRange(metadata);
            return this;
        }

        public ViewOrderByFieldBuilder AddMetadata(string name, object? value)
        {
            AddMetadata(new ModelFramework.Common.Builders.MetadataBuilder().WithName(name).WithValue(value));
            return this;
        }

        public ModelFramework.Database.Contracts.IViewOrderByField Build()
        {
            return new ModelFramework.Database.ViewOrderByField(IsDescending, SourceSchemaName, SourceObjectName, Expression, Alias, Name, Metadata.Select(x => x.Build()));
        }

        public ViewOrderByFieldBuilder WithAlias(string alias)
        {
            Alias = alias;
            return this;
        }

        public ViewOrderByFieldBuilder WithExpression(string expression)
        {
            Expression = expression;
            return this;
        }

        public ViewOrderByFieldBuilder WithIsDescending(bool isDescending = true)
        {
            IsDescending = isDescending;
            return this;
        }

        public ViewOrderByFieldBuilder WithName(string name)
        {
            Name = name;
            return this;
        }

        public ViewOrderByFieldBuilder WithSourceObjectName(string sourceObjectName)
        {
            SourceObjectName = sourceObjectName;
            return this;
        }

        public ViewOrderByFieldBuilder WithSourceSchemaName(string sourceSchemaName)
        {
            SourceSchemaName = sourceSchemaName;
            return this;
        }

        public ViewOrderByFieldBuilder()
        {
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            IsDescending = default;
            SourceSchemaName = string.Empty;
            SourceObjectName = string.Empty;
            Expression = string.Empty;
            Alias = string.Empty;
            Name = string.Empty;
        }

        public ViewOrderByFieldBuilder(ModelFramework.Database.Contracts.IViewOrderByField source)
        {
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            IsDescending = source.IsDescending;
            SourceSchemaName = source.SourceSchemaName;
            SourceObjectName = source.SourceObjectName;
            Expression = source.Expression;
            Alias = source.Alias;
            Name = source.Name;
            Metadata.AddRange(source.Metadata.Select(x => new ModelFramework.Common.Builders.MetadataBuilder(x)));
        }
    }
#nullable restore
}
