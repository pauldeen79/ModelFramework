namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

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

    protected override void PostProcessImmutableBuilderClass(ClassBuilder classBuilder)
    {
        if (classBuilder == null)
        {
            // Not possible, but needs to be added because TTTF.Runtime doesn't support nullable reference types
            return;
        }

        if (new[] { $"{nameof(ITypeBase)}Builder", $"{nameof(IClass)}Builder", $"{nameof(IInterface)}Builder" }.Contains($"I{classBuilder.Name}"))
        {
            if ($"I{classBuilder.Name}" == $"{nameof(ITypeBase)}Builder")
            {
                // HACK
                if (classBuilder.Constructors.Any())
                {
                    classBuilder.Constructors.Single(x => x.Parameters.Count == 1).Parameters.Single().TypeName = typeof(ITypeBase).FullName!;
                    classBuilder.GenericTypeArgumentConstraints[0] = $"where TEntity : {typeof(ITypeBase).FullName}";
                }
            }
            else
            {
                // HACK
                classBuilder.BaseClass = $"{typeof(ITypeBase).Name.Substring(1)}Builder<{classBuilder.Name}, ModelFramework.Objects.Contracts.I{classBuilder.Name.Replace("Builder", "")}>";
                classBuilder.Methods.RemoveAll(x => !classBuilder.Properties.Any(y => x.Name == $"With{y.Name}" || x.Name == $"Add{y.Name}"));
            }
        }
    }

    protected override void PostProcessImmutableEntityClass(ClassBuilder classBuilder)
    {
        if (classBuilder == null)
        {
            // Not possible, but needs to be added because TTTF.Runtime doesn't support nullable reference types
            return;
        }

        if (new[] { nameof(IClass), nameof(IInterface) }.Contains($"I{classBuilder.Name}"))
        {
            // HACK
            var props = string.Join(", ", typeof(ITypeBase).ToClass(new ClassSettings()).Properties.Select(x => x.Name.ToPascalCase().GetCsharpFriendlyName()));
            classBuilder.Constructors.Single().WithChainCall($"base({props})");
            classBuilder.BaseClass = typeof(ITypeBase).GetEntityClassName();
        }
    }

    protected override IEnumerable<ClassMethodBuilder> CreateExtraOverloads(IClass c)
    {
        if (c == null)
        {
            // Not possible, but needs to be added because TTTF.Runtime doesn't support nullable reference types
            yield break;
        }

        if (c.Properties.Any(p => p.Name == nameof(IMetadataContainer.Metadata)))
        {
            yield return new ClassMethodBuilder()
                .WithName("AddMetadata")
                .WithTypeName($"I{c.Name}" == nameof(ITypeBase) ? "TBuilder" : $"{c.Name}Builder")
                .AddParameter("name", typeof(string))
                .AddParameters(new ParameterBuilder().WithName("value").WithType(typeof(object)).WithIsNullable())
                .AddLiteralCodeStatements($"AddMetadata(new {typeof(MetadataBuilder).FullName}().WithName(name).WithValue(value));",
                                          $"I{c.Name}" == nameof(ITypeBase) ? "return (TBuilder)this;" : "return this;");
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

    protected IClass GetTypeBaseModel()
        => typeof(ITypeBase).ToClass(new ClassSettings()).ToImmutableClassBuilder(new ImmutableClassSettings
        (
            newCollectionTypeName: RecordCollectionType.WithoutGenerics(),
            constructorSettings: new ImmutableClassConstructorSettings(
                validateArguments: ValidateArgumentsInConstructor,
                addNullChecks: AddNullChecks),
            addPrivateSetters: AddPrivateSetters)
        )
        .With(x => FixImmutableClassProperties(x))
        .Build();

    protected IClass[] GetCodeStatementBuilderClasses(Type codeStatementType,
                                                      Type codeStatementInterfaceType,
                                                      Type codeStatementBuilderInterfaceType,
                                                      string buildersNamespace)
        => GetClassesFromSameNamespace(codeStatementType ?? throw new ArgumentNullException(nameof(codeStatementType)))
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
