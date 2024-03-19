namespace DatabaseFramework.TemplateFramework.ViewModels;

public class ViewViewModel : DatabaseSchemaGeneratorViewModelBase<View>, INameContainer
{
    public string Schema
        => GetModel().Schema.FormatAsDatabaseIdentifier();

    public string Name
        => GetModel().Name.FormatAsDatabaseIdentifier();

    public string Definition
        => GetModel().Definition;

    public string Distinct
        => GetModel().Distinct
            ? " DISTINCT"
            : string.Empty;

    public string Top
    {
        get
        {
            var model = GetModel();
            if (model.Top is null)
            {
                return string.Empty;
            }

            var percent = model.TopPercent
                    ? " PERCENT"
                    : string.Empty;

            return $" TOP {model.Top.ToString(Settings.CultureInfo)}{percent}";
        }
    }

    public IReadOnlyCollection<ViewSelectField> SelectFields
        => GetModel().SelectFields;

    public IReadOnlyCollection<ViewSelectCondition> Conditions
        => GetModel().Conditions;

    public IReadOnlyCollection<ViewGroupByField> GroupByFields
        => GetModel().GroupByFields;

    public IReadOnlyCollection<ViewOrderByField> OrderByFields
        => GetModel().OrderByFields;

    public IReadOnlyCollection<ViewGroupByCondition> GroupByConditions
        => GetModel().GroupByConditions;

    public CodeGenerationHeaderModel CodeGenerationHeaders
        => new CodeGenerationHeaderModel(GetModel(), Settings.CreateCodeGenerationHeader);
}
