namespace ClassFramework.Domain.Builders;

public partial class ClassPropertyBuilder
{
    partial void SetDefaultValues()
    {
        HasGetter = true;
        HasSetter = true;
    }
}
