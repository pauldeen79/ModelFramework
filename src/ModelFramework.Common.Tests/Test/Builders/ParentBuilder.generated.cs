﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 6.0.8
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
        public string ParentProperty
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

        public ModelFramework.Common.Tests.Test.Child Child
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

        public System.Collections.Generic.List<ModelFramework.Common.Tests.Test.Child> Children
        {
            get;
            set;
        }

        public ModelFramework.Common.Tests.Test.Parent Build()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            return new ModelFramework.Common.Tests.Test.Parent(ParentProperty, Child, Children);
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public ParentBuilder WithParentProperty(string parentProperty)
        {
            ParentProperty = parentProperty;
            return this;
        }

        public ParentBuilder WithParentProperty(System.Func<string> parentPropertyDelegate)
        {
            _parentPropertyDelegate = new (parentPropertyDelegate);
            return this;
        }

        public ParentBuilder WithChild(ModelFramework.Common.Tests.Test.Child child)
        {
            Child = child;
            return this;
        }

        public ParentBuilder WithChild(System.Func<ModelFramework.Common.Tests.Test.Child> childDelegate)
        {
            _childDelegate = new (childDelegate);
            return this;
        }

        public ParentBuilder AddChildren(System.Collections.Generic.IEnumerable<ModelFramework.Common.Tests.Test.Child> children)
        {
            return AddChildren(children.ToArray());
        }

        public ParentBuilder AddChildren(params ModelFramework.Common.Tests.Test.Child[] children)
        {
            Children.AddRange(children);
            return this;
        }

        public ParentBuilder()
        {
            Children = new System.Collections.Generic.List<ModelFramework.Common.Tests.Test.Child>();
            #pragma warning disable CS8603 // Possible null reference return.
            _parentPropertyDelegate = new (() => string.Empty);
            _childDelegate = new (() => default);
            #pragma warning restore CS8603 // Possible null reference return.
        }

        public ParentBuilder(ModelFramework.Common.Tests.Test.Parent source)
        {
            Children = new System.Collections.Generic.List<ModelFramework.Common.Tests.Test.Child>();
            _parentPropertyDelegate = new (() => source.ParentProperty);
            _childDelegate = new (() => source.Child);
            Children.AddRange(source.Children);
        }

        protected System.Lazy<string> _parentPropertyDelegate;

        protected System.Lazy<ModelFramework.Common.Tests.Test.Child> _childDelegate;
    }
#nullable restore
}

