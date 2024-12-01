using System.Collections.Generic;

namespace TanitakaTech.StateVariable.StateCollections
{
    public interface IStateCollectionReader<T> : IStateReader<IEnumerable<T>>
    {
        IEnumerable<T> ReadAllElements();
        
        IEnumerable<T> IStateReader<IEnumerable<T>> .Read()
        {
            return ReadAllElements();
        }
    }
}