using System.Collections.Generic;

namespace TanitakaTech.StateVariable.CollectionVariable
{
    public interface ICollectionVariableReader<T>
    {
        IEnumerable<T> ReadAllElements();
    }
}