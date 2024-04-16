using System.Collections.Generic;
using System.Linq;
using ObservableCollections;
using R3;

namespace TanitakaTech.StateVariable.VariableCollection
{
    public class ObservableDictionaryVariable<ID, T> : 
        IVariableCollectionObserver<T>,
        IVariableCollectionElementObserver<T, ID>,
        IVariableCollectionElementSetter<T, ID>,
        IVariableCollectionModifier<T, ID>
    {
        private ObservableCollections.ObservableDictionary<ID, T> ObservableDictionary { get; }

        public ObservableDictionaryVariable(ObservableCollections.ObservableDictionary<ID, T> observableDictionary)
        {
            ObservableDictionary = observableDictionary;
        }
        
        Observable<CollectionAddEvent<T>> IVariableCollectionObserver<T>.ObserveAdd()
        {
            return ObservableDictionary.ObserveAdd()
                .Select(e => new CollectionAddEvent<T>(e.Index, e.Value.Value));
        }

        Observable<CollectionRemoveEvent<T>> IVariableCollectionObserver<T>.ObserveRemove()
        {
            return ObservableDictionary.ObserveRemove()
                .Select(e => new CollectionRemoveEvent<T>(e.Index, e.Value.Value));
        }

        Observable<CollectionReplaceEvent<T>> IVariableCollectionObserver<T>.ObserveReplace()
        {
            return ObservableDictionary.ObserveReplace()
                .Select(e => new CollectionReplaceEvent<T>(e.Index, e.OldValue.Value, e.NewValue.Value));
        }

        Observable<CollectionMoveEvent<T>> IVariableCollectionObserver<T>.ObserveMove()
        {
            return ObservableDictionary.ObserveMove()
                .Select(e => new CollectionMoveEvent<T>(e.OldIndex, e.NewIndex, e.Value.Value));
        }

        Observable<Unit> IVariableCollectionObserver<T>.ObserveReset()
        {
            return ObservableDictionary.ObserveReset();
        }

        Observable<int> IVariableCollectionObserver<T>.ObserveCountChanged()
        {
            return ObservableDictionary.ObserveCountChanged();
        }

        Observable<T> IVariableCollectionElementObserver<T, ID>.ObserveElement(ID id)
        {
            var replaceObservable = ObservableDictionary.ObserveReplace()
                .Where(e => e.NewValue.Key.Equals(id))
                .Select(e => e.NewValue.Value);
            var resetObservable = ObservableDictionary.ObserveReset()
                .Select(_ => (T)default);
            var addObservable = ObservableDictionary.ObserveAdd()
                .Where(e => e.Value.Key.Equals(id))
                .Select(e => e.Value.Value);
            var removeObservable = ObservableDictionary.ObserveRemove()
                .Where(e => e.Value.Key.Equals(id))
                .Select(_ => (T)default);
            
            return Observable.Merge(replaceObservable, resetObservable, addObservable, removeObservable)
                .DistinctUntilChanged();
        }

        T IVariableCollectionElementReader<T, ID>.ReadElement(ID id)
        {
            return ObservableDictionary[id];
        }
        
        IEnumerable<T> IVariableCollectionReader<T>.ReadAllElements()
        {
            return ObservableDictionary.AsEnumerable()
                .Select(pair => pair.Value);
        }

        public void SetElement(ID id, T newElement)
        {
            ObservableDictionary[id] = newElement;
        }

        void IVariableCollectionModifier<T, ID>.Add(T element, ID id)
        {
            ObservableDictionary.Add(id, element);
        }

        void IVariableCollectionModifier<T, ID>.Remove(ID id)
        {
            ObservableDictionary.Remove(id);
        }

        void IVariableCollectionModifier<T, ID>.Replace(T newElement, ID id)
        {
            SetElement(id, newElement);
        }

        void IVariableCollectionModifier<T, ID>.Reset(IEnumerable<KeyValuePair<ID, T>> newCollection)
        {
            ObservableDictionary.Clear();
            if (newCollection == null) return;
            
            foreach (var pair in newCollection)
            {
                ObservableDictionary.Add(pair.Key, pair.Value);
            }
        }
    }
}