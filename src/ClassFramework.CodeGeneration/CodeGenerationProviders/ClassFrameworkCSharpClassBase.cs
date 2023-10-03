namespace ClassFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public abstract class ClassFrameworkCSharpClassBase : CSharpClassBase
{
    public override bool RecurseOnDeleteGeneratedFiles => false;
    public override string DefaultFileName => string.Empty; // not used because we're using multiple files, but it's abstract so we need to fill it

    protected override bool CreateCodeGenerationHeader => true;
    protected override bool EnableNullableContext => true;
    protected override Type RecordCollectionType => typeof(IReadOnlyCollection<>);
    protected override Type RecordConcreteCollectionType => typeof(ReadOnlyValueCollection<>);
    protected override string FileNameSuffix => Constants.TemplateGenerated;
    protected override string ProjectName => Constants.ProjectName;
    protected override bool UseLazyInitialization => false; // we don't want lazy stuff in models, just getters and setters
    protected override bool AddBackingFieldsForCollectionProperties => false; // we just want static stuff - else you need to choose builders or models instead of entities
    protected override bool AddPrivateSetters => false; // we just want static stuff - else you need to choose builders or models instead of entities
    protected override ArgumentValidationType ValidateArgumentsInConstructor => ArgumentValidationType.Shared;
    protected override bool ConvertStringToStringBuilderOnBuilders => false; // we don't want string builders, just strings

    protected override void Visit<TBuilder, TEntity>(ModelFramework.Objects.Builders.TypeBaseBuilder<TBuilder, TEntity> typeBaseBuilder)
    {
        Guard.IsNotNull(typeBaseBuilder);
        
        Type? sourceModel = null;
        Type[] interfaces;
        
        if (typeBaseBuilder.Name.EndsWith(BuilderName))
        {
            if (typeBaseBuilder is ModelFramework.Objects.Builders.ClassBuilder classBuilder)
            {
                classBuilder.AddMethods(new ModelFramework.Objects.Builders.ClassMethodBuilder().WithName("SetDefaultValues").WithPartial().WithVisibility(ModelFramework.Objects.Contracts.Visibility.Private));
                classBuilder.Constructors.First(x => x.Parameters.Count == 0).CodeStatements.Add(new ModelFramework.Objects.CodeStatements.Builders.LiteralCodeStatementBuilder("SetDefaultValues();"));
            }

            sourceModel = Array.Find(GetType().Assembly.GetTypes(), x => x.Name == $"I{typeBaseBuilder.Name.ReplaceSuffix(BuilderName, string.Empty, StringComparison.Ordinal)}");
            if (sourceModel is null)
            {
                return;
            }

            interfaces = sourceModel.GetInterfaces();
            foreach (var i in interfaces.Where(x => x.FullName is not null && !x.Name.EndsWith("Base")))
            {
                typeBaseBuilder.AddInterfaces(i.FullName!.Replace("ClassFramework.CodeGeneration.Models.Abstractions.", $"{Constants.Namespaces.Domain}.{BuildersName}.Abstractions.", StringComparison.Ordinal) + BuilderName);
            }

            return;
        }

        sourceModel = Array.Find(GetType().Assembly.GetTypes(), x => x.Name == $"I{typeBaseBuilder.Name}");
        if (sourceModel is null)
        {
            return;
        }

        interfaces = sourceModel.GetInterfaces();
        foreach (var i in interfaces.Where(x => x.FullName is not null && !x.Name.EndsWith("Base")))
        {
            typeBaseBuilder.AddInterfaces(i.FullName!.Replace("ClassFramework.CodeGeneration.Models.Abstractions.", $"{Constants.Namespaces.Domain}.Abstractions.", StringComparison.Ordinal));
        }
    }

    protected object CreateBuilderInterfacesModel()
        => GetType().Assembly.GetTypes()
            .Where(x => x.Namespace == "ClassFramework.CodeGeneration.Models.Abstractions")
            .Select(x => x.ToInterfaceBuilder()
                .WithNamespace(CurrentNamespace)
                .WithVisibility(ModelFramework.Objects.Contracts.Visibility.Public)
                .WithName($"{x.Name}{BuilderName}")
                .Chain(y => y.Properties.RemoveAll(z => z.ParentTypeFullName != x.FullName))
                .WithAll(y => y.Properties, z =>
                {
                    z.TypeName = MapCodeGenerationNamespacesToDomain(z.TypeName).Replace("System.Collections.Generic.IReadOnlyCollection", "System.Collections.Generic.List", StringComparison.Ordinal);
                    if (!z.TypeName.Contains(".Domains", StringComparison.Ordinal))
                    {
                        foreach (var mapping in GetBuilderNamespaceMappings())
                        {
                            if (z.TypeName.IndexOf($"{mapping.Key}.", StringComparison.Ordinal) > -1)
                            {
                                z.TypeName = z.TypeName.Replace($"{mapping.Key}.", $"{mapping.Value}.", StringComparison.Ordinal);
                                if (z.TypeName.EndsWith(">", StringComparison.Ordinal))
                                {
                                    z.TypeName = z.TypeName.ReplaceSuffix(">", $"{BuilderName}>", StringComparison.Ordinal);
                                }
                                else
                                {
                                    z.TypeName += BuilderName;
                                }
                            }
                        }
                    }
                    z.HasSetter = true;
                    z.SetterVisibility = null; //TODO: Find out why this is set to Private. null should be sufficient (means equal to the property visibility)
                    z.Attributes.Clear();
                })
                .Chain(y =>
                {
                    for (int i = 0; i < y.Interfaces.Count; i++)
                    {
                        y.Interfaces[i] = y.Interfaces[i].Replace("ClassFramework.CodeGeneration.Models.Abstractions.", $"ClassFramework.Domain.{BuildersName}.Abstractions.", StringComparison.Ordinal) + BuilderName;
                    }
                })
                .Build()
            ).ToArray();
}
