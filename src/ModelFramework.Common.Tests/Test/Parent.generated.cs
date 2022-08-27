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

namespace ModelFramework.Common.Tests.Test
{
#nullable enable
    public partial record Parent
    {
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
            get;
        }

        public Parent(string parentProperty, ModelFramework.Common.Tests.Test.Child child, System.Collections.Generic.IEnumerable<ModelFramework.Common.Tests.Test.Child> children)
        {
            if (parentProperty == null) throw new System.ArgumentNullException("parentProperty");
            if (child == null) throw new System.ArgumentNullException("child");
            if (children == null) throw new System.ArgumentNullException("children");
            this.ParentProperty = parentProperty;
            this.Child = child;
            this.Children = new CrossCutting.Common.ReadOnlyValueCollection<ModelFramework.Common.Tests.Test.Child>(children);
            System.ComponentModel.DataAnnotations.Validator.ValidateObject(this, new System.ComponentModel.DataAnnotations.ValidationContext(this, null, null), true);
        }
    }
#nullable restore
}

