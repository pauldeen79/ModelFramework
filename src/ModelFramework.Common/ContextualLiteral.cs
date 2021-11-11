using System;

namespace ModelFramework.Common
{
    public class ContextualLiteral : Literal
    {
        private readonly Func<object, object, bool> _conditionDelegate;
        private readonly Func<object, object, string> _transformationDelegate;

        public ContextualLiteral(string value, Func<object, object, bool> conditionDelegate, object originalValue = null, Func<object, object, string> transformationDelegate = null)
            : base(value, originalValue)
        {
            _conditionDelegate = conditionDelegate;
            _transformationDelegate = transformationDelegate;
        }

        public string ToString(object template, object model)
        {
            return _conditionDelegate(template, model)
                ? Transform(template, model)
                : null;
        }

        private string Transform(object template, object model)
            => _transformationDelegate == null
                ? ToString()
                : _transformationDelegate(template, model);
    }
}
