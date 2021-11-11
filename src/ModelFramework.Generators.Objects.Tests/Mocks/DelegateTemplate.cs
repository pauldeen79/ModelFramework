using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace ModelFramework.Generators.Objects.Tests.Mocks
{
    [ExcludeFromCodeCoverage]
    public class DelegateTemplate : CSharpClassGeneratorBase
    {
        public Action<StringBuilder> RenderDelegate { get; internal set; }

        public virtual void Render(StringBuilder builder)
        {
            RenderDelegate.Invoke(builder);
        }
    }
}
