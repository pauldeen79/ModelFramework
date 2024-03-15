namespace DatabaseFramework.Domain.Builders;

public partial class StoredProcedureBuilder
{
    partial void SetDefaultValues()
    {
        Schema = "dbo";
    }
}
