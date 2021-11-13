﻿using System;
using ModelFramework.Objects.Contracts;

namespace ModelFramework.Objects.Settings
{
    public class ImmutableBuilderClassSettings
    {
        public string NewCollectionTypeName { get; }
        public bool AddProperties { get; }
        public bool AddCopyConstructor { get; }
        public string NewNamespace { get; }
        public bool Poco { get; }
        public bool AddNullChecks { get; }
        public Func<ITypeBase, bool, string> FormatInstanceTypeNameDelegate { get; }

        public ImmutableBuilderClassSettings(string newCollectionTypeName = "System.Collections.Generic.List",
                                             bool addProperties = false,
                                             bool addCopyConstructor = false,
                                             string newNamespace = null,
                                             bool poco = false,
                                             bool addNullChecks = false,
                                             Func<ITypeBase, bool, string> formatInstanceTypeNameDelegate = null)
        {
            NewCollectionTypeName = newCollectionTypeName;
            AddProperties = addProperties;
            AddCopyConstructor = addCopyConstructor;
            NewNamespace = newNamespace;
            Poco = poco;
            AddNullChecks = addNullChecks;
            FormatInstanceTypeNameDelegate = formatInstanceTypeNameDelegate;
        }

        public ImmutableBuilderClassSettings WithPoco(bool isPoco)
            => new ImmutableBuilderClassSettings(NewCollectionTypeName,
                                                 AddProperties,
                                                 AddCopyConstructor,
                                                 NewNamespace,
                                                 Poco || isPoco,
                                                 AddNullChecks,
                                                 FormatInstanceTypeNameDelegate);
    }
}
