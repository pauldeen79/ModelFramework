namespace ModelFramework.Objects.Extensions;

public static class MethodInfoExtensions
{
    public static bool ReturnTypeIsNullable(this MethodInfo methodInfo)
        => NullableHelper.IsNullable(methodInfo.ReturnType, methodInfo.ReturnParameter.Member, methodInfo.CustomAttributes, 0);
}
