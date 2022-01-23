using System;
using System.Collections.Generic;
using System.Linq;
using CrossCutting.Common;
using CrossCutting.Common.Extensions;
using ModelFramework.CodeGeneration.CodeGenerationProviders;
using ModelFramework.Common;
using ModelFramework.Common.Builders;
using ModelFramework.Common.Contracts;
using ModelFramework.Common.Extensions;
using ModelFramework.Database.Contracts;
using ModelFramework.Objects;
using ModelFramework.Objects.Builders;
using ModelFramework.Objects.Contracts;
using ModelFramework.Objects.Extensions;

namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders
{
    public abstract class ModelFrameworkCSharpClassBase : CSharpClassBase
    {
        public override string LastGeneratedFilesFileName => "*.generated.cs";
        public override Action? AdditionalActionDelegate => null;

        protected override bool CreateCodeGenerationHeader => true;
        protected override bool EnableNullableContext => true;
        protected override Type RecordCollectionType => typeof(ValueCollection<>);

        protected IClass[] GetCodeStatementBuilderClasses(Type codeStatementType, Type codeStatementInterfaceType, Type codeStatementBuilderInterfaceType, string buildersNamespace)
            => GetClassesFromSameNamespace(codeStatementType ?? throw new ArgumentNullException(nameof(codeStatementType)))
                .Select
                (
                    c => CreateBuilder(c, buildersNamespace)
                        .AddInterfaces(codeStatementBuilderInterfaceType)
                        .Chain(x => x.Methods.First(x => x.Name == "Build").WithType(codeStatementInterfaceType))
                        .Build()
                ).ToArray();

        protected override string FormatInstanceTypeName(ITypeBase instance, bool forCreate)
        {
            if (instance.Namespace == "ModelFramework.Common")
            {
                return forCreate
                    ? "ModelFramework.Common." + instance.Name
                    : "ModelFramework.Common.Contracts.I" + instance.Name;
            }

            if (instance.Namespace == "ModelFramework.Objects")
            {
                return forCreate
                    ? "ModelFramework.Objects." + instance.Name
                    : "ModelFramework.Objects.Contracts.I" + instance.Name;
            }

            if (instance.Namespace == "ModelFramework.Database")
            {
                return forCreate
                    ? "ModelFramework.Database." + instance.Name
                    : "ModelFramework.Database.Contracts.I" + instance.Name;
            }

            return string.Empty;
        }

        protected override void FixImmutableBuilderProperties(ClassBuilder classBuilder)
        {
            foreach (var property in classBuilder.Properties)
            {
                var typeName = property.TypeName.FixTypeName();
                if (typeName.StartsWithAny(StringComparison.InvariantCulture, "ModelFramework.Objects.Contracts.I",
                                                                              "ModelFramework.Database.Contracts.I",
                                                                              "ModelFramework.Common.Contracts.I"))
                {
                    property.ConvertSinglePropertyToBuilderOnBuilder
                    (
                        typeName.Replace("Contracts.I", "Builders.", StringComparison.InvariantCulture) + "Builder"
                    );
                }
                else if (typeName.Contains("Collection<ModelFramework."))
                {
                    var isCodeStatement = typeName.Contains("ModelFramework.Objects.Contracts.ICodeStatement")
                        || typeName.Contains("ModelFramework.Database.Contracts.ISqlStatement");
                    property.ConvertCollectionPropertyToBuilderOnBuilder
                    (
                        false,
                        typeof(ValueCollection<>).WithoutGenerics(),
                        isCodeStatement
                            ? typeName.ReplaceSuffix(">", "Builder>", StringComparison.InvariantCulture)
                            : typeName.Replace("Contracts.I", "Builders.", StringComparison.InvariantCulture).ReplaceSuffix(">", "Builder>", StringComparison.InvariantCulture),
                        isCodeStatement
                            ? "{4}{0}.AddRange(source.{0}.Select(x => x.CreateBuilder()));"
                            : null
                    );
                }
                else if (typeName.Contains("Collection<System.String"))
                {
                    property.AddMetadata(Objects.MetadataNames.CustomBuilderMethodParameterExpression, $"new {typeof(ValueCollection<string>).FullName?.FixTypeName()}({{0}})");
                }
                else if (typeName.IsBooleanTypeName() || typeName.IsNullableBooleanTypeName())
                {
                    property.SetDefaultArgumentValueForWithMethod(true);
                    if (property.Name == nameof(ClassProperty.HasGetter) || property.Name == nameof(ClassProperty.HasSetter))
                    {
                        property.SetDefaultValueForBuilderClassConstructor(new Literal("true"));
                    }
                }
                else if (property.Name == nameof(ITypeContainer.TypeName) && property.TypeName.IsStringTypeName())
                {
                    property.AddBuilderOverload("WithType", typeof(Type), "type", "{2} = type.AssemblyQualifiedName;");
                }

                if (property.Name == nameof(IVisibilityContainer.Visibility))
                {
                    property.SetDefaultValueForBuilderClassConstructor
                    (
                        new Literal
                        (
                            classBuilder.Name == nameof(ClassField)
                                ? $"{typeof(Visibility).FullName}.{Visibility.Private}"
                                : $"{typeof(Visibility).FullName}.{Visibility.Public}"
                        )
                    );
                }

                if (property.Name == nameof(ClassProperty.HasSetter))
                {
                    property.SetBuilderWithExpression(@"{0} = {2};
if ({2})
{5}
    HasInitializer = false;
{6}");
                }

                if (property.Name == nameof(ClassProperty.HasInitializer))
                {
                    property.SetBuilderWithExpression(@"{0} = {2};
if ({2})
{5}
    HasSetter = false;
{6}");
                }
            }
        }

        protected override IEnumerable<ClassMethodBuilder> CreateExtraOverloads(IClass c)
        {
            if (c.Properties.Any(p => p.Name == nameof(IMetadataContainer.Metadata)))
            {
                yield return new ClassMethodBuilder()
                    .WithName("AddMetadata")
                    .WithTypeName($"{c.Name}Builder")
                    .AddParameter("name", typeof(string))
                    .AddParameters(new ParameterBuilder().WithName("value").WithType(typeof(object)).WithIsNullable())
                    .AddLiteralCodeStatements($"AddMetadata(new {typeof(MetadataBuilder).FullName}().WithName(name).WithValue(value));",
                                               "return this;");
            }

            if (c.Properties.Any(p => p.Name == nameof(IParametersContainer.Parameters) && p.TypeName.FixTypeName().GetGenericArguments().GetClassName() == nameof(IParameter)))
            {
                yield return new ClassMethodBuilder()
                    .WithName("AddParameter")
                    .WithTypeName($"{c.Name}Builder")
                    .AddParameter("name", typeof(string))
                    .AddParameter("type", typeof(Type))
                    .AddLiteralCodeStatements("return AddParameters(new ParameterBuilder().WithName(name).WithType(type));");
                yield return new ClassMethodBuilder()
                    .WithName("AddParameter")
                    .WithTypeName($"{c.Name}Builder")
                    .AddParameter("name", typeof(string))
                    .AddParameter("typeName", typeof(string))
                    .AddLiteralCodeStatements("return AddParameters(new ParameterBuilder().WithName(name).WithTypeName(typeName));");
            }

            if (c.Properties.Any(p => p.Name == nameof(ICodeStatementsContainer.CodeStatements)))
            {
                yield return new ClassMethodBuilder()
                    .WithName("AddLiteralCodeStatements")
                    .WithTypeName($"{c.Name}Builder")
                    .AddParameters(new ParameterBuilder().WithName("statements").WithType(typeof(string[])).WithIsParamArray())
                    .AddLiteralCodeStatements("return AddCodeStatements(ModelFramework.Objects.Extensions.EnumerableOfStringExtensions.ToLiteralCodeStatementBuilders(statements));");
                yield return new ClassMethodBuilder()
                    .WithName("AddLiteralCodeStatements")
                    .WithTypeName($"{c.Name}Builder")
                    .AddParameters(new ParameterBuilder().WithName("statements").WithType(typeof(IEnumerable<string>)))
                    .AddLiteralCodeStatements("return AddLiteralCodeStatements(statements.ToArray());");
            }
        }

        protected static Type[] GetCommonModelTypes()
            => new[]
            {
                typeof(IMetadata)
            };

        protected static Type[] GetObjectsModelTypes()
            => new[]
            {
                typeof(IAttribute),
                typeof(IAttributeParameter),
                typeof(IClass),
                typeof(IClassConstructor),
                typeof(IClassField),
                typeof(IClassMethod),
                typeof(IClassProperty),
                typeof(IEnum),
                typeof(IEnumMember),
                typeof(IInterface),
                typeof(IParameter)
            };

        protected static Type[] GetDatabaseModelTypes()
            => new[]
            {
                typeof(ICheckConstraint),
                typeof(IDefaultValueConstraint),
                typeof(IForeignKeyConstraint),
                typeof(IForeignKeyConstraintField),
                typeof(IIndex),
                typeof(IIndexField),
                typeof(IPrimaryKeyConstraint),
                typeof(IPrimaryKeyConstraintField),
                typeof(ISchema),
                typeof(IStoredProcedure),
                typeof(IStoredProcedureParameter),
                typeof(ITable),
                typeof(ITableField),
                typeof(IUniqueConstraint),
                typeof(IUniqueConstraintField),
                typeof(IView),
                typeof(IViewCondition),
                typeof(IViewField),
                typeof(IViewOrderByField),
                typeof(IViewSource)
            };
    }
}
