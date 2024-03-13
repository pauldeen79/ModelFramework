namespace DatabaseFramework.Domain.Domains;

/// <summary>
/// Specifies the cascade action on update or delete.
/// </summary>
public enum CascadeAction
{
    /// <summary>
    /// Take no action
    /// </summary>
    NoAction = 0,

    /// <summary>
    /// Cascade
    /// </summary>
    Cascade = 1,

    /// <summary>
    /// Set to null
    /// </summary>
    SetNull = 2,

    /// <summary>
    /// Set default value
    /// </summary>
    SetDefault = 3
}
