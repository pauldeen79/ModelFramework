namespace ModelFramework.Objects.Contracts;

public interface IOverload : IParametersContainer
{
    /// <summary>
    /// Format string for method name.
    /// </summary>
    /// <remarks>
    /// Leave empty to use default method name template, as specified in settings.
    /// Use {0} for property name (unchanged casing)</remarks>
    string MethodName { get; }
    /// <summary>
    /// Initialize expression for overload.
    /// </summary>
    /// <remarks>
    /// For single properties, use:
    /// {0} for pascal cased property name
    /// {1} for typename
    /// {2} for property name (unchanged casing)
    /// For collection properties, use:
    /// {0} for pascal cased property name
    /// {1} for typename
    /// {2} for generic agument of type
    /// {3} for indent
    /// {4} for property name (unchanged casing)
    /// </remarks>
    string InitializeExpression { get; }
}
