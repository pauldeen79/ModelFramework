namespace DatabaseFramework.CodeGeneration.Models;

internal interface ISchema : INameContainer, IMetadataContainer
{
    [Required] IReadOnlyCollection<ITable> Tables { get; }
    [Required] IReadOnlyCollection<IStoredProcedure> StoredProcedures { get; }
    [Required] IReadOnlyCollection<IView> Views { get; }
}
