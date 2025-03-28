using System;
using System.Collections.Generic;
using System.Linq;
using ObservableCollections;
using R3;

namespace TanitakaTech.StateVariable.StateCollections
{
    public class ObservableSimpleListState<TValue> :
        IStateCollectionObserver<TValue>,
        IStateCollectionElementObserver<int, TValue>,   // CAUTION: Observing Element can be changed by Add/Remove/Replace
        IStateCollectionModifier<int, TValue>
    {
        private ObservableList<TValue> ObservableList { get; }

        public ObservableSimpleListState(ObservableList<TValue> observableList)
        {
            ObservableList = observableList;
        }

        public ObservableSimpleListState() : this(new ObservableList<TValue>()){}

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

        public Observable<TValue> ObserveElement(int id)
        {
            var replaceObservable = ObservableList.ObserveReplace()
                .Where(e => e.Index == id)
                .Select(e => e.NewValue);
            var resetObservable = ObservableList.ObserveReset()
                .Select(_ => (TValue)default);
            var addObservable = ObservableList.ObserveAdd()
                .Where(e => e.Index == id)
                .Select(e => e.Value);
            var removeObservable = ObservableList.ObserveRemove()
                .Where(e => e.Index == id)
                .Select(_ => (TValue)default);

            return Observable.Merge(replaceObservable, resetObservable, addObservable, removeObservable)
                .Prepend(ReadElement(id))
                .DistinctUntilChanged();
        }

        public TValue ReadElement(int id)
        {
            return ObservableList[id];
        }

        public ElementReadResult TryReadElement(int id, out TValue element1)
        {
            element1 = ReadElement(id);
            return element1 != null ? ElementReadResult.Success : ElementReadResult.NotFound;
        }

        public IEnumerable<TValue> ReadAllElements()
        {
            return ObservableList;
        }

        public void SetElement(int id, TValue newElement)
        {
            bool isExist = ObservableList.Count > id;
            if (isExist)
            {
                var alreadyElement = ObservableList[id];
                var index = ObservableList.IndexOf(alreadyElement);
                ObservableList[index] = newElement;
            }
            else
            {
                ObservableList.Add(newElement);
            }
        }

        public void SetElementWithPrevious(int id, Func<(bool hasPreviousValue, TValue previousElement), TValue> newElementSelector)
        {
            bool isExist = ObservableList.Count > id;
            if (!isExist)
            {
                ObservableList.Add(newElementSelector((false, default)));
                return;
            }
            var alreadyElement = ObservableList[id];
            ObservableList[id] = newElementSelector((true, alreadyElement));
        }

        public void Add(int id, TValue newElement)
        {
            ObservableList.Add(newElement);
        }

        public void Remove(int id)
        {
            bool isExist = ObservableList.Count > id;
            if (isExist)
            {
                ObservableList.RemoveAt(id);
            }
        }

        public void Replace(int id, TValue newElement)
        {
            SetElement(id, newElement);
        }

        public void Reset(IEnumerable<KeyValuePair<int, TValue>> newCollection)
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

        public void Reset()
        {
            ObservableList.Clear();
        }
    }
}