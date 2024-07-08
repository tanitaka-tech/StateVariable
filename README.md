[![openupm](https://img.shields.io/npm/v/com.tanitaka.state-variable?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.tanitaka.state-variable/)
![license](https://img.shields.io/github/license/tanitaka-tech/StateVariable)


## Features 🚀
- Variable Set/Read/Observe
- VariableCollection
- DependencyVariable/HalfDependencyVariable

## Variable List


| class                        | summary                                                            | implements interface                                                                                                                                                                              |
| ---------------------------- | ------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **ObservableVariable**       | A state variable that can be observed                              | IVariableReader, IVariableObserver, IVariableSetter, IDisposable                                                                                                                                  |
| **Variable**                 | A state variable that cannot be observed but is more memory-efficient than ObservableVariable | IVariableReader, IVariableSetter                                                                                                                                                                  |
| **DependencyVariable**       | A variable that derives results from other variables and triggers them | IVariableReader, IVariableObserver                                                                                                                                                                |
| **HalfDependencyVariable**       | A Variable that derive results from other variables and their own variable and trigger them | IVariableReader, IVariableSetter, IVariableObserver                                                                                                                                                                |
| **ObservableListVariable**   | A collection of Variables (internally implemented as a List)       | IVariableReader, IVariableSetter, IVariableCollectionObserver, IVariableCollectionElementObserver, IVariableCollectionElementSetter, IVariableCollectionModifier                                  |
| **ObservableDictionaryVariable** | A collection of Variables (internally implemented as a Dictionary) | IVariableReader, IVariableSetter, IVariableCollectionObserver, IVariableCollectionElementObserver, IVariableCollectionElementSetter, IVariableCollectionModifier                                  |

## Concept

それぞれのVariableはDIコンテナの適切なContextにBindし、各Objectで必要な依存を取得します。

``` csharp

// ----- 任意のDIコンテナのInstaller(このコード例はZenject)
var testVariable = new ObservableVariable<Test>(new Test());
testVariable.AddTo(this);    // ObservableVariableは内部的にReactivePropertyを使用しているため、Dispose管理する必要がある
Container.BindInstance<IVariableReader<Test>>(testVariable);
Container.BindInstance<IVariableObserver<Test>>(testVariable);
Container.BindInstance<IVariableSetter<Test>>(testVariable);

// ----- 任意のObject内
[Inject] IVariableObserver<Test> _testObserver;
[Inject] IVariableReader<Test> _testReader;
[Inject] IVariableSetter<Test> _testSetter;

void Start()
{
    // How to use variable
    _testObserver.Observe().Subscribe(test => {}).AddTo(this);
    Test test = _testReader.Read();
    _testSetter.Set(new Test());

    // ObserverはReadも可能
    test = _testObserver.Read();

    // SetterもReadできる
    test = _testSetter.Read();
}

```

- 機能毎に3つのinterfaceが用意されており、必要なinterfaceのみを注入することでモジュールやオブジェクトの責務を明確にする。
- VariableはStateVariableの略であり、StateVariableには「とあるContext内で共有する状態変数」という意味合いを込めている。そのため、型シグネチャによって共有されている変数ということを強調できる。
- DependencyVariableにより、状態変数から導出される変数を状態変数と同じシグネチャでハンドリングできる

## Installation ☘️

### Install via git URL
1. Open the Package Manager
1. Press [＋▼] button and click Add package from git URL...
1. Enter the following:
    - https://github.com/tanitaka-tech/StateVariable.git

### ~~Install via OpenUPM~~ (not yet)
```sh
openupm add com.tanitaka-tech.state-variable
```

## Required
- [R3](https://github.com/Cysharp/R3)
- [ObaservableCollections](https://github.com/Cysharp/ObservableCollections)
