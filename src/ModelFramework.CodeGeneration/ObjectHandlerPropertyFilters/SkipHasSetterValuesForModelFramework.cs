namespace ModelFramework.CodeGeneration.ObjectHandlerPropertyFilters;

public class SkipHasSetterValuesForModelFramework : IObjectHandlerPropertyFilter
{
    public bool IsValid(ObjectHandlerRequest command, PropertyInfo propertyInfo)
    {
        var classPropertyBuilder = command.Instance as ClassPropertyBuilder;
        if (classPropertyBuilder != null)
        {
            if (propertyInfo.Name == nameof(ClassPropertyBuilder.HasSetter))
            {
                return false;
            }
        }
        return true;
    }
}
