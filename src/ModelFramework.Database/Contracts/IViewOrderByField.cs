namespace ModelFramework.Database.Contracts
{
    public interface IViewOrderByField : IViewField
    {
        bool Descending { get; }
    }
}
