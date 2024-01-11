﻿namespace ClassFramework.IntegrationTests.CodeGenerationProviders;

public class AbstractionsInterfaces : TestCodeGenerationProviderBase
{
    public AbstractionsInterfaces(ICsharpExpressionCreator csharpExpressionCreator, IPipeline<IConcreteTypeBuilder, BuilderContext> builderPipeline, IPipeline<IConcreteTypeBuilder, EntityContext> entityPipeline, IPipeline<TypeBaseBuilder, ReflectionContext> reflectionPipeline) : base(csharpExpressionCreator, builderPipeline, entityPipeline, reflectionPipeline)
    {
    }

    public override IEnumerable<TypeBase> Model => GetAbstractionsInterfaces();

    public override string Path => "ClassFramework.Domain.POC/Abstractions";
    public override bool RecurseOnDeleteGeneratedFiles => false;
    public override string LastGeneratedFilesFilename => string.Empty;
    public override Encoding Encoding => Encoding.UTF8;
}
