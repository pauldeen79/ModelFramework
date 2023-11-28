﻿namespace ClassFramework.TemplateFramework.ViewModels;

public class EnumerationViewModel : CsharpClassGeneratorViewModel<Enumeration>
{
    public EnumerationViewModel(Enumeration data, CsharpClassGeneratorSettings settings) : base(data, settings)
    {
    }

    public string Name => Data.Name.Sanitize().GetCsharpFriendlyName();
}