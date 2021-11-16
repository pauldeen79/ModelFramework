using System;
using ModelFramework.Objects.Contracts;

namespace ModelFramework.Objects.Settings
{
    public class ImmutableBuilderClassSettings
    {
        public string NewCollectionTypeName { get; }
        public bool AddProperties { get; }
        public bool AddCopyConstructor { get; }
        public bool Poco { get; }
        public bool AddNullChecks { get; }
        public string SetMethodNameFormatString { get; }
        public Func<ITypeBase, bool, string> FormatInstanceTypeNameDelegate { get; }

        public ImmutableBuilderClassSettings(string newCollectionTypeName = "System.Collections.Generic.List",
                                             bool addProperties = false,
                                             bool addCopyConstructor = false,
                                             bool poco = false,
                                             bool addNullChecks = false,
                                             string setMethodNameFormatString = "With{0}",
                                             Func<ITypeBase, bool, string> formatInstanceTypeNameDelegate = null)
        {
            NewCollectionTypeName = newCollectionTypeName;
            AddProperties = addProperties;
            AddCopyConstructor = addCopyConstructor;
            Poco = poco;
            AddNullChecks = addNullChecks;
            SetMethodNameFormatString = setMethodNameFormatString;
            FormatInstanceTypeNameDelegate = formatInstanceTypeNameDelegate;
        }

        public ImmutableBuilderClassSettings WithPoco(bool isPoco)
            => new ImmutableBuilderClassSettings(NewCollectionTypeName,
                                                 AddProperties,
                                                 AddCopyConstructor,
                                                 Poco || isPoco,
                                                 AddNullChecks,
                                                 SetMethodNameFormatString,
                                                 FormatInstanceTypeNameDelegate);
    }
}
