using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace training1_cosmosdb
{
    public static class HttpExample
    {
        [FunctionName("HttpExample")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            //バインディングの設定を追加
            [CosmosDB(
                databaseName: "my-database", 
                collectionName: "my-container",
                CreateIfNotExists = true,                
                ConnectionStringSetting = "CosmosDbConnectionString")] IAsyncCollector<dynamic> documentsOut,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");


            // Add a JSON document to the output container.
            await documentsOut.AddAsync(new
            {
                // create a random ID
                id = System.Guid.NewGuid().ToString(),
                name = "Cosmos DBと接続します。" 
            });

            return new OkObjectResult("Cosmos DBと接続します。");
        }
    }
}
