# az-func-learn-csharp

C# で Azure Functions

https://docs.microsoft.com/ja-jp/azure/azure-functions/create-first-function-cli-csharp?tabs=azure-cli%2Cin-process

## 実行モデルが２つある

インプロセス
```
関数コードは、Functions ホスト プロセスと同じプロセスで実行されます。
ホスト プロセスと関数の間の統合が深くなる。
インプロセスで実行中の .NET 関数が Functions ランタイムと同じ .NET のバージョンで実行される必要がある。
```

分離プロセス （.NET 5.0をどうしても利用したい場合はこちらかな。）
```
関数コードは、別の .NET ワーカー プロセスで実行される
Functions ランタイムでネイティブにサポートされていない、現在の .NET リリース (.NET 5.0 など) を使用する関数を開発することができる。
.NET 分離プロジェクトは別個のワーカー プロセスで実行されるため、バインドで ICollector<T>、IAsyncCollector<T>、CloudBlockBlob などの豊富なバインド クラスを利用できません。
```

今回はインプロセスで試す。

## インストールされている.NETのバージョンの確認
```
dotnet --list-sdks
3.1.416 [C:\Program Files\dotnet\sdk]
5.0.401 [C:\Program Files\dotnet\sdk]
6.0.101 [C:\Program Files\dotnet\sdk]
```

## Azureリソースの作成 (functionsをdotnetで作成)
```
# resource group
az group create -n az-func-example-rg -l japaneast
# storage
az storage account create -n funcstorage0001 -g az-func-example-rg -l japaneast --sku Standard_LRS --kind StorageV2
az storage account show-connection-string -g az-func-example-rg -n funcstorage0001
# functions
az functionapp create -g az-func-example-rg --consumption-plan-location japaneast --runtime dotnet --functions-version 4 --name my-example-func --storage-account funcstorage0001
```

## Functionsの作成とデプロイ
基本的にはjavascriptと同様
```
func init LocalFunctionProj --dotnet
func new --name HttpExample --template "HTTP trigger" --authlevel "anonymous"
func azure functionapp publish my-example-func
```

## Durable Functionsの作成とデプロイ
```
func new -t "DurableFunctionsOrchestration" -n  HelloOrchestration
※javascriptとは違い１つのファイル内にOrchestration,Activity,Clientの３つの関数が作成される。
func start 
func azure functionapp publish my-example-func
```

# (注意) 不要になったら削除する
```
az group delete --name az-func-example-rg -y
```

# 他の言語との比較 (Java, Javascript)
Javascriptで必要だったfunction.json設定ファイル必要ない。csファイル内でバインディングの設定を行える。
C#の言語になれている場合は、FunctionsではC#が第一候補かもしれない。
