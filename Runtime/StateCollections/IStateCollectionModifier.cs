using System.Collections.Generic;
using System.Linq;

namespace TanitakaTech.StateVariable.StateCollections
{
    public interface IStateCollectionModifier<TKey, TValue> : IStateCollectionElementSetter<TKey, TValue>
    {
        void Add(TKey id, TValue newElement);
        void Remove(TKey id);
        void Replace(TKey id, TValue newElement);
        void Reset(IEnumerable<KeyValuePair<TKey, TValue>> newCollection = null);

        void Reset(IEnumerable<(TKey, TValue)> newCollection = null)
        {
            Reset(newCollection?.Select(pair => KeyValuePair.Create(pair.Item1, pair.Item2)));
        }
    }
}