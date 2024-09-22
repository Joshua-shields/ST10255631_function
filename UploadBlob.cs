using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;

public static class UploadBlob
{
    [Function("UploadBlob")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
        ILogger log)
    {
        string containerName = req.Query["containerName"];
        string blobName = req.Query["blobName"];

        using var stream = req.Body;
        var connectionString = Environment.GetEnvironmentVariable("AzureStorage:ConnectionString");
        var blobServiceClient = new BlobServiceClient(connectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync();
        var blobClient = containerClient.GetBlobClient(blobName);
        await blobClient.UploadAsync(stream, true);

        return new OkObjectResult("Blob uploaded");
    }
}
//////////////////////////////////////////////////////END OF FILE//////////////////////////////////////////////////////