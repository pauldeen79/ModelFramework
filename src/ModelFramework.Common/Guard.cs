﻿namespace ModelFramework.Common;

public static class Guard
{
    public static void AgainstNull(object value, string argumentName)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(argumentName));
        }
    }

    public static T AgainstNull<T>(T instance, string argumentName)
        => instance is null
            ? throw new ArgumentNullException(nameof(argumentName))
            : instance;
}
