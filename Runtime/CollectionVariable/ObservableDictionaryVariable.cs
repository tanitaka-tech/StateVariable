using System.Collections.Generic;
using System.Linq;
using ObservableCollections;
using R3;

namespace TanitakaTech.StateVariable.CollectionVariable
{
    public class ObservableDictionaryVariable<ID, T> : 
        ICollectionVariableObserver<T>,
        ICollectionVariableElementObserver<T, ID>,
        ICollectionVariableElementSetter<T, ID>,
        ICollectionVariableModifier<T, ID>
    {
        private ObservableDictionary<ID, T> ObservableDictionary { get; }

        public ObservableDictionaryVariable(ObservableDictionary<ID, T> observableDictionary)
        {
            ObservableDictionary = observableDictionary;
        }
        
        Observable<CollectionAddEvent<T>> ICollectionVariableObserver<T>.ObserveAdd()
        {
            return ObservableDictionary.ObserveAdd()
                .Select(e => new CollectionAddEvent<T>(e.Index, e.Value.Value));
        }

        Observable<CollectionRemoveEvent<T>> ICollectionVariableObserver<T>.ObserveRemove()
        {
            return ObservableDictionary.ObserveRemove()
                .Select(e => new CollectionRemoveEvent<T>(e.Index, e.Value.Value));
        }

        Observable<CollectionReplaceEvent<T>> ICollectionVariableObserver<T>.ObserveReplace()
        {
            return ObservableDictionary.ObserveReplace()
                .Select(e => new CollectionReplaceEvent<T>(e.Index, e.OldValue.Value, e.NewValue.Value));
        }

        Observable<CollectionMoveEvent<T>> ICollectionVariableObserver<T>.ObserveMove()
        {
            return ObservableDictionary.ObserveMove()
                .Select(e => new CollectionMoveEvent<T>(e.OldIndex, e.NewIndex, e.Value.Value));
        }

        Observable<Unit> ICollectionVariableObserver<T>.ObserveReset()
        {
            return ObservableDictionary.ObserveReset();
        }

        Observable<int> ICollectionVariableObserver<T>.ObserveCountChanged()
        {
            return ObservableDictionary.ObserveCountChanged();
        }

        Observable<T> ICollectionVariableElementObserver<T, ID>.ObserveElement(ID id)
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

        T ICollectionVariableElementReader<T, ID>.ReadElement(ID id)
        {
            return ObservableDictionary[id];
        }
        
        IEnumerable<T> ICollectionVariableElementReader<T, ID>.ReadAllElements()
        {
            return ObservableDictionary.AsEnumerable()
                .Select(pair => pair.Value);
        }

        public void SetElement(ID id, T newElement)
        {
            ObservableDictionary[id] = newElement;
        }

        void ICollectionVariableModifier<T, ID>.Add(T element, ID id)
        {
            ObservableDictionary.Add(id, element);
        }

        void ICollectionVariableModifier<T, ID>.Remove(ID id)
        {
            ObservableDictionary.Remove(id);
        }

        void ICollectionVariableModifier<T, ID>.Replace(T newElement, ID id)
        {
            SetElement(id, newElement);
        }

        void ICollectionVariableModifier<T, ID>.Reset(IEnumerable<KeyValuePair<ID, T>> newCollection)
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