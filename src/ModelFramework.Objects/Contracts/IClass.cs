using CrossCutting.Common;
using ModelFramework.Objects.Settings;

namespace ModelFramework.Objects.Contracts
{
    public interface IClass : ITypeBase, IEnumsContainer
    {
        ValueCollection<IClassField> Fields { get; }
        bool Static { get; }
        bool Sealed { get; }
        ValueCollection<IClass> SubClasses { get; }
        ValueCollection<IClassConstructor> Constructors { get; }
        string BaseClass { get; }
        bool AutoGenerateInterface { get; }
        InterfaceSettings AutoGenerateInterfaceSettings { get; }
        bool Record { get; }
    }
}
