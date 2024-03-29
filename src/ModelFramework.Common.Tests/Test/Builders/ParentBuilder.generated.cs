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

namespace ModelFramework.Common.Tests.Test.Builders
{
#nullable enable
    public partial class ParentBuilder
    {
        [System.ComponentModel.DataAnnotations.StringLengthAttribute(10, MinimumLength = 1)]
        public System.Text.StringBuilder ParentProperty
        {
            get
            {
                return _parentPropertyDelegate.Value;
            }
            set
            {
                _parentPropertyDelegate = new (() => value);
            }
        }

        public ModelFramework.Common.Tests.Test.Builders.ChildBuilder Child
        {
            get
            {
                return _childDelegate.Value;
            }
            set
            {
                _childDelegate = new (() => value);
            }
        }

        public System.Collections.Generic.IEnumerable<ModelFramework.Common.Tests.Test.Builders.ChildBuilder> Children
        {
            get;
            set;
        }

        public System.Collections.Generic.IEnumerable<string> Strings
        {
            get;
            set;
        }

        public ModelFramework.Common.Tests.Test.Parent Build()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            return new ModelFramework.Common.Tests.Test.Parent(ParentProperty?.ToString(), Child?.Build(), Children.Select(x => x.Build()), Strings);
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public ParentBuilder WithParentProperty(System.Text.StringBuilder parentProperty)
        {
            ParentProperty = parentProperty;
            return this;
        }

        public ParentBuilder WithParentProperty(System.Func<System.Text.StringBuilder> parentPropertyDelegate)
        {
            _parentPropertyDelegate = new (parentPropertyDelegate);
            return this;
        }

        public ParentBuilder WithParentProperty(string value)
        {
            ParentProperty.Clear().Append(value);
            return this;
        }

        public ParentBuilder AppendToParentProperty(string value)
        {
            ParentProperty.Append(value);
            return this;
        }

        public ParentBuilder AppendLineToParentProperty(string value)
        {
            ParentProperty.AppendLine(value);
            return this;
        }

        public ParentBuilder WithChild(ModelFramework.Common.Tests.Test.Builders.ChildBuilder child)
        {
            Child = child;
            return this;
        }

        public ParentBuilder WithChild(System.Func<ModelFramework.Common.Tests.Test.Builders.ChildBuilder> childDelegate)
        {
            _childDelegate = new (childDelegate);
            return this;
        }

        public ParentBuilder AddChildren(System.Collections.Generic.IEnumerable<ModelFramework.Common.Tests.Test.Builders.ChildBuilder> children)
        {
            return AddChildren(children.ToArray());
        }

        public ParentBuilder AddChildren(params ModelFramework.Common.Tests.Test.Builders.ChildBuilder[] children)
        {
            if (children != null)
            {
                Children = Children.Concat(children);
            }
            return this;
        }

        public ParentBuilder AddStrings(System.Collections.Generic.IEnumerable<string> strings)
        {
            return AddStrings(strings.ToArray());
        }

        public ParentBuilder AddStrings(params string[] strings)
        {
            if (strings != null)
            {
                Strings = Strings.Concat(strings);
            }
            return this;
        }

        public ParentBuilder()
        {
            Children = Enumerable.Empty<ModelFramework.Common.Tests.Test.Builders.ChildBuilder>();
            Strings = Enumerable.Empty<string>();
            #pragma warning disable CS8603 // Possible null reference return.
            _parentPropertyDelegate = new (() => new System.Text.StringBuilder());
            _childDelegate = new (() => default);
            #pragma warning restore CS8603 // Possible null reference return.
        }

        public ParentBuilder(ModelFramework.Common.Tests.Test.Parent source)
        {
            if (source == null)
            {
                throw new System.ArgumentNullException("source");
            }
            Children = Enumerable.Empty<ModelFramework.Common.Tests.Test.Builders.ChildBuilder>();
            Strings = Enumerable.Empty<string>();
            _parentPropertyDelegate = new (() => new System.Text.StringBuilder(source.ParentProperty));
            _childDelegate = new (() => new ModelFramework.Common.Tests.Test.Builders.ChildBuilder(source.Child));
            if (source.Children != null) Children = source.Children.Select(x => new ModelFramework.Common.Tests.Test.Builders.ChildBuilder(x));
            if (source.Strings != null) Strings = source.Strings;
        }

        protected System.Lazy<System.Text.StringBuilder> _parentPropertyDelegate;

        protected System.Lazy<ModelFramework.Common.Tests.Test.Builders.ChildBuilder> _childDelegate;
    }
#nullable restore
}

