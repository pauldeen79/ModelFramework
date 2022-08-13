﻿namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public abstract partial class ModelFrameworkCSharpClassBase : CSharpClassBase
{
    protected override bool CreateCodeGenerationHeader => true;
    protected override bool EnableNullableContext => true;
    protected override Type RecordCollectionType => typeof(IReadOnlyCollection<>);
    protected override bool IsMemberValid(IParentTypeContainer parent, ITypeBase typeBase)
        => parent != null
        && typeBase != null
        && (string.IsNullOrEmpty(parent.ParentTypeFullName)
            || parent.ParentTypeFullName.GetClassName() == $"I{typeBase.Name}"
            || parent.ParentTypeFullName.GetClassName() == nameof(IEnumsContainer)
            || $"I{typeBase.Name}" == nameof(ITypeBase));

    protected override string FormatInstanceTypeName(ITypeBase instance, bool forCreate)
    {
        if (instance == null)
        {
            // Not possible, but needs to be added because TTTF.Runtime doesn't support nullable reference types
            return string.Empty;
        }

        if (forCreate)
        {
            // For creation, the typename doesn't have to be altered/formatted.
            return string.Empty;
        }

        if (instance.Namespace == "ModelFramework.Common")
        {
            return "ModelFramework.Common.Contracts.I" + instance.Name;
        }

        if (instance.Namespace == "ModelFramework.Objects")
        {
            return "ModelFramework.Objects.Contracts.I" + instance.Name;
        }

        if (instance.Namespace == "ModelFramework.Database")
        {
            return "ModelFramework.Database.Contracts.I" + instance.Name;
        }

        return string.Empty;
    }

    protected override void FixImmutableClassProperties<TBuilder, TEntity>(TypeBaseBuilder<TBuilder, TEntity> typeBaseBuilder)
        => FixImmutableBuilderProperties(typeBaseBuilder);

    protected override void FixImmutableBuilderProperties<TBuilder, TEntity>(TypeBaseBuilder<TBuilder, TEntity> typeBaseBuilder)
    {
        if (typeBaseBuilder == null)
        {
            // Not possible, but needs to be added because TTTF.Runtime doesn't support nullable reference types
            return;
        }

        typeBaseBuilder.Properties.ForEach(x => FixImmutableBuilderProperty(typeBaseBuilder.Name, x));
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
            typeof(IClassConstructor),
            typeof(IClassField),
            typeof(IClassMethod),
            typeof(IClassProperty),
            typeof(IEnum),
            typeof(IEnumMember),
            typeof(IParameter)
        };

    protected static Type[] GetObjectsModelBaseTypes()
        => new[]
        {
            typeof(ITypeBase)
        };

    protected static Type[] GetObjectsModelOverrideTypes()
        => new[]
        {
            typeof(IClass),
            typeof(IInterface),
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

    protected IClass[] GetCodeStatementBuilderClasses(Type codeStatementType,
                                                      Type codeStatementInterfaceType,
                                                      Type codeStatementBuilderInterfaceType,
                                                      string buildersNamespace)
        => GetClassesFromSameNamespace(codeStatementType)
            .Select
            (
                c => CreateBuilder(c, buildersNamespace)
                    .AddInterfaces(codeStatementBuilderInterfaceType)
                    .Chain(x => x.Methods.First(x => x.Name == "Build").WithType(codeStatementInterfaceType))
                    .Build()
            ).ToArray();

    private static void FixImmutableBuilderProperty(string name, ClassPropertyBuilder property)
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
        else if (typeName.Contains("Collection<ModelFramework.", StringComparison.InvariantCulture))
        {
            var isCodeStatement = typeName.Contains(typeof(ICodeStatement).FullName!)
                || typeName.Contains(typeof(ISqlStatement).FullName!);
            property.ConvertCollectionPropertyToBuilderOnBuilder
            (
                false,
                typeof(ReadOnlyValueCollection<>).WithoutGenerics(),
                isCodeStatement
                    ? typeName.ReplaceSuffix(">", "Builder>", StringComparison.InvariantCulture)
                    : typeName.Replace("Contracts.I", "Builders.", StringComparison.InvariantCulture).ReplaceSuffix(">", "Builder>", StringComparison.InvariantCulture),
                isCodeStatement
                    ? "{4}{0}.AddRange(source.{0}.Select(x => x.CreateBuilder()))"
                    : null
            );
        }
        else if (typeName.Contains($"Collection<{typeof(string).FullName}", StringComparison.InvariantCulture))
        {
            property.AddMetadata(Objects.MetadataNames.CustomBuilderMethodParameterExpression, $"new {typeof(ValueCollection<string>).FullName?.FixTypeName()}({{0}})");
        }
        else if (typeName.IsBooleanTypeName() || typeName.IsNullableBooleanTypeName())
        {
            property.SetDefaultArgumentValueForWithMethod(true);
            if (property.Name == nameof(IClassProperty.HasGetter) || property.Name == nameof(IClassProperty.HasSetter))
            {
                property.SetDefaultValueForBuilderClassConstructor(new Literal("true"));
            }
        }

        if (property.Name == nameof(ITypeContainer.TypeName) && property.TypeName.IsStringTypeName())
        {
            property.AddBuilderOverload("WithType", typeof(Type), "type", "{2} = type.AssemblyQualifiedName;");
        }

        if (property.Name == nameof(IMetadataContainer.Metadata) && property.TypeName.FixTypeName().GetGenericArguments().GetClassName() == nameof(IMetadata))
        {
            property.AddBuilderOverload("AddMetadata", new[] { typeof(string), typeof(object) }, new[] { "name", "value" }, new[] { false, true }, new[] { false, false }, "AddMetadata(new Common.Builders.MetadataBuilder().WithName(name).WithValue(value));");
        }

        if (property.Name == nameof(IParametersContainer.Parameters) && property.TypeName.FixTypeName().GetGenericArguments().GetClassName() == nameof(IParameter))
        {
            property.AddBuilderOverload("AddParameter", new[] { typeof(string), typeof(Type) }, new[] { "name", "type" }, "AddParameters(new ParameterBuilder().WithName(name).WithType(type));");
            property.AddBuilderOverload("AddParameter", new[] { typeof(string), typeof(string) }, new[] { "name", "typeName" }, "AddParameters(new ParameterBuilder().WithName(name).WithTypeName(typeName));");
        }

        if (property.Name == nameof(ICodeStatementsContainer.CodeStatements) && property.TypeName.FixTypeName().GetGenericArguments().GetClassName() == nameof(ICodeStatement))
        {
            property.AddBuilderOverload("AddLiteralCodeStatements", typeof(string[]), "statements", false, parameterTypeParamArray: true, "AddCodeStatements(ModelFramework.Objects.Extensions.EnumerableOfStringExtensions.ToLiteralCodeStatementBuilders(statements));");
            property.AddBuilderOverload("AddLiteralCodeStatements", typeof(IEnumerable<string>), "statements", "AddLiteralCodeStatements(statements.ToArray());");
        }

        if (property.Name == nameof(IVisibilityContainer.Visibility))
        {
            property.SetDefaultValueForBuilderClassConstructor
            (
                new Literal
                (
                    $"I{name}" == nameof(IClassField)
                        ? $"{typeof(Visibility).FullName}.{Visibility.Private}"
                        : $"{typeof(Visibility).FullName}.{Visibility.Public}"
                )
            );
        }

        if (property.Name == nameof(IClassProperty.HasSetter))
        {
            property.SetBuilderWithExpression(@"{0} = {2};
if ({2})
{5}
    HasInitializer = false;
{6}");
        }

        if (property.Name == nameof(IClassProperty.HasInitializer))
        {
            property.SetBuilderWithExpression(@"{0} = {2};
if ({2})
{5}
    HasSetter = false;
{6}");
        }
    }
}
