﻿namespace ModelFramework.CodeGeneration.ObjectHandlers;

public class BuilderObjectHandler : IObjectHandler
{
    public bool ProcessInstance(ObjectHandlerRequest command, ICsharpExpressionDumperCallback callback)
    {
        var type = command.Type ?? command.InstanceType;
        if (type == null)
        {
            return false;
        }

        var level = command.Level + 1;
        var first = true;
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var processedProperties = new List<string>();

        foreach (var property in properties.Where(x => callback.IsPropertyValid(command, x)))
        {
            if (type.GetMethods().Any(x => x.Name == $"With{property.Name}"))
            {
                first = ProcessBuilderMethod(command, callback, level, first, processedProperties, property, "With");
            }
            else if (type.GetMethods().Any(x => x.Name == $"Add{property.Name}"))
            {
                first = ProcessBuilderMethod(command, callback, level, first, processedProperties, property, "Add");
            }
        }

        if (first)
        {
            // No 'With' or 'Add' methods found, let the default object handler handle this
            return false;
        }

        AppendFinalize(command, callback, properties, processedProperties);

        return true;
    }

    private static bool ProcessBuilderMethod(ObjectHandlerRequest command,
                                             ICsharpExpressionDumperCallback callback,
                                             int level,
                                             bool first,
                                             List<string> processedProperties,
                                             PropertyInfo property,
                                             string methodPrefix)
    {
        if (first)
        {
            callback.Append("()");
            first = false;
        }
        var propertyValue = property.GetValue(command.Instance);

        var addedSomething = false;
        if (methodPrefix == "Add"
            && !(propertyValue is string)
            && propertyValue is IEnumerable enumerable)
        {
            var firstVal = true;
            level++;
            foreach (var value in enumerable.OfType<object>())
            {
                if (firstVal)
                {
                    firstVal = false;
                    addedSomething = true;
                    callback.ChainAppendLine()
                            .ChainAppend(new string(' ', (level - 1) * 4))
                            .ChainAppend($".{methodPrefix}{property.Name}(")
                            .ChainAppendLine()
                            .ChainAppend(new string(' ', level * 4));
                }
                else
                {
                    callback.ChainAppend(",")
                            .ChainAppendLine()
                            .ChainAppend(new string(' ', level * 4));
                }
                callback.ProcessRecursive(value, value?.GetType(), level);
            }
        }
        else
        {
            addedSomething = true;
            callback.ChainAppendLine()
                    .ChainAppend(new string(' ', level * 4))
                    .ChainAppend($".{methodPrefix}{property.Name}(")
                    .ChainProcessRecursive(propertyValue, propertyValue?.GetType(), level);
        }
        if (addedSomething)
        {
            callback.Append(")");
        }
        processedProperties.Add(property.Name);
        return first;
    }

    private static void AppendFinalize(ObjectHandlerRequest command,
                                       ICsharpExpressionDumperCallback callback,
                                       PropertyInfo[] properties,
                                       List<string> processedProperties)
    {
        var writableProperties = properties.Where
        (
            property =>
                (command.IsAnonymousType || !property.IsReadOnly())
                && !processedProperties.Contains(property.Name)
                && callback.IsPropertyValid(command, property)
        ).ToArray();

        if (writableProperties.Length > 0)
        {
            ProcessWritableProperties(command, callback, writableProperties);
        }
    }

    private static void ProcessWritableProperties(ObjectHandlerRequest command,
                                                  ICsharpExpressionDumperCallback callback,
                                                  IEnumerable<PropertyInfo> properties)
    {
        callback.ChainAppendLine()
                .ChainAppend(new string(' ', command.Level * 4))
                .ChainAppendLine("{");

        var level = command.Level + 1;

        foreach (var property in properties)
        {
            var propertyValue = property.GetValue(command.Instance);
            var propertyType = propertyValue?.GetType();
            callback.Append(new string(' ', level * 4));
            var propertyCommand = new CustomTypeHandlerRequest(propertyValue, propertyType, level);
            var propertyIsCustom = callback.IsPropertyCustom(propertyCommand, $"{property.Name} = ", ",");
            if (!propertyIsCustom)
            {
                callback.ChainAppend(property.Name)
                        .ChainAppend(" = ")
                        .ChainProcessRecursive(propertyValue, propertyValue?.GetType(), level)
                        .ChainAppend(",");
            }

            callback.AppendLine();
        }

        level--;

        callback.ChainAppend(new string(' ', level * 4))
                .ChainAppend("}");
    }
}
