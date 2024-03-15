namespace DatabaseFramework.Domain.Builders;

public partial class TableBuilder
{
    partial void SetDefaultValues()
    {
        Schema = "dbo";
    }
}
