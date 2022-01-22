﻿using System;
using System.Collections.Generic;
using System.Linq;
using CrossCutting.Common;
using CrossCutting.Common.Extensions;
using ModelFramework.Common.Extensions;
using ModelFramework.Generators.Objects;
using ModelFramework.Objects.Builders;
using ModelFramework.Objects.Contracts;
using ModelFramework.Objects.Extensions;
using ModelFramework.Objects.Settings;

namespace ModelFramework.CodeGeneration.CodeGenerationProviders
{
    public abstract class CSharpClassBase : ICodeGenerationProvider
    {
        public bool GenerateMultipleFiles { get; set; }
        protected abstract bool EnableNullableContext { get; }
        protected abstract bool CreateCodeGenerationHeader { get; }

        public string BasePath { get; set; } = string.Empty;

        public abstract string Prefix { get; }

        public abstract string DefaultFileName { get; }

        protected abstract object CreateModel();

        public object CreateAdditionalParameters()
            => new Dictionary<string, object>
                {
                    { nameof(CSharpClassGenerator.EnableNullableContext), EnableNullableContext },
                    { nameof(CSharpClassGenerator.CreateCodeGenerationHeader), CreateCodeGenerationHeader },
                    { nameof(CSharpClassGenerator.GenerateMultipleFiles), GenerateMultipleFiles },
                    { nameof(CSharpClassGenerator.Model), CreateModel() }
                };

        public object CreateGenerator()
            => new CSharpClassGenerator();

        protected IClass[] GetImmutableBuilderClasses(Type[] types, string entitiesNamespace, string buildersNamespace)
            => types.Select
            (
                x => CreateBuilder(x.ToClassBuilder(new ClassSettings())
                                    .WithName(x.Name.Substring(1))
                                    .WithNamespace(entitiesNamespace)
                                    .Chain(y => FixImmutableBuilderProperties(y))
                                    .Build()
                                    .ToImmutableClass(CreateImmutableClassSettings()), buildersNamespace)
                                    .Build()
            ).ToArray();

        protected IClass[] GetImmutableClasses(Type[] types, string entitiesNamespace)
            => types.Select
            (
                x => x.ToClassBuilder(new ClassSettings())
                      .WithName(x.Name.Substring(1))
                      .WithNamespace(entitiesNamespace)
                      .Chain(y => FixImmutableBuilderProperties(y))
                      .Build()
                      .ToImmutableClassBuilder(CreateImmutableClassSettings())
                      .WithRecord()
                      .WithPartial()
                      .AddInterfaces(x)
                      .Build()
            ).ToArray();

        protected IClass[] GetClassesFromSameNamespace(Type type)
        {
            if (type.FullName == null)
            {
                throw new ArgumentException("Can't get classes from same namespace when the FullName of this type is null. Could not determine namespace.");
            }

            return type.Assembly.GetExportedTypes()
                .Where
                (
                    t => t.FullName != null
                        && t.FullName.GetNamespaceWithDefault() == type.FullName.GetNamespaceWithDefault()
                        && t.GetProperties().Any()
                )
                .Select
                (
                    t => t.ToClassBuilder(new ClassSettings(createConstructors: true)).WithName(t.Name)
                                                                                      .WithNamespace(t.FullName?.GetNamespaceWithDefault() ?? string.Empty)
                    .Chain(x => FixImmutableBuilderProperties(x))
                    .Build()
                )
                .ToArray();
        }

        protected ClassBuilder CreateBuilder(IClass c, string @namespace)
            => c.ToImmutableBuilderClassBuilder(new ImmutableBuilderClassSettings(constructorSettings: new ImmutableBuilderClassConstructorSettings(addCopyConstructor: true),
                                                formatInstanceTypeNameDelegate: FormatInstanceTypeName))
                .WithNamespace(@namespace)
                .WithPartial()
                .AddMethods(CreateExtraOverloads(c));

        protected abstract string FormatInstanceTypeName(ITypeBase instance, bool forCreate);

        protected abstract void FixImmutableBuilderProperties(ClassBuilder classBuilder);

        protected abstract IEnumerable<ClassMethodBuilder> CreateExtraOverloads(IClass c);
       
        private static ImmutableClassSettings CreateImmutableClassSettings()
            => new ImmutableClassSettings(newCollectionTypeName: typeof(ValueCollection<>).WithoutGenerics(),
                                          validateArgumentsInConstructor: true);
    }
}
