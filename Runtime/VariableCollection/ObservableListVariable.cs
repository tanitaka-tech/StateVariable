using System;
using System.Collections.Generic;
using System.Linq;
using ObservableCollections;
using R3;

namespace TanitakaTech.StateVariable.VariableCollection
{
    public class ObservableListVariable<T, ID> :
        IVariableCollectionObserver<T>,
        IVariableCollectionElementObserver<T, ID>,
        IVariableCollectionElementSetter<T, ID>,
        IVariableCollectionModifier<T, ID>
    {
        private ObservableList<T> ObservableList { get; }
        private Func<T, ID, bool> ElementSelector { get; }

        public ObservableListVariable(ObservableList<T> observableList, Func<T, ID, bool> elementSelector)
        {
            ObservableList = observableList;
            ElementSelector = elementSelector;
        }

        Observable<CollectionAddEvent<T>> IVariableCollectionObserver<T>.ObserveAdd()
        {
            return ObservableList.ObserveAdd();
        }

        Observable<CollectionRemoveEvent<T>> IVariableCollectionObserver<T>.ObserveRemove()
        {
            return ObservableList.ObserveRemove();
        }

        Observable<CollectionReplaceEvent<T>> IVariableCollectionObserver<T>.ObserveReplace()
        {
            return ObservableList.ObserveReplace();
        }

        Observable<CollectionMoveEvent<T>> IVariableCollectionObserver<T>.ObserveMove()
        {
            return ObservableList.ObserveMove();
        }

        Observable<Unit> IVariableCollectionObserver<T>.ObserveReset()
        {
            return ObservableList.ObserveReset();
        }

        Observable<int> IVariableCollectionObserver<T>.ObserveCountChanged()
        {
            return ObservableList.ObserveCountChanged();
        }

        Observable<T> IVariableCollectionElementObserver<T, ID>.ObserveElement(ID id)
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

        T IVariableCollectionElementReader<T, ID>.ReadElement(ID id)
        {
            return ObservableList.FirstOrDefault(e => ElementSelector(e, id));
        }
        
        IEnumerable<T> IVariableCollectionReader<T>.ReadAllElements()
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

        void IVariableCollectionModifier<T, ID>.Add(T element, ID id)
        {
            ObservableList.Add(element);
        }

        void IVariableCollectionModifier<T, ID>.Remove(ID id)
        {
            var alreadyElement = ObservableList.FirstOrDefault(e => ElementSelector(e, id));
            if (alreadyElement != null)
            {
                ObservableList.Remove(alreadyElement);
            }
        }

        void IVariableCollectionModifier<T, ID>.Replace(T newElement, ID id)
        {
            SetElement(id, newElement);
        }

        void IVariableCollectionModifier<T, ID>.Reset(IEnumerable<KeyValuePair<ID, T>> newCollection)
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