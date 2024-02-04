namespace ClassFramework.Pipelines.Shared;

public class PipelineBuilderCopySettings
{
    public bool CopyAttributes { get; }
    public bool CopyInterfaces { get; }
    public Predicate<Domain.Attribute>? CopyAttributePredicate { get; }
    public Predicate<string>? CopyInterfacePredicate { get; }

    public PipelineBuilderCopySettings(
        bool copyAttributes = false,
        bool copyInterfaces = false,
        Predicate<Domain.Attribute>? copyAttributePredicate = null,
        Predicate<string>? copyInterfacePredicate = null)
    {
        CopyAttributes = copyAttributes;
        CopyInterfaces = copyInterfaces;
        CopyAttributePredicate = copyAttributePredicate;
        CopyInterfacePredicate = copyInterfacePredicate;
    }
}
