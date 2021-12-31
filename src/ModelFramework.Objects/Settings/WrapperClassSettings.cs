using System;
using System.Collections.Generic;
using System.Reflection;
using ModelFramework.Objects.Contracts;

namespace ModelFramework.Objects.Settings
{
    public record WrapperClassSettings
    {
        public Func<MethodInfo, IEnumerable<ICodeStatement>>? MethodCodeStatementsDelegate { get; }
        public Func<PropertyInfo, IEnumerable<ICodeStatement>>? PropertyCodeStatementsDelegate { get; }

        public WrapperClassSettings(Func<MethodInfo, IEnumerable<ICodeStatement>>? methodCodeStatementsDelegate = null,
                                    Func<PropertyInfo, IEnumerable<ICodeStatement>>? propertyCodeStatementsDelegate = null)
        {
            MethodCodeStatementsDelegate = methodCodeStatementsDelegate;
            PropertyCodeStatementsDelegate = propertyCodeStatementsDelegate;
        }
    }
}
