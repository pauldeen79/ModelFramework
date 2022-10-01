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

namespace ModelFramework.Common.Tests.Test
{
#nullable enable
    public partial record Parent
    {
        [System.ComponentModel.DataAnnotations.StringLengthAttribute(10, MinimumLength = 1)]
        public string ParentProperty
        {
            get;
        }

        public ModelFramework.Common.Tests.Test.Child Child
        {
            get;
        }

        public System.Collections.Generic.IReadOnlyCollection<ModelFramework.Common.Tests.Test.Child> Children
        {
            get
            {
                return _children;
            }
        }

        public System.Collections.Generic.IReadOnlyCollection<string> Strings
        {
            get;
        }

        public Parent(string parentProperty, ModelFramework.Common.Tests.Test.Child child, System.Collections.Generic.IEnumerable<ModelFramework.Common.Tests.Test.Child> children, System.Collections.Generic.IEnumerable<string> strings)
        {
            if (parentProperty == null) throw new System.ArgumentNullException("parentProperty");
            if (child == null) throw new System.ArgumentNullException("child");
            if (children == null) throw new System.ArgumentNullException("children");
            if (strings == null) throw new System.ArgumentNullException("strings");
            this.ParentProperty = parentProperty;
            this.Child = child;
            _children = new CrossCutting.Common.ValueCollection<ModelFramework.Common.Tests.Test.Child>(children);
            this.Strings = new System.Collections.Generic.List<System.String>(strings);
            System.ComponentModel.DataAnnotations.Validator.ValidateObject(this, new System.ComponentModel.DataAnnotations.ValidationContext(this, null, null), true);
        }

        private CrossCutting.Common.ValueCollection<ModelFramework.Common.Tests.Test.Child> _children;
    }
#nullable restore
}
