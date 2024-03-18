namespace DatabaseFramework.Domain;

[CustomValidation(typeof(ViewValidator), nameof(ViewValidator.Validate))]
public partial record View
{
}
