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
    public partial class ViewConditionBuilder
    {
        public string Expression
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

        public string Combination
        {
            get
            {
                return _combinationDelegate.Value;
            }
            set
            {
                _combinationDelegate = new (() => value);
            }
        }

        public System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder> Metadata
        {
            get;
            set;
        }

        public string FileGroupName
        {
            get
            {
                return _fileGroupNameDelegate.Value;
            }
            set
            {
                _fileGroupNameDelegate = new (() => value);
            }
        }

        public ViewConditionBuilder AddMetadata(System.Collections.Generic.IEnumerable<ModelFramework.Common.Builders.MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }

        public ViewConditionBuilder AddMetadata(params ModelFramework.Common.Builders.MetadataBuilder[] metadata)
        {
            Metadata.AddRange(metadata);
            return this;
        }

        public ViewConditionBuilder AddMetadata(string name, object? value)
        {
            AddMetadata(new ModelFramework.Common.Builders.MetadataBuilder().WithName(name).WithValue(value));
            return this;
        }

        public ModelFramework.Database.Contracts.IViewCondition Build()
        {
            return new ModelFramework.Database.ViewCondition(Expression, Combination, Metadata.Select(x => x.Build()), FileGroupName);
        }

        public ViewConditionBuilder WithCombination(string combination)
        {
            Combination = combination;
            return this;
        }

        public ViewConditionBuilder WithCombination(System.Func<string> combinationDelegate)
        {
            _combinationDelegate = new (combinationDelegate);
            return this;
        }

        public ViewConditionBuilder WithExpression(string expression)
        {
            Expression = expression;
            return this;
        }

        public ViewConditionBuilder WithExpression(System.Func<string> expressionDelegate)
        {
            _expressionDelegate = new (expressionDelegate);
            return this;
        }

        public ViewConditionBuilder WithFileGroupName(string fileGroupName)
        {
            FileGroupName = fileGroupName;
            return this;
        }

        public ViewConditionBuilder WithFileGroupName(System.Func<string> fileGroupNameDelegate)
        {
            _fileGroupNameDelegate = new (fileGroupNameDelegate);
            return this;
        }

        public ViewConditionBuilder()
        {
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            _expressionDelegate = new (() => string.Empty);
            _combinationDelegate = new (() => string.Empty);
            _fileGroupNameDelegate = new (() => string.Empty);
        }

        public ViewConditionBuilder(ModelFramework.Database.Contracts.IViewCondition source)
        {
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            _expressionDelegate = new (() => source.Expression);
            _combinationDelegate = new (() => source.Combination);
            Metadata.AddRange(source.Metadata.Select(x => new ModelFramework.Common.Builders.MetadataBuilder(x)));
            _fileGroupNameDelegate = new (() => source.FileGroupName);
        }

        private System.Lazy<string> _expressionDelegate;

        private System.Lazy<string> _combinationDelegate;

        private System.Lazy<string> _fileGroupNameDelegate;
    }
#nullable restore
}

