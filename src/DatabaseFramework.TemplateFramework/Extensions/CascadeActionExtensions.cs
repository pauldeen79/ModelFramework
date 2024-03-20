namespace DatabaseFramework.Database.Extensions;

public static class CascadeActionExtensions
{
    public static string ToSql(this CascadeAction instance)
        => instance switch
        {
            CascadeAction.NoAction => "NO ACTION",
            CascadeAction.Cascade => "CASCADE",
            CascadeAction.SetNull => "SET NULL",
            CascadeAction.SetDefault => "SET DEFAULT",
            _ => string.Empty,
        };
}
