namespace ModelFramework.Objects.Extensions;

public static class FieldInfoExtensions
{
    public static bool IsNullable(this FieldInfo fieldInfo)
        => NullableHelper.IsNullable(fieldInfo.FieldType, fieldInfo.DeclaringType, fieldInfo.CustomAttributes, 0);
}
