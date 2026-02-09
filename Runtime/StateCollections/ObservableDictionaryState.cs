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
        private readonly ObservableDictionary<TKey, TValue> _observableDictionary;

        public ObservableDictionaryState() : this(new ObservableCollections.ObservableDictionary<TKey, TValue>())
        {
        }

        public ObservableDictionaryState(ObservableCollections.ObservableDictionary<TKey, TValue> observableDictionary)
        {
            _observableDictionary = observableDictionary;
        }

        public Observable<CollectionAddEvent<TValue>> ObserveAdd()
        {
            return _observableDictionary.ObserveAdd()
                .Select(e => new CollectionAddEvent<TValue>(e.Index, e.Value.Value));
        }

        public Observable<CollectionRemoveEvent<TValue>> ObserveRemove()
        {
            return _observableDictionary.ObserveRemove()
                .Select(e => new CollectionRemoveEvent<TValue>(e.Index, e.Value.Value));
        }

        public Observable<CollectionReplaceEvent<TValue>> ObserveReplace()
        {
            return _observableDictionary.ObserveReplace()
                .Select(e => new CollectionReplaceEvent<TValue>(e.Index, e.OldValue.Value, e.NewValue.Value));
        }

        public Observable<CollectionMoveEvent<TValue>> ObserveMove()
        {
            return _observableDictionary.ObserveMove()
                .Select(e => new CollectionMoveEvent<TValue>(e.OldIndex, e.NewIndex, e.Value.Value));
        }

        public Observable<Unit> ObserveReset()
        {
            return _observableDictionary.ObserveReset().AsUnitObservable();
        }

        public Observable<int> ObserveCountChanged()
        {
            return _observableDictionary.ObserveCountChanged();
        }

        public Observable<Unit> ObserveAllChangesInternal()
        {
            return Observable
                .Merge(
                    ObserveAdd().AsUnitObservable()
                    , ObserveRemove().AsUnitObservable()
                    , ObserveReplace().AsUnitObservable()
                    , ObserveMove().AsUnitObservable()
                    , ObserveReset().AsUnitObservable()
                    , ObserveCountChanged().AsUnitObservable()
                    , ObserveChanged().AsUnitObservable()
                    , _observableDictionary.ObserveDictionaryAdd().AsUnitObservable()
                    , _observableDictionary.ObserveDictionaryRemove().AsUnitObservable()
                    , _observableDictionary.ObserveDictionaryReplace().AsUnitObservable()
                )
                .Prepend(Unit.Default)
                .ThrottleLastFrame(1);
        }

        public Observable<CollectionChangedEvent<KeyValuePair<TKey, TValue>>> ObserveChanged()
        {
            return _observableDictionary.ObserveChanged();
        }

        public Observable<TValue> ObserveElement(TKey id)
        {
            var replaceObservable = _observableDictionary.ObserveReplace()
                .Where(e => e.NewValue.Key.Equals(id))
                .Select(e => e.NewValue.Value);
            var resetObservable = _observableDictionary.ObserveReset()
                .Select(_ => (TValue)default);
            var addObservable = _observableDictionary.ObserveAdd()
                .Where(e => e.Value.Key.Equals(id))
                .Select(e => e.Value.Value);
            var removeObservable = _observableDictionary.ObserveRemove()
                .Where(e => e.Value.Key.Equals(id))
                .Select(_ => (TValue)default);
            
            return Observable.Merge(replaceObservable, resetObservable, addObservable, removeObservable)
                .Prepend(ReadElement(id))
                .DistinctUntilChanged();
        }

        public TValue ReadElement(TKey id)
        {
            return _observableDictionary[id];
        }
        
        public ElementReadResult TryReadElement(TKey id, out TValue element)
        {
            if (_observableDictionary.TryGetValue(id, out var value))
            {
                element = value;
                return ElementReadResult.Success;
            }
            element = default;
            return ElementReadResult.NotFound;
        }
        
        public IEnumerable<TValue> ReadAllElements()
        {
            return _observableDictionary.AsEnumerable()
                .Select(pair => pair.Value);
        }

        public void SetElement(TKey id, TValue newElement)
        {
            _observableDictionary[id] = newElement;
        }

        public void SetElementWithPrevious(TKey id, System.Func<(bool hasPreviousValue, TValue previousElement), TValue> newElementSelector)
        {
            bool hasPreviousValue = _observableDictionary.TryGetValue(id, out var previous);
            _observableDictionary[id] = newElementSelector((hasPreviousValue, previous));
        }

        public void Add(TKey id, TValue element)
        {
            _observableDictionary.Add(id, element);
        }

        public void Remove(TKey id)
        {
            _observableDictionary.Remove(id);
        }

        public void Replace(TKey id, TValue newElement)
        {
            SetElement(id, newElement);
        }

        public void Reset(IEnumerable<KeyValuePair<TKey, TValue>> newCollection)
        {
            _observableDictionary.Clear();
            if (newCollection == null) return;
            
            foreach (var pair in newCollection)
            {
                _observableDictionary.Add(pair);
            }
        }

        public void Reset()
        {
            _observableDictionary.Clear();
        }
    }
}