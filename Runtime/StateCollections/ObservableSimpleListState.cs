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
        private readonly ObservableList<TValue> _observableList;

        public ObservableSimpleListState(ObservableList<TValue> observableList)
        {
            _observableList = observableList;
        }

        public ObservableSimpleListState() : this(new ObservableList<TValue>()){}

        public Observable<CollectionAddEvent<TValue>> ObserveAdd()
        {
            return _observableList.ObserveAdd();
        }

        public Observable<CollectionRemoveEvent<TValue>> ObserveRemove()
        {
            return _observableList.ObserveRemove();
        }

        public Observable<CollectionReplaceEvent<TValue>> ObserveReplace()
        {
            return _observableList.ObserveReplace();
        }

        public Observable<CollectionMoveEvent<TValue>> ObserveMove()
        {
            return _observableList.ObserveMove();
        }

        public Observable<Unit> ObserveReset()
        {
            return _observableList.ObserveReset().AsUnitObservable();
        }

        public Observable<int> ObserveCountChanged()
        {
            return _observableList.ObserveCountChanged();
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
                    , _observableList.ObserveChanged().AsUnitObservable()
                )
                .Prepend(Unit.Default)
                .ThrottleLastFrame(1);
        }

        public Observable<TValue> ObserveElement(int id)
        {
            var replaceObservable = _observableList.ObserveReplace()
                .Where(e => e.Index == id)
                .Select(e => e.NewValue);
            var resetObservable = _observableList.ObserveReset()
                .Select(_ => (TValue)default);
            var addObservable = _observableList.ObserveAdd()
                .Where(e => e.Index == id)
                .Select(e => e.Value);
            var removeObservable = _observableList.ObserveRemove()
                .Where(e => e.Index == id)
                .Select(_ => (TValue)default);

            return Observable.Merge(replaceObservable, resetObservable, addObservable, removeObservable)
                .Prepend(ReadElement(id))
                .DistinctUntilChanged();
        }

        public TValue ReadElement(int id)
        {
            return _observableList[id];
        }

        public ElementReadResult TryReadElement(int id, out TValue element1)
        {
            element1 = ReadElement(id);
            return element1 != null ? ElementReadResult.Success : ElementReadResult.NotFound;
        }

        public IEnumerable<TValue> ReadAllElements()
        {
            return _observableList;
        }

        public void SetElement(int id, TValue newElement)
        {
            bool isExist = _observableList.Count > id;
            if (isExist)
            {
                var alreadyElement = _observableList[id];
                var index = _observableList.IndexOf(alreadyElement);
                _observableList[index] = newElement;
            }
            else
            {
                _observableList.Add(newElement);
            }
        }

        public void SetElementWithPrevious(int id, Func<(bool hasPreviousValue, TValue previousElement), TValue> newElementSelector)
        {
            bool isExist = _observableList.Count > id;
            if (!isExist)
            {
                _observableList.Add(newElementSelector((false, default)));
                return;
            }
            var alreadyElement = _observableList[id];
            _observableList[id] = newElementSelector((true, alreadyElement));
        }

        public void Add(int id, TValue newElement)
        {
            _observableList.Add(newElement);
        }

        public void Remove(int id)
        {
            bool isExist = _observableList.Count > id;
            if (isExist)
            {
                _observableList.RemoveAt(id);
            }
        }

        public void Replace(int id, TValue newElement)
        {
            SetElement(id, newElement);
        }

        public void Reset(IEnumerable<KeyValuePair<int, TValue>> newCollection)
        {
            _observableList.Clear();
            if (newCollection == null)
            {
                return;
            }

            // NOTE: AddRangeを使うと要素がnullになることがあるため、Addで追加
            var addRange = newCollection.Select(pair => pair.Value).ToList();
            foreach (var value in addRange)
            {
                _observableList.Add(value);
            }
        }

        public void Reset()
        {
            _observableList.Clear();
        }
    }
}