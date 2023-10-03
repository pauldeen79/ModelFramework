namespace ClassFramework.Domain.Models;

public partial class ClassPropertyModel
{
    partial void SetDefaultValues()
    {
        HasGetter = true;
        HasSetter = true;
    }
}
