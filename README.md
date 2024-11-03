![license](https://img.shields.io/github/license/tanitaka-tech/StateVariable)
![nuget](https://www.nuget.org/packages/StateVariable/)

## Mission of this Library
- To provide a simple and easy-to-use state management library for Unity.(Like Redux or Svelte.store)

## Features 🌟
- Variable Set/Read/Observe
- VariableCollection
- DependencyVariable/HalfDependencyVariable

## Variable List

| class                                   | summary                                                            | implements interface                                                                                                                                                                              |
|-----------------------------------------| ------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **ObservableVariable**  <br/>**SubjectVariable** | A state variable that can be observed                              | IVariableReader, IVariableObserver, IVariableSetter, IDisposable                                                                                                                                  |
| **Variable**                            | A state variable that cannot be observed but is more memory-efficient than ObservableVariable | IVariableReader, IVariableSetter                                                                                                                                                                  |
| **DependencyVariable**                  | A variable that derives results from other variables and triggers them | IVariableReader, IVariableObserver                                                                                                                                                                |
| **HalfDependencyVariable**              | A Variable that derive results from other variables and their own variable and trigger them | IVariableReader, IVariableSetter, IVariableObserver                                                                                                                                                                |
| **ObservableListVariable**              | A collection of Variables (internally implemented as a List)       | IVariableReader, IVariableSetter, IVariableCollectionObserver, IVariableCollectionElementObserver, IVariableCollectionElementSetter, IVariableCollectionModifier                                  |
| **ObservableDictionaryVariable**        | A collection of Variables (internally implemented as a Dictionary) | IVariableReader, IVariableSetter, IVariableCollectionObserver, IVariableCollectionElementObserver, IVariableCollectionElementSetter, IVariableCollectionModifier                                  |

## Usage Example(This Example used a Zenject)

Each Variable is bound to the appropriate context of the DI container, and each object obtains the necessary dependencies.

``` csharp

// ----- Installer
var testVariable = new ObservableVariable<Test>(new Test());
testVariable.AddTo(this);
Container.BindInstance<IVariableReader<Test>>(testVariable);
Container.BindInstance<IVariableObserver<Test>>(testVariable);
Container.BindInstance<IVariableSetter<Test>>(testVariable);

```

``` csharp
[Inject] IVariableObserver<Test> _testObserver;
[Inject] IVariableReader<Test> _testReader;
[Inject] IVariableSetter<Test> _testSetter;

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
dotnet add package StateVariable --version 1.2.4
```

## Required
- [R3](https://github.com/Cysharp/R3)
- [ObaservableCollections](https://github.com/Cysharp/ObservableCollections)
- [ObaservableCollections.R3](https://github.com/Cysharp/ObservableCollections/tree/master/src/ObservableCollections.R3)
