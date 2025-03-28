using System.Collections.Generic;
using System.Linq;
using ObservableCollections;
using R3;

namespace TanitakaTech.StateVariable.StateCollections
{
    public class ObservableDictionaryState<TKey, TValue> :
        IStateCollectionObserver<TValue>,
        IStateCollectionElementObserver<TKey, TValue>,
        IStateCollectionModifier<TKey, TValue>
    {
        private ObservableCollections.ObservableDictionary<TKey, TValue> ObservableDictionary { get; }

        public ObservableDictionaryState() : this(new ObservableCollections.ObservableDictionary<TKey, TValue>())
        {
        }
        
        public ObservableDictionaryState(ObservableCollections.ObservableDictionary<TKey, TValue> observableDictionary)
        {
            ObservableDictionary = observableDictionary;
        }
        
        public Observable<CollectionAddEvent<TValue>> ObserveAdd()
        {
            return ObservableDictionary.ObserveAdd()
                .Select(e => new CollectionAddEvent<TValue>(e.Index, e.Value.Value));
        }

        public Observable<CollectionRemoveEvent<TValue>> ObserveRemove()
        {
            return ObservableDictionary.ObserveRemove()
                .Select(e => new CollectionRemoveEvent<TValue>(e.Index, e.Value.Value));
        }

        public Observable<CollectionReplaceEvent<TValue>> ObserveReplace()
        {
            return ObservableDictionary.ObserveReplace()
                .Select(e => new CollectionReplaceEvent<TValue>(e.Index, e.OldValue.Value, e.NewValue.Value));
        }

        public Observable<CollectionMoveEvent<TValue>> ObserveMove()
        {
            return ObservableDictionary.ObserveMove()
                .Select(e => new CollectionMoveEvent<TValue>(e.OldIndex, e.NewIndex, e.Value.Value));
        }

        public Observable<Unit> ObserveReset()
        {
            return ObservableDictionary.ObserveReset().AsUnitObservable();
        }

        public Observable<int> ObserveCountChanged()
        {
            return ObservableDictionary.ObserveCountChanged();
        }

        public Observable<TValue> ObserveElement(TKey id)
        {
            var replaceObservable = ObservableDictionary.ObserveReplace()
                .Where(e => e.NewValue.Key.Equals(id))
                .Select(e => e.NewValue.Value);
            var resetObservable = ObservableDictionary.ObserveReset()
                .Select(_ => (TValue)default);
            var addObservable = ObservableDictionary.ObserveAdd()
                .Where(e => e.Value.Key.Equals(id))
                .Select(e => e.Value.Value);
            var removeObservable = ObservableDictionary.ObserveRemove()
                .Where(e => e.Value.Key.Equals(id))
                .Select(_ => (TValue)default);
            
            return Observable.Merge(replaceObservable, resetObservable, addObservable, removeObservable)
                .Prepend(ReadElement(id))
                .DistinctUntilChanged();
        }

        public TValue ReadElement(TKey id)
        {
            return ObservableDictionary[id];
        }
        
        public ElementReadResult TryReadElement(TKey id, out TValue element)
        {
            if (ObservableDictionary.TryGetValue(id, out var value))
            {
                element = value;
                return ElementReadResult.Success;
            }
            element = default;
            return ElementReadResult.NotFound;
        }
        
        public IEnumerable<TValue> ReadAllElements()
        {
            return ObservableDictionary.AsEnumerable()
                .Select(pair => pair.Value);
        }

        public void SetElement(TKey id, TValue newElement)
        {
            ObservableDictionary[id] = newElement;
        }

        public void SetElementWithPrevious(TKey id, System.Func<(bool hasPreviousValue, TValue previousElement), TValue> newElementSelector)
        {
            bool hasPreviousValue = ObservableDictionary.TryGetValue(id, out var previous);
            ObservableDictionary[id] = newElementSelector((hasPreviousValue, previous));
        }

        public void Add(TKey id, TValue element)
        {
            ObservableDictionary.Add(id, element);
        }

        public void Remove(TKey id)
        {
            ObservableDictionary.Remove(id);
        }

        public void Replace(TKey id, TValue newElement)
        {
            SetElement(id, newElement);
        }

        public void Reset(IEnumerable<KeyValuePair<TKey, TValue>> newCollection)
        {
            ObservableDictionary.Clear();
            if (newCollection == null) return;
            
            foreach (var pair in newCollection)
            {
                ObservableDictionary.Add(pair);
            }
        }

        public void Reset()
        {
            ObservableDictionary.Clear();
        }
    }
}