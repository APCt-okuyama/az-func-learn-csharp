# Cosmos DB と接続

```
func init --dotnet
func new --name HttpExample --template "HTTP trigger" --authlevel "anonymous"
func start
func azure functionapp publish my-example-func
```

## Cosmos DB作成
```
az cosmosdb create --name "my-training1-account" --resource-group az-func-example-rg --default-consistency-level Eventual --locations regionName="japaneast" failoverPriority=0 isZoneRedundant=False --capabilities EnableServerless
az cosmosdb list-connection-strings --name my-training1-account --resource-group az-func-example-rg
```

Cosmos DB 拡張機能パッケージをプロジェクトに追加
```
dotnet add package Microsoft.Azure.WebJobs.Extensions.CosmosDB
```

バインディング設定 (HttpExample.cs)
```
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            //バインディングの設定を追加
            [CosmosDB(
                databaseName: "my-database", 
                collectionName: "my-container",
                CreateIfNotExists = true,
                ConnectionStringSetting = "CosmosDbConnectionString")]IAsyncCollector<dynamic> documentsOut,
            ILogger log)
```