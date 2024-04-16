using System.Collections.Generic;

namespace TanitakaTech.StateVariable.CollectionVariable
{
    public interface ICollectionVariableElementReader<T, ID>
    {
        T ReadElement(ID id);
        IEnumerable<T> ReadAllElements();
    }
}