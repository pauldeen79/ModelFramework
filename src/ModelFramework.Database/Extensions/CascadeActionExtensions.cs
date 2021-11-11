using ModelFramework.Database.Contracts;

namespace ModelFramework.Database.Extensions
{
    public static class CascadeActionExtensions
    {
        /// <summary>
        /// Converts the CascadeAction value to a string for T-SQL statements.
        /// </summary>
        /// <param name="instance">The instance.</param>
        public static string ToSql(this CascadeAction instance)
        {
            //NO ACTION | CASCADE | SET NULL | SET DEFAULT 
            switch (instance)
            {
                case CascadeAction.NoAction:
                    return "NO ACTION";
                case CascadeAction.Cascade:
                    return "CASCADE";
                case CascadeAction.SetNull:
                    return "SET NULL";
                case CascadeAction.SetDefault:
                    return "SET DEFAULT";
                default:
                    return null;
            }
        }
    }
}
