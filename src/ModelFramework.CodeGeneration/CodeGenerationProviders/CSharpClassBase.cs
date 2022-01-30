using System;
using System.Collections.Generic;
using System.Linq;
using CrossCutting.Common.Extensions;
using ModelFramework.Common.Extensions;
using ModelFramework.Generators.Objects;
using ModelFramework.Objects.Builders;
using ModelFramework.Objects.Contracts;
using ModelFramework.Objects.Extensions;
using ModelFramework.Objects.Settings;
using TextTemplateTransformationFramework.Runtime.CodeGeneration;

namespace ModelFramework.CodeGeneration.CodeGenerationProviders
{
    public abstract class CSharpClassBase : ICodeGenerationProvider
    {
        public bool GenerateMultipleFiles { get; private set; }
        public string BasePath { get; private set; } = string.Empty;

        public abstract string Path { get; }
        public abstract string DefaultFileName { get; }
        public abstract bool RecurseOnDeleteGeneratedFiles { get; }
        public abstract object CreateModel();

        public virtual string LastGeneratedFilesFileName => "*.generated.cs";
        public virtual Action? AdditionalActionDelegate => null;

        public void Initialize(bool generateMultipleFiles, string basePath)
        {
            GenerateMultipleFiles = generateMultipleFiles;
            BasePath = basePath;
        }

        public object CreateAdditionalParameters()
            => new Dictionary<string, object>
            {
                { nameof(CSharpClassGenerator.EnableNullableContext), EnableNullableContext },
                { nameof(CSharpClassGenerator.CreateCodeGenerationHeader), CreateCodeGenerationHeader },
                { nameof(CSharpClassGenerator.GenerateMultipleFiles), GenerateMultipleFiles }
            };

        public object CreateGenerator()
            => new CSharpClassGenerator();

        protected virtual IEnumerable<ClassMethodBuilder> CreateExtraOverloads(IClass c)
            => Enumerable.Empty<ClassMethodBuilder>();
        protected virtual string NewCollectionTypeName => "System.Collections.Generic.List";
        protected virtual string SetMethodNameFormatString => "With{0}";
        protected virtual bool Poco => false;
        protected virtual bool AddNullChecks => false;
        protected virtual bool AddCopyConstructor => true;

        protected abstract bool EnableNullableContext { get; }
        protected abstract bool CreateCodeGenerationHeader { get; }
        protected abstract Type RecordCollectionType { get; }

        protected abstract string FormatInstanceTypeName(ITypeBase instance, bool forCreate);
        protected abstract void FixImmutableBuilderProperties(ClassBuilder classBuilder);

        protected IClass[] GetImmutableBuilderClasses(Type[] types, string entitiesNamespace, string buildersNamespace)
            => GetImmutableBuilderClasses(types.Select(x => x.ToClass(new ClassSettings())).ToArray(), entitiesNamespace, buildersNamespace);

        protected IClass[] GetImmutableBuilderClasses(ITypeBase[] models, string entitiesNamespace, string buildersNamespace)
            => GetImmutableBuilderClasses(models, entitiesNamespace, buildersNamespace, Array.Empty<string>());

        protected IClass[] GetImmutableBuilderClasses(ITypeBase[] models, string entitiesNamespace, string buildersNamespace, params string[] interfacesToAdd)
            => models.Select
            (
                x => CreateBuilder(new ClassBuilder(x.ToClass())
                                    .WithName(x.Name.Substring(1))
                                    .WithNamespace(entitiesNamespace)
                                    .Chain(y => FixImmutableBuilderProperties(y))
                                    .Build()
                                    .ToImmutableClass(CreateImmutableClassSettings()), buildersNamespace
                                  )
                                  .Chain(x => x.AddInterfaces(interfacesToAdd.Select(y => string.Format(y, x.Name))))
                                  .Build()
            ).ToArray();

        protected IClass[] GetImmutableClasses(Type[] types, string entitiesNamespace)
            => GetImmutableClasses(types.Select(x => x.ToClass(new ClassSettings())).ToArray(), entitiesNamespace);

        protected IClass[] GetImmutableClasses(ITypeBase[] models, string entitiesNamespace)
            => models.Select
            (
                x => new ClassBuilder(x.ToClass())
                      .WithName(x.Name.Substring(1))
                      .WithNamespace(entitiesNamespace)
                      .Chain(y => FixImmutableBuilderProperties(y))
                      .Build()
                      .ToImmutableClassBuilder(CreateImmutableClassSettings())
                      .WithRecord()
                      .WithPartial()
                      .AddInterfaces($"{x.Namespace}.{x.Name}")
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
            => c.ToImmutableBuilderClassBuilder(new ImmutableBuilderClassSettings(newCollectionTypeName: NewCollectionTypeName,
                                                                                  constructorSettings: new ImmutableBuilderClassConstructorSettings(addCopyConstructor: AddCopyConstructor),
                                                                                  poco: Poco,
                                                                                  addNullChecks: AddNullChecks,
                                                                                  setMethodNameFormatString: SetMethodNameFormatString,
                                                                                  formatInstanceTypeNameDelegate: FormatInstanceTypeName))
                .WithNamespace(@namespace)
                .WithPartial()
                .AddMethods(CreateExtraOverloads(c));

        protected ImmutableClassSettings CreateImmutableClassSettings()
            => new ImmutableClassSettings(newCollectionTypeName: RecordCollectionType.WithoutGenerics(),
                                          validateArgumentsInConstructor: true);
    }
}
