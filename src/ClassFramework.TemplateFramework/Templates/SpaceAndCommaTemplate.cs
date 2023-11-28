﻿namespace ClassFramework.TemplateFramework.Templates;

public class SpaceAndCommaTemplate : CsharpClassGeneratorBase<NewLineViewModel>, IStringBuilderTemplate
{
    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);

        builder.Append(", ");
    }
}
