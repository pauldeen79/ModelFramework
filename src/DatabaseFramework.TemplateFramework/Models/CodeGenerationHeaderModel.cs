namespace DatabaseFramework.TemplateFramework.Models;

public class CodeGenerationHeaderModel
{
    public CodeGenerationHeaderModel(INameContainer databaseObject, bool createCodeGenerationHeader)
    {
        Guard.IsNotNull(databaseObject);
        CreateCodeGenerationHeader = createCodeGenerationHeader;
        Name = databaseObject.Name;
        Schema = (databaseObject as ISchemaContainer)?.Schema;
        ObjectType = databaseObject switch
        {
            Table => "Table",
            StoredProcedure => "Stored procedure",
            View => "View",
            ForeignKeyConstraint => "ForeignKey",
            _ => throw new NotSupportedException($"Unsupported database type: {databaseObject.GetType().FullName}")
        };
    }

    public bool CreateCodeGenerationHeader { get; }
    public string Name { get; }
    public string? Schema { get; }
    public string ObjectType { get;}
}
