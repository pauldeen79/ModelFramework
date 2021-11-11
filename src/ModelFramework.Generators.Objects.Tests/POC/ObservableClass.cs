using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace ModelFramework.Generators.Tests.POC
{
    [ExcludeFromCodeCoverage]
    public class ObservableClass : INotifyPropertyChanged
    {
        #region Template for non-enumerable property
        private string _property1;

        public string Property1
        {
            get
            {
                return _property1;
            }
            set
            {
                _property1 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Property1)));
            }
        }
        #endregion

        #region Template for enumerable property (change to ICollection<T>)
        public ICollection<string> Property2 { get; set; }
        #endregion

        #region Fixed
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        public ObservableClass()
        {
            #region Iterate all enumerables, and initialie with ObservableCollection<T>
            Property2 = new ObservableCollection<string>();
            #endregion
        }
    }
}
