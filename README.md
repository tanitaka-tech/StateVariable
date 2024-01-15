[![openupm](https://img.shields.io/npm/v/com.tanitaka.state-variable?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.tanitaka.state-variable/)
![license](https://img.shields.io/github/license/tanitaka-tech/StateVariable)


## Features 🚀
- RequestPusher/RequestConsumer
- Variable Set/Read/Subscribe

## 使用想定

### IVariableSetter/Reader/Observer
Context内で同じ変数を共有したい時に使用する。(ex. 画面内で選択中のアイテムの参照など)
提供する機能はReactivePropertyと同じだが、こちらは役割を絞ることでより詳細なコード表現を目的にする。
- 機能毎に3つのinterfaceが用意されており、必要なinterfaceのみを注入することでモジュールの責務を限定する。
- VariableとはStateVariableの略であり、StateVariableには「とあるContext内で共有する変数」という意味合いを込めている。そのため、共有されている変数ということを強調できる。

### IRequestPusher/Consumer
Context内で「子から親へのメッセージング」したい時に使用する。(ex. ボタンモジュールから画面遷移RequestをPushし、画面モジュールでRequestをConsumeするなど)
提供する機能はMessagePipeと似ているが、こちらはよりシンプルな使用を想定している。
- Messageという命名はモジュール間で相互にやりとりする印象があるが、Requestはモジュール間で一方向な印象があり、ユースケースを限定した命名になっている。
- Requestに対してFilterをかける機能などは無く、本当にただPushしてConsumeでRequestをawaitするだけ。

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

## Required 🙏
- [UniTask](https://github.com/Cysharp/UniTask)
- [UniRx](https://github.com/neuecc/UniRx)
