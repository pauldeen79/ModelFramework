using CrossCutting.Common;
using ModelFramework.Common.Contracts;

namespace ModelFramework.Database.Contracts
{
    public interface IView : INameContainer, IMetadataContainer
    {
        ValueCollection<IViewField> SelectFields { get; }
        ValueCollection<IViewOrderByField> OrderByFields { get; }
        ValueCollection<IViewField> GroupByFields { get; }
        ValueCollection<IViewSource> Sources { get; }
        ValueCollection<IViewCondition> Conditions { get; }
        int? Top { get; }
        bool TopPercent { get; }
        bool Distinct { get; }
        string Definition { get; }
    }
}
