namespace DatabaseFramework.Domain.Builders;

[CustomValidation(typeof(ViewValidator), nameof(ViewValidator.Validate))]
public partial class ViewBuilder
{
}
