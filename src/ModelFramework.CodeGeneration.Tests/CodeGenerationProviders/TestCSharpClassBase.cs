﻿namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public abstract partial class TestCSharpClassBase : ModelFrameworkCSharpClassBase
{
    protected override bool InheritFromInterfaces => false;

    protected override void FixImmutableClassProperties(InterfaceBuilder interfaceBuilder) => FixInterface(interfaceBuilder);
    protected override void FixImmutableClassProperties(ClassBuilder classBuilder) => FixClass(classBuilder);
    protected override void FixImmutableBuilderProperties(InterfaceBuilder interfaceBuilder) => FixInterface(interfaceBuilder);
    protected override void FixImmutableBuilderProperties(ClassBuilder classBuilder) => FixClass(classBuilder);

    private static void FixInterface(InterfaceBuilder interfaceBuilder)
    {
        if (interfaceBuilder == null)
        {
            // Not possible, but needs to be added because TTTF.Runtime doesn't support nullable reference types
            return;
        }

        foreach (var property in interfaceBuilder.Properties)
        {
            FixImmutableBuilderProperty(property);
        }
    }

    private static void FixClass(ClassBuilder classBuilder)
    {
        if (classBuilder == null)
        {
            // Not possible, but needs to be added because TTTF.Runtime doesn't support nullable reference types
            return;
        }

        foreach (var property in classBuilder.Properties)
        {
            FixImmutableBuilderProperty(property);
        }
    }

    private static void FixImmutableBuilderProperty(ClassPropertyBuilder property)
    {
        var typeName = property.TypeName.FixTypeName();
        if (typeName.StartsWith("ModelFramework.Common.Contracts.Test.I", StringComparison.InvariantCulture))
        {
            property.TypeName = typeName.Replace("Contracts.Test.I", "Test.", StringComparison.InvariantCulture);
        }
        else if (typeName.Contains("Collection<ModelFramework.", StringComparison.InvariantCulture))
        {
            property.TypeName = typeName.Replace("Contracts.Test.I", "Test.", StringComparison.InvariantCulture);
        }
    }
}