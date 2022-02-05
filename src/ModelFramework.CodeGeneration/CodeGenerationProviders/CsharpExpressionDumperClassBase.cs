﻿namespace ModelFramework.CodeGeneration.CodeGenerationProviders;

public abstract class CSharpExpressionDumperClassBase : ClassBase
{
    protected override bool CreateCodeGenerationHeader => true;
    protected override bool EnableNullableContext => true;

    protected abstract string[] NamespacesToAbbreviate { get; }
    protected abstract Type[] Models { get; }
    protected abstract string Namespace { get; }
    protected abstract string ClassName { get; }

    public override object CreateModel()
        => new[]
        {
            new ClassBuilder()
                .WithNamespace(Namespace)
                .WithName(ClassName)
                .WithPartial()
                .AddUsings(NamespacesToAbbreviate)
                .AddNamespacesToAbbreviate(NamespacesToAbbreviate)
                .AddMethods(new ClassMethodBuilder()
                    .WithName("GetModels")
                    .WithProtected()
                    .WithStatic()
                    .WithType(typeof(ITypeBase[]))
                    .AddLiteralCodeStatements($"return {CreateCode()}.Select(x => x.Build()).ToArray();"))
        }
        .Select(x => x.Build())
        .ToArray();

    private string CreateCode()
    {
        var models = Models.Select(x => x.ToClassBuilder(new ClassSettings())).ToArray();
        var serviceCollection = new ServiceCollection();
        using var serviceProvider = serviceCollection
            .AddCsharpExpressionDumper
            (
                x => x.AddSingleton<IObjectHandlerPropertyFilter, SkipDefaultValuesForModelFramework>()
                      .AddSingleton<ITypeNameFormatter>(new CsharpFriendlyNameFormatter())
                      .AddSingleton<ITypeNameFormatter>(new SkipNamespacesTypeNameFormatter(NamespacesToAbbreviate))
            )
            .BuildServiceProvider();
        var dumper = serviceProvider.GetRequiredService<ICsharpExpressionDumper>();

        return dumper.Dump(models);
    }
}
