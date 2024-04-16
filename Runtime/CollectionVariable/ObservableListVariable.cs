using System;
using System.Collections.Generic;
using System.Linq;
using ObservableCollections;
using R3;

namespace TanitakaTech.StateVariable.CollectionVariable
{
    public class ObservableListVariable<T, ID> :
        ICollectionVariableObserver<T>,
        ICollectionVariableElementObserver<T, ID>,
        ICollectionVariableElementSetter<T, ID>,
        ICollectionVariableModifier<T, ID>
    {
        private ObservableList<T> ObservableList { get; }
        private Func<T, ID, bool> ElementSelector { get; }

        public ObservableListVariable(ObservableList<T> observableList, Func<T, ID, bool> elementSelector)
        {
            ObservableList = observableList;
            ElementSelector = elementSelector;
        }

        Observable<CollectionAddEvent<T>> ICollectionVariableObserver<T>.ObserveAdd()
        {
            return ObservableList.ObserveAdd();
        }

        Observable<CollectionRemoveEvent<T>> ICollectionVariableObserver<T>.ObserveRemove()
        {
            return ObservableList.ObserveRemove();
        }

        Observable<CollectionReplaceEvent<T>> ICollectionVariableObserver<T>.ObserveReplace()
        {
            return ObservableList.ObserveReplace();
        }

        Observable<CollectionMoveEvent<T>> ICollectionVariableObserver<T>.ObserveMove()
        {
            return ObservableList.ObserveMove();
        }

        Observable<Unit> ICollectionVariableObserver<T>.ObserveReset()
        {
            return ObservableList.ObserveReset();
        }

        Observable<int> ICollectionVariableObserver<T>.ObserveCountChanged()
        {
            return ObservableList.ObserveCountChanged();
        }

        Observable<T> ICollectionVariableElementObserver<T, ID>.ObserveElement(ID id)
        {
            var replaceObservable = ObservableList.ObserveReplace()
                .Where(e => ElementSelector(e.NewValue, id))
                .Select(e => e.NewValue);
            var resetObservable = ObservableList.ObserveReset()
                .Select(_ => (T)default);
            var addObservable = ObservableList.ObserveAdd()
                .Where(e => ElementSelector(e.Value, id))
                .Select(e => e.Value);
            var removeObservable = ObservableList.ObserveRemove()
                .Where(e => ElementSelector(e.Value, id))
                .Select(_ => (T)default);

            return Observable.Merge(replaceObservable, resetObservable, addObservable, removeObservable)
                .DistinctUntilChanged();
        }

        T ICollectionVariableElementReader<T, ID>.ReadElement(ID id)
        {
            return ObservableList.FirstOrDefault(e => ElementSelector(e, id));
        }
        
        IEnumerable<T> ICollectionVariableReader<T>.ReadAllElements()
        {
            return ObservableList;
        }

        public void SetElement(ID id, T newElement)
        {
            var alreadyElement = ObservableList.FirstOrDefault(e => ElementSelector(e, id));
            bool isExist = alreadyElement != null;
            if (isExist)
            {
                var index = ObservableList.IndexOf(alreadyElement);
                ObservableList[index] = newElement;
            }
            else
            {
                ObservableList.Add(newElement);
            }
        }

        void ICollectionVariableModifier<T, ID>.Add(T element, ID id)
        {
            ObservableList.Add(element);
        }

        void ICollectionVariableModifier<T, ID>.Remove(ID id)
        {
            var alreadyElement = ObservableList.FirstOrDefault(e => ElementSelector(e, id));
            if (alreadyElement != null)
            {
                ObservableList.Remove(alreadyElement);
            }
        }

        void ICollectionVariableModifier<T, ID>.Replace(T newElement, ID id)
        {
            SetElement(id, newElement);
        }

        void ICollectionVariableModifier<T, ID>.Reset(IEnumerable<KeyValuePair<ID, T>> newCollection)
        {
            ObservableList.Clear();
            if (newCollection == null)
            {
                return;
            }
            
            foreach (var pair in newCollection)
            {
                ObservableList.Add(pair.Value);
            }
        }
    }
}