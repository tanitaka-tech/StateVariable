![license](https://img.shields.io/github/license/tanitaka-tech/StateVariable)

[nuget](https://www.nuget.org/packages/StateVariable/)

## Mission of this Library
- To provide a simple and easy-to-use state management library for Unity.(Like Redux or Svelte.store)

## Features 🌟
- State Set/Read/Observe
- StateCollection
- DependencyState/HalfDependencyState

## State List

| class                                   | summary                                                                                           | implements interface                                                                                                                                                                              |
|-----------------------------------------|---------------------------------------------------------------------------------------------------| ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **ObservableState**  <br/>**SubjectState** | A state variable that can be observed                                                             | IStateReader, IStateObserver, IStateSetter, IDisposable                                                                                                                                  |
| **State**                            | A state variable that cannot be observed but is more memory-efficient than ObservableState        | IStateReader, IStateSetter                                                                                                                                                                  |
| **DependencyState**                  | A state variable that derives results from other variables and triggers them                      | IStateReader, IStateObserver                                                                                                                                                                |
| **HalfDependencyState**              | A state variable that derive results from other variables and their own variable and trigger them | IStateReader, IStateSetter, IStateObserver                                                                                                                                                                |
| **ObservableListState**              | A collection of StateVariables (internally implemented as a List)                                 | IStateReader, IStateSetter, IStateCollectionObserver, IStateCollectionElementObserver, IStateCollectionElementSetter, IStateCollectionModifier                                  |
| **ObservableDictionaryState**        | A collection of StateVariables (internally implemented as a Dictionary)                           | IStateReader, IStateSetter, IStateCollectionObserver, IStateCollectionElementObserver, IStateCollectionElementSetter, IStateCollectionModifier                                  |

## Usage Example(This Example used a Zenject)

Each State is bound to the appropriate context of the DI container, and each object obtains the necessary dependencies.

``` csharp

// ----- Installer
var testState = new ObservableState<Test>(new Test());
testState.AddTo(this);
Container.BindInstance<IStateReader<Test>>(testState);
Container.BindInstance<IStateObserver<Test>>(testState);
Container.BindInstance<IStateSetter<Test>>(testState);

```

``` csharp
[Inject] IStateObserver<Test> _testObserver;
[Inject] IStateReader<Test> _testReader;
[Inject] IStateSetter<Test> _testSetter;

void Start()
{
    // How to use variable
    _testObserver.Observe().Subscribe(test => {}).AddTo(this);
    Test test = _testReader.Read();
    _testSetter.Set(new Test());

    // Observer can also read
    test = _testObserver.Read();

    // Setter can also read
    test = _testSetter.Read();
}

```

## Installation ☘️

### Install via nuget
1. Run the following:
```
dotnet add package StateVariable --version 1.4.0
```

## Required
- [R3](https://github.com/Cysharp/R3)
- [ObaservableCollections](https://github.com/Cysharp/ObservableCollections)
- [ObaservableCollections.R3](https://github.com/Cysharp/ObservableCollections/tree/master/src/ObservableCollections.R3)
