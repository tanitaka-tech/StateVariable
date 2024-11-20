using System;
using System.Collections.Generic;
using System.Linq;
using ObservableCollections;
using R3;

namespace TanitakaTech.StateVariable.VariableCollection
{
    public class ObservableListState<TKey, TValue> :
        IStateCollectionObserver<TValue>,
        IStateCollectionElementObserver<TKey, TValue>,
        IStateCollectionElementSetter<TKey, TValue>,
        IStateCollectionModifier<TKey, TValue>
    {
        private ObservableList<TValue> ObservableList { get; }
        private Func<TKey, TValue, bool> ElementSelector { get; }

        public ObservableListState(Func<TKey, TValue, bool> elementSelector): this(new ObservableList<TValue>(), elementSelector)
        {
        }
        
        public ObservableListState(ObservableList<TValue> observableList, Func<TKey, TValue, bool> elementSelector)
        {
            ObservableList = observableList;
            ElementSelector = elementSelector;
        }

        public Observable<CollectionAddEvent<TValue>> ObserveAdd()
        {
            return ObservableList.ObserveAdd();
        }

        public Observable<CollectionRemoveEvent<TValue>> ObserveRemove()
        {
            return ObservableList.ObserveRemove();
        }

        public Observable<CollectionReplaceEvent<TValue>> ObserveReplace()
        {
            return ObservableList.ObserveReplace();
        }

        public Observable<CollectionMoveEvent<TValue>> ObserveMove()
        {
            return ObservableList.ObserveMove();
        }

        public Observable<Unit> ObserveReset()
        {
            return ObservableList.ObserveReset().AsUnitObservable();
        }

        public Observable<int> ObserveCountChanged()
        {
            return ObservableList.ObserveCountChanged();
        }

        public Observable<TValue> ObserveElement(TKey id)
        {
            var replaceObservable = ObservableList.ObserveReplace()
                .Where(e => ElementSelector(id, e.NewValue))
                .Select(e => e.NewValue);
            var resetObservable = ObservableList.ObserveReset()
                .Select(_ => (TValue)default);
            var addObservable = ObservableList.ObserveAdd()
                .Where(e => ElementSelector(id, e.Value))
                .Select(e => e.Value);
            var removeObservable = ObservableList.ObserveRemove()
                .Where(e => ElementSelector(id, e.Value))
                .Select(_ => (TValue)default);

            return Observable.Merge(replaceObservable, resetObservable, addObservable, removeObservable)
                .Prepend(ReadElement(id))
                .DistinctUntilChanged();
        }

        public TValue ReadElement(TKey id)
        {
            return ObservableList.FirstOrDefault(e => ElementSelector(id, e));
        }

        public ElementReadResult TryReadElement(TKey id, out TValue element1)
        {
            element1 = ReadElement(id);
            return element1 != null ? ElementReadResult.Success : ElementReadResult.NotFound;
        }

        
        public IEnumerable<TValue> ReadAllElements()
        {
            return ObservableList;
        }

        public void SetElement(TKey id, TValue newElement)
        {
            var alreadyElement = ObservableList.FirstOrDefault(e => ElementSelector(id, e));
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

        public void SetElementWithPrevious(TKey id, Func<(bool hasPreviousValue, TValue previousElement), TValue> newElementSelector)
        {
            var alreadyElement = ObservableList.FirstOrDefault(e => ElementSelector(id, e));
            bool isExist = alreadyElement != null;
            var previous = isExist ? alreadyElement : default;
            var newElement = newElementSelector((isExist, previous));
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

        public void Add(TKey id, TValue newElement)
        {
            ObservableList.Add(newElement);
        }

        public void Remove(TKey id)
        {
            var alreadyElement = ObservableList.FirstOrDefault(e => ElementSelector(id, e));
            if (alreadyElement != null)
            {
                ObservableList.Remove(alreadyElement);
            }
        }

        public void Replace(TKey id, TValue newElement)
        {
            SetElement(id, newElement);
        }

        public void Reset(IEnumerable<KeyValuePair<TKey, TValue>> newCollection)
        {
            ObservableList.Clear();
            if (newCollection == null)
            {
                return;
            }
            
            // NOTE: AddRangeを使うと要素がnullになることがあるため、Addで追加
            var addRange = newCollection.Select(pair => pair.Value).ToList();
            foreach (var value in addRange)
            {
                ObservableList.Add(value);
            }
        }
    }
}