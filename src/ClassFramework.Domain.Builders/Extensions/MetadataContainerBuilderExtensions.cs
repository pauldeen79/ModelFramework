﻿namespace ClassFramework.Domain.Builders.Extensions;

public static class MetadataContainerBuilderExtensions
{
    public static T AddMetadata<T>(this T instance, string name, object? value) where T : IMetadataContainerBuilder
    {
        instance.Metadata.Add(new MetadataBuilder().WithName(name).WithValue(value));
        return instance;
    }
}
