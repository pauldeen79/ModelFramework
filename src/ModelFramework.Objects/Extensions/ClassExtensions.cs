using System;
using System.Linq;
using ModelFramework.Objects.Contracts;

namespace ModelFramework.Objects.Extensions
{
    public static class ClassExtensions
    {
        public static IClass ToImmutableBuilderClass
        (
            this IClass instance,
            string newCollectionTypeName = "System.Collections.Generic.Collection",
            bool addProperties = false,
            bool addCopyConstructor = false,
            bool addNullChecks = false,
            string newNamespace = null,
            Func<ITypeBase, bool, string> formatInstanceTypeNameDelegate = null
        ) => ((ITypeBase)instance).ToImmutableBuilderClass(newCollectionTypeName,
                                                           addProperties,
                                                           addCopyConstructor,
                                                           newNamespace,
                                                           instance.IsPoco(),
                                                           addNullChecks,
                                                           formatInstanceTypeNameDelegate);

        public static bool IsPoco(this IClass instance)
            => instance.Constructors.Count == 0 || instance.Constructors.Any(x => x.Parameters.Count == 0);
    }
}
