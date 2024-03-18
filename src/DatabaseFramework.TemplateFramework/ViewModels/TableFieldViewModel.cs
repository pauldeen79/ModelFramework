namespace DatabaseFramework.TemplateFramework.ViewModels;

public class TableFieldViewModel : DatabaseSchemaGeneratorViewModelBase<TableField>
{
    public string Name
        => GetModel().Name.FormatAsDatabaseIdentifier();

    public string Identity
        => GetModel().IsIdentity
            ? " IDENTITY(1, 1)"
            : string.Empty;

    public string NullOrNotNull
        => GetModel().IsRequired
            ? "NOT NULL"
            : "NULL";

    public bool HasCheckConstraints
        => GetModel().CheckConstraints.Count > 0;

    public bool IsLastTableField
        => Context.IsLastIteration
            ?? throw new InvalidOperationException("Can only render table fields as part of a table. There is no context with hierarchy.");

    public IReadOnlyCollection<CheckConstraint> CheckConstraints
        => GetModel().CheckConstraints;

    public string Type
    {
        get
        {
            var model = GetModel();
            var builder = new StringBuilder();
            builder.Append($"{model.Type.ToString().ToLower(Settings.CultureInfo)}");

            if (model.Type.IsDatabaseStringType())
            {
                builder.Append("(");
                if (model.IsStringMaxLength == true)
                {
                    builder.Append("max");
                }
                else
                {
                    builder.Append(model.StringLength.GetValueOrDefault(32).ToString(Settings.CultureInfo));
                }
                builder.Append(")");
                if (!string.IsNullOrEmpty(model.StringCollation))
                {
                    builder.Append($" COLLATE {model.StringCollation}");
                }
            }
            else if (model.NumericPrecision is not null && model.NumericScale is not null)
            {
                builder.Append($"({model.NumericPrecision.GetValueOrDefault(8).ToString(Settings.CultureInfo)},{model.NumericScale.GetValueOrDefault(0).ToString(Settings.CultureInfo)})");
            }

            return builder.ToString();
        }
    }
}
