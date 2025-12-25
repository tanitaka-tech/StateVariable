using R3;

namespace TanitakaTech.StateVariable
{
    /// <summary>
    /// ReactivePropertyをIStateインターフェースに変換するためのクラス
    /// 主にScriptableObjectに紐づいたSerializableReactivePropertyなど、Disposeされると困る状態に使う
    /// </summary>
    public class RPMediatorState<T> :
        IStateSetter<T>,
        IStateObserver<T>,
        IStateReader<T>
    {
        private ReactiveProperty<T> ReactiveProperty { get; }

        // 注: ReactivePropertyのDisposeはDI側で管理
        public RPMediatorState(ReactiveProperty<T> reactiveProperty)
        {
            ReactiveProperty = reactiveProperty;
        }

        public void Set(T value)
        {
            ReactiveProperty.Value = value;
        }

        public T Read() => ReactiveProperty.Value;

        public Observable<T> Observe() => ReactiveProperty;
    }
}