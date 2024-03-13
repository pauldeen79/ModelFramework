namespace DatabaseFramework.CodeGeneration.Models;

public interface ISchema : INameContainer, IMetadataContainer
{
    [Required] IReadOnlyCollection<ITable> Tables { get; }
    [Required] IReadOnlyCollection<IStoredProcedure> StoredProcedures { get; }
    [Required] IReadOnlyCollection<IView> Views { get; }
}
