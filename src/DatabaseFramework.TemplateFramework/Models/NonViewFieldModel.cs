namespace DatabaseFramework.TemplateFramework.Models;

public class NonViewFieldModel
{
    public NonViewFieldModel(INonViewField value)
    {
        Guard.IsNotNull(value);

        Value = value;
    }

    public INonViewField Value { get; }
}
