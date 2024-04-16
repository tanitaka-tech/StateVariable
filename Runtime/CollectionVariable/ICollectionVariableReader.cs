using System.Collections.Generic;

namespace TanitakaTech.StateVariable.CollectionVariable
{
    public interface ICollectionVariableReader<T> : IVariableReader<IEnumerable<T>>
    {
        IEnumerable<T> ReadAllElements();
        
        IEnumerable<T> IVariableReader<IEnumerable<T>> .Read()
        {
            return ReadAllElements();
        }
    }
}