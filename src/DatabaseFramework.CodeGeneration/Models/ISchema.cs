namespace DatabaseFramework.CodeGeneration.Models;

internal interface ISchema : Abstractions.INameContainer, Abstractions.IMetadataContainer
{
    [Required] IReadOnlyCollection<ITable> Tables { get; }
    [Required] IReadOnlyCollection<IStoredProcedure> StoredProcedures { get; }
    [Required] IReadOnlyCollection<IView> Views { get; }
}
