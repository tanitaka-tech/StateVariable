using System.Collections.Generic;

namespace TanitakaTech.StateVariable.VariableCollection
{
    public interface IVariableCollectionReader<T> : IVariableReader<IEnumerable<T>>
    {
        IEnumerable<T> ReadAllElements();
        
        IEnumerable<T> IVariableReader<IEnumerable<T>> .Read()
        {
            return ReadAllElements();
        }
    }
}