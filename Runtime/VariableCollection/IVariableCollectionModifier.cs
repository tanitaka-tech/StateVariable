using System.Collections.Generic;

namespace TanitakaTech.StateVariable.VariableCollection
{
    public interface IVariableCollectionModifier<T, ID>
    {
        void Add(T element, ID id = default);
        void Remove(ID id);
        void Replace(T newElement, ID id);
        void Reset(IEnumerable<KeyValuePair<ID, T>> newCollection = null);
    }
}