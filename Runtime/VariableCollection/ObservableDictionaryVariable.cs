using System.Collections.Generic;
using System.Linq;
using ObservableCollections;
using R3;

namespace TanitakaTech.StateVariable.VariableCollection
{
    public class ObservableDictionaryVariable<TKey, TValue> : 
        IVariableCollectionObserver<TValue>,
        IVariableCollectionElementObserver<TKey, TValue>,
        IVariableCollectionElementSetter<TKey, TValue>,
        IVariableCollectionModifier<TKey, TValue>
    {
        private ObservableCollections.ObservableDictionary<TKey, TValue> ObservableDictionary { get; }

        public ObservableDictionaryVariable(ObservableCollections.ObservableDictionary<TKey, TValue> observableDictionary)
        {
            ObservableDictionary = observableDictionary;
        }
        
        Observable<CollectionAddEvent<TValue>> IVariableCollectionObserver<TValue>.ObserveAdd()
        {
            return ObservableDictionary.ObserveAdd()
                .Select(e => new CollectionAddEvent<TValue>(e.Index, e.Value.Value));
        }

        Observable<CollectionRemoveEvent<TValue>> IVariableCollectionObserver<TValue>.ObserveRemove()
        {
            return ObservableDictionary.ObserveRemove()
                .Select(e => new CollectionRemoveEvent<TValue>(e.Index, e.Value.Value));
        }

        Observable<CollectionReplaceEvent<TValue>> IVariableCollectionObserver<TValue>.ObserveReplace()
        {
            return ObservableDictionary.ObserveReplace()
                .Select(e => new CollectionReplaceEvent<TValue>(e.Index, e.OldValue.Value, e.NewValue.Value));
        }

        Observable<CollectionMoveEvent<TValue>> IVariableCollectionObserver<TValue>.ObserveMove()
        {
            return ObservableDictionary.ObserveMove()
                .Select(e => new CollectionMoveEvent<TValue>(e.OldIndex, e.NewIndex, e.Value.Value));
        }

        Observable<Unit> IVariableCollectionObserver<TValue>.ObserveReset()
        {
            return ObservableDictionary.ObserveReset();
        }

        Observable<int> IVariableCollectionObserver<TValue>.ObserveCountChanged()
        {
            return ObservableDictionary.ObserveCountChanged();
        }

        Observable<TValue> IVariableCollectionElementObserver<TKey, TValue>.ObserveElement(TKey id)
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
                .DistinctUntilChanged();
        }

        TValue IVariableCollectionElementReader<TKey, TValue>.ReadElement(TKey id)
        {
            return ObservableDictionary[id];
        }
        
        IEnumerable<TValue> IVariableCollectionReader<TValue>.ReadAllElements()
        {
            return ObservableDictionary.AsEnumerable()
                .Select(pair => pair.Value);
        }

        public void SetElement(TKey id, TValue newElement)
        {
            ObservableDictionary[id] = newElement;
        }

        void IVariableCollectionModifier<TKey, TValue>.Add(TKey id, TValue element)
        {
            ObservableDictionary.Add(id, element);
        }

        void IVariableCollectionModifier<TKey, TValue>.Remove(TKey id)
        {
            ObservableDictionary.Remove(id);
        }

        void IVariableCollectionModifier<TKey, TValue>.Replace(TKey id, TValue newElement)
        {
            SetElement(id, newElement);
        }

        void IVariableCollectionModifier<TKey, TValue>.Reset(IEnumerable<KeyValuePair<TKey, TValue>> newCollection)
        {
            ObservableDictionary.Clear();
            if (newCollection == null) return;
            
            foreach (var pair in newCollection)
            {
                ObservableDictionary.Add(pair);
            }
        }
    }
}