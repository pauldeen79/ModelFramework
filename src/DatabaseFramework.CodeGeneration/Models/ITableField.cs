namespace DatabaseFramework.CodeGeneration.Models;

internal interface ITableField : INonViewField, ICheckConstraintContainer
{
    bool IsIdentity { get; }
    bool IsRequired { get; }
}
