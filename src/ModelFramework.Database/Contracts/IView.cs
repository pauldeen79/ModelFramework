using System.Collections.Generic;
using ModelFramework.Common.Contracts;

namespace ModelFramework.Database.Contracts
{
    public interface IView : INameContainer, IMetadataContainer
    {
        IReadOnlyCollection<IViewField> SelectFields { get; }
        IReadOnlyCollection<IViewOrderByField> OrderByFields { get; }
        IReadOnlyCollection<IViewField> GroupByFields { get; }
        IReadOnlyCollection<IViewSource> Sources { get; }
        IReadOnlyCollection<IViewCondition> Conditions { get; }
        //bool WithCheckOption { get; }
        //bool Encryption { get; }
        //bool SchemaBinding { get; }
        //bool ViewMetadata { get; }
        int? Top { get; }
        bool TopPercent { get; }
        bool Distinct { get; }
        string Definition { get; }
    }
}
