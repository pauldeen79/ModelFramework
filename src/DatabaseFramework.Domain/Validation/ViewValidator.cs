namespace DatabaseFramework.Domain.Validation;

public static class ViewValidator
{
    public static ValidationResult Validate(object instance)
    {
        if (instance is null)
        {
            return ValidationResult.Success;
        }

        if (!instance.GetType().Name.In(nameof(View), nameof(ViewBuilder)))
        {
            return new ValidationResult($"The {nameof(ViewValidator)} attribute can only be applied to {nameof(View)} and {nameof(ViewBuilder)} types");
        }

        var definition = instance.GetType().GetProperty(nameof(View.Definition)).GetValue(instance) as string;
        var selectFieldsCount = (instance.GetType().GetProperty(nameof(View.SelectFields)).GetValue(instance) as IEnumerable)?.OfType<object>().Count() ?? 0;
        var sourcesCount = (instance.GetType().GetProperty(nameof(View.Sources)).GetValue(instance) as IEnumerable)?.OfType<object>().Count() ?? 0;

        if (string.IsNullOrEmpty(definition)
            && selectFieldsCount == 0
            && sourcesCount == 0)
        {
            return new ValidationResult($"Either {nameof(View.Definition)} or {nameof(View.SelectFields)} and {nameof(View.Sources)} are required", [nameof(View.Definition), nameof(View.SelectFields), nameof(View.Sources)]);
        }

        if (!string.IsNullOrEmpty(definition)
            && (selectFieldsCount > 0 || sourcesCount > 0))
        {
            return new ValidationResult($"When {nameof(View.Definition)} is filled, then {nameof(View.SelectFields)} and {nameof(View.Sources)} need to be empty", [nameof(View.Definition), nameof(View.SelectFields), nameof(View.Sources)]);
        }

        if (selectFieldsCount > 0
            && sourcesCount > 0
            && !string.IsNullOrEmpty(definition))
        {
            return new ValidationResult($"When {nameof(View.SelectFields)} and {nameof(View.Sources)} are filled, then {nameof(View.Definition)} needs to be empty", [nameof(View.Definition), nameof(View.SelectFields), nameof(View.Sources)]);
        }

        if ((selectFieldsCount > 0 && sourcesCount == 0)
            || (selectFieldsCount == 0 && sourcesCount > 0))
        {
            return new ValidationResult($"When {nameof(View.SelectFields)} or {nameof(View.Sources)} is filled, then both fields are required", [nameof(View.SelectFields), nameof(View.Sources)]);
        }

        // When we get to here, then either Definition is filled, or both SelectFields and Sources are filled.
        // That's valid.

        return ValidationResult.Success;
    }
}
