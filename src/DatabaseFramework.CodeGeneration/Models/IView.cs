﻿namespace DatabaseFramework.CodeGeneration.Models;

internal interface IView : ISchemaContainer, INameContainer, IMetadataContainer
{
    [Required][MinCount(1)] IReadOnlyCollection<IViewField> SelectFields { get; }
    [Required] IReadOnlyCollection<IViewOrderByField> OrderByFields { get; }
    [Required] IReadOnlyCollection<IViewField> GroupByFields { get; }
    [Required][MinCount(1)] IReadOnlyCollection<IViewSource> Sources { get; }
    [Required] IReadOnlyCollection<IViewCondition> Conditions { get; }
    int? Top { get; }
    bool TopPercent { get; }
    bool Distinct { get; }
    [Required] string Definition { get; }
}
