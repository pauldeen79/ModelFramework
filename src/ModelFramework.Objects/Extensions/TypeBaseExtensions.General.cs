using System;
using System.Collections.Generic;
using System.Linq;
using ModelFramework.Common.Extensions;
using ModelFramework.Objects.Contracts;

namespace ModelFramework.Objects.Extensions
{
    public static partial class TypeBaseExtensions
    {
        public static string GetInheritedClasses(this ITypeBase instance)
            => instance is IClass cls
                ? GetInheritedClassesForClass(cls)
                : GetInheritedClassesForTypeBase(instance);

        private static string GetInheritedClassesForClass(IClass cls)
        {
            var lst = new List<string>();
            if (!string.IsNullOrEmpty(cls.BaseClass))
            {
                lst.Add(cls.BaseClass);
            }

            lst.AddRange(cls.Interfaces);

            return lst.Count == 0
                ? string.Empty
                : $" : {string.Join(", ", lst.Select(x => x.GetCsharpFriendlyTypeName()))}";
        }

        private static string GetInheritedClassesForTypeBase(ITypeBase instance)
            => !instance.Interfaces.Any()
                ? string.Empty
                : $" : {string.Join(", ", instance.Interfaces.Select(x => x.GetCsharpFriendlyTypeName()))}";

        public static string GetContainerType(this ITypeBase type)
        {
            if (type is IClass cls)
            {
                return cls.Record
                    ? "record"
                    : "class";
            }
            if (type is IInterface)
            {
                return "interface";
            }

            throw new InvalidOperationException($"Unknown container type: [{type.GetType().FullName}]");
        }

        public static string GetGenericTypeArgumentsString(this ITypeBase instance)
            => instance.GenericTypeArguments != null && instance.GenericTypeArguments.Count > 0
                ? $"<{string.Join(", ", instance.GenericTypeArguments)}>"
                : string.Empty;
    }
}
