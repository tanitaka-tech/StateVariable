using System.Collections.Generic;
using ObservableCollections;
using R3;

namespace TanitakaTech.StateVariable.StateCollections
{
    public class ObservableHashSetState<TKey> :
        IStateCollectionObserver<TKey>,
        IStateCollectionElementObserver<TKey, TKey>,
        IStateCollectionModifier<TKey, TKey>
    {
        private readonly ObservableHashSet<TKey> _observableHashset;

        public ObservableHashSetState() : this(new ObservableHashSet<TKey>())
        {
        }
        
        public ObservableHashSetState(ObservableHashSet<TKey> observableHashset)
        {
            _observableHashset = observableHashset;
        }
        
        public Observable<CollectionAddEvent<TKey>> ObserveAdd()
        {
            return _observableHashset.ObserveAdd()
                .Select(e => new CollectionAddEvent<TKey>(e.Index, e.Value));
        }

        public Observable<CollectionRemoveEvent<TKey>> ObserveRemove()
        {
            return _observableHashset.ObserveRemove()
                .Select(e => new CollectionRemoveEvent<TKey>(e.Index, e.Value));
        }

        public Observable<CollectionReplaceEvent<TKey>> ObserveReplace()
        {
            return _observableHashset.ObserveReplace()
                .Select(e => new CollectionReplaceEvent<TKey>(e.Index, e.OldValue, e.NewValue));
        }

        public Observable<CollectionMoveEvent<TKey>> ObserveMove()
        {
            return _observableHashset.ObserveMove()
                .Select(e => new CollectionMoveEvent<TKey>(e.OldIndex, e.NewIndex, e.Value));
        }

        public Observable<Unit> ObserveReset()
        {
            return _observableHashset.ObserveReset().AsUnitObservable();
        }

        public Observable<int> ObserveCountChanged()
        {
            return _observableHashset.ObserveCountChanged();
        }

        public Observable<TKey> ObserveElement(TKey id)
        {
            var replaceObservable = _observableHashset.ObserveReplace()
                .Where(e => e.NewValue.Equals(id))
                .Select(e => e.NewValue);
            var resetObservable = _observableHashset.ObserveReset()
                .Select(_ => (TKey)default);
            var addObservable = _observableHashset.ObserveAdd()
                .Where(e => e.Value.Equals(id))
                .Select(e => e.Value);
            var removeObservable = _observableHashset.ObserveRemove()
                .Where(e => e.Value.Equals(id))
                .Select(_ => (TKey)default);
            
            return Observable.Merge(replaceObservable, resetObservable, addObservable, removeObservable)
                .Prepend(ReadElement(id))
                .DistinctUntilChanged();
        }

        public TKey ReadElement(TKey id)
        {
            if(_observableHashset.Contains(id)) return id;
            throw new KeyNotFoundException();
        }
        
        public ElementReadResult TryReadElement(TKey id, out TKey element)
        {
            if (_observableHashset.TryGetValue(id, out var value))
            {
                element = value;
                return ElementReadResult.Success;
            }
            element = default;
            return ElementReadResult.NotFound;
        }
        
        public IEnumerable<TKey> ReadAllElements()
        {
            return _observableHashset;
        }

        public void SetElement(TKey id, TKey newElement)
        {
            _observableHashset.Add(newElement);
        }

        public void SetElementWithPrevious(TKey id, System.Func<(bool hasPreviousValue, TKey previousElement), TKey> newElementSelector)
        {
            bool hasPreviousValue = _observableHashset.TryGetValue(id, out var previous);
            _observableHashset.Add(newElementSelector((hasPreviousValue, previous)));
        }

        public void Add(TKey id, TKey element)
        {
            _observableHashset.Add(element);
        }

        public void Remove(TKey id)
        {
            _observableHashset.Remove(id);
        }

        public void Replace(TKey id, TKey newElement)
        {
            SetElement(id, newElement);
        }

        public void Reset(IEnumerable<KeyValuePair<TKey, TKey>> newCollection)
        {
            _observableHashset.Clear();
            if (newCollection == null) return;
            
            foreach (var pair in newCollection)
            {
                _observableHashset.Add(pair.Key);
            }
        }

        public void Reset()
        {
            _observableHashset.Clear();
        }
    }
}