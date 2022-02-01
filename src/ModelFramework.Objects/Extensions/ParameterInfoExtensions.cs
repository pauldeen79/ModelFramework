namespace ModelFramework.Objects.Extensions;

public static class ParameterInfoExtensions
{
    public static bool IsNullable(this ParameterInfo parameter)
        => NullableHelper.IsNullable(parameter.ParameterType, parameter.Member, parameter.CustomAttributes);
}
