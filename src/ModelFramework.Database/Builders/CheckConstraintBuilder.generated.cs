﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 6.0.9
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
    public partial class CheckConstraintBuilder
    {
        public System.Text.StringBuilder Expression
        {
            get
            {
                return _expressionDelegate.Value;
            }
            set
            {
                _expressionDelegate = new (() => value);
            }
        }

        public System.Text.StringBuilder Name
        {
            get
            {
                return _nameDelegate.Value;
            }
            set
            {
                _nameDelegate = new (() => value);
            }
        }

        public System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder> Metadata
        {
            get;
            set;
        }

        public ModelFramework.Database.Contracts.ICheckConstraint Build()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            return new ModelFramework.Database.CheckConstraint(Expression?.ToString(), Name?.ToString(), Metadata.Select(x => x.Build()));
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public CheckConstraintBuilder WithExpression(System.Text.StringBuilder expression)
        {
            Expression = expression;
            return this;
        }

        public CheckConstraintBuilder WithExpression(System.Func<System.Text.StringBuilder> expressionDelegate)
        {
            _expressionDelegate = new (expressionDelegate);
            return this;
        }

        public CheckConstraintBuilder WithExpression(string value)
        {
            if (Expression == null)
                Expression = new System.Text.StringBuilder();
            Expression.Clear().Append(value);
            return this;
        }

        public CheckConstraintBuilder AppendToExpression(string value)
        {
            if (Expression == null)
                Expression = new System.Text.StringBuilder();
            Expression.Append(value);
            return this;
        }

        public CheckConstraintBuilder AppendLineToExpression(string value)
        {
            if (Expression == null)
                Expression = new System.Text.StringBuilder();
            Expression.AppendLine(value);
            return this;
        }

        public CheckConstraintBuilder WithName(System.Text.StringBuilder name)
        {
            Name = name;
            return this;
        }

        public CheckConstraintBuilder WithName(System.Func<System.Text.StringBuilder> nameDelegate)
        {
            _nameDelegate = new (nameDelegate);
            return this;
        }

        public CheckConstraintBuilder WithName(string value)
        {
            if (Name == null)
                Name = new System.Text.StringBuilder();
            Name.Clear().Append(value);
            return this;
        }

        public CheckConstraintBuilder AppendToName(string value)
        {
            if (Name == null)
                Name = new System.Text.StringBuilder();
            Name.Append(value);
            return this;
        }

        public CheckConstraintBuilder AppendLineToName(string value)
        {
            if (Name == null)
                Name = new System.Text.StringBuilder();
            Name.AppendLine(value);
            return this;
        }

        public CheckConstraintBuilder AddMetadata(System.Collections.Generic.IEnumerable<ModelFramework.Common.Builders.MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }

        public CheckConstraintBuilder AddMetadata(params ModelFramework.Common.Builders.MetadataBuilder[] metadata)
        {
            Metadata.AddRange(metadata);
            return this;
        }

        public CheckConstraintBuilder AddMetadata(string name, object? value)
        {
            AddMetadata(new ModelFramework.Common.Builders.MetadataBuilder().WithName(name).WithValue(value));
            return this;
        }

        public CheckConstraintBuilder()
        {
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            #pragma warning disable CS8603 // Possible null reference return.
            _expressionDelegate = new (() => new System.Text.StringBuilder());
            _nameDelegate = new (() => new System.Text.StringBuilder());
            #pragma warning restore CS8603 // Possible null reference return.
        }

        public CheckConstraintBuilder(ModelFramework.Database.Contracts.ICheckConstraint source)
        {
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            _expressionDelegate = new (() => new System.Text.StringBuilder(source.Expression));
            _nameDelegate = new (() => new System.Text.StringBuilder(source.Name));
            Metadata.AddRange(source.Metadata.Select(x => new ModelFramework.Common.Builders.MetadataBuilder(x)));
        }

        protected System.Lazy<System.Text.StringBuilder> _expressionDelegate;

        protected System.Lazy<System.Text.StringBuilder> _nameDelegate;
    }
#nullable restore
}

