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
    public partial class ChildBuilder
    {
        public string ChildProperty
        {
            get
            {
                return _childPropertyDelegate.Value;
            }
            set
            {
                _childPropertyDelegate = new (() => value);
            }
        }

        public ModelFramework.Common.Tests.Test.Child Build()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            return new ModelFramework.Common.Tests.Test.Child(ChildProperty);
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public ChildBuilder WithChildProperty(string childProperty)
        {
            ChildProperty = childProperty;
            return this;
        }

        public ChildBuilder WithChildProperty(System.Func<string> childPropertyDelegate)
        {
            _childPropertyDelegate = new (childPropertyDelegate);
            return this;
        }

        public ChildBuilder()
        {
            #pragma warning disable CS8603 // Possible null reference return.
            _childPropertyDelegate = new (() => string.Empty);
            #pragma warning restore CS8603 // Possible null reference return.
        }

        public ChildBuilder(ModelFramework.Common.Tests.Test.Child source)
        {
            _childPropertyDelegate = new (() => source.ChildProperty);
        }

        protected System.Lazy<string> _childPropertyDelegate;
    }
#nullable restore
}

