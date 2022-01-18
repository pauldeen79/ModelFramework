namespace ModelFramework.Database.Contracts
{
    public interface IViewOrderByField : IViewField
    {
        bool IsDescending { get; }
    }
}
