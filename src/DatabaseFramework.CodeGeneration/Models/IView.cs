namespace DatabaseFramework.CodeGeneration.Models;

internal interface IView : IDatabaseObject
{
    [Required][MinCount(1)] IReadOnlyCollection<IViewSelectField> SelectFields { get; }
    [Required] IReadOnlyCollection<IViewOrderByField> OrderByFields { get; }
    [Required] IReadOnlyCollection<IViewGroupByField> GroupByFields { get; }
    [Required][MinCount(1)] IReadOnlyCollection<IViewSource> Sources { get; }
    [Required] IReadOnlyCollection<IViewSelectCondition> Conditions { get; }
    [Required] IReadOnlyCollection<IViewGroupByCondition> GroupByConditions { get; }
    int? Top { get; }
    bool TopPercent { get; }
    bool Distinct { get; }
    [Required(AllowEmptyStrings = true)] string Definition { get; }
}
