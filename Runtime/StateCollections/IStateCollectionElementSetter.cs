using System;

namespace TanitakaTech.StateVariable.StateCollections
{
    public interface IStateCollectionElementSetter<TKey, TValue> : IStateCollectionElementReader<TKey, TValue>
    {
        void SetElement(TKey id, TValue newElement);
        void SetElementWithPrevious(TKey id, Func<(bool hasPreviousValue, TValue previousElement), TValue> newElementSelector);
    }
}