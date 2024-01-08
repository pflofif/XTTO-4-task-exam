using Azure.Data.Tables;
using AzureStorageTableWebApi.Infrastructure.Entites;
using Microsoft.AspNetCore.Mvc;

namespace AzureStorageTableWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private const string TableName = "Product";

    public ProductController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private async Task<TableClient> GetTableClient()
    {
        var serviceClient = new TableServiceClient(_configuration.GetConnectionString("StorageConnectionString"));

        var tableClient = serviceClient.GetTableClient(TableName);
        await tableClient.CreateIfNotExistsAsync();
        return tableClient;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync(string id)
    {
        var tableClient = await GetTableClient();

        var product = await tableClient.GetEntityAsync<Product>("Product", id);
        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] Product product)
    {
        string id = Guid.NewGuid().ToString("N");
        product.Id = id;
        product.RowKey = id;
        product.PartitionKey = "Product";

        await UpsertProduct(product);
        return Ok(product);
    }

    private async Task UpsertProduct(Product product)
    {
        var tableClient = await GetTableClient();
        await tableClient.UpsertEntityAsync(product);
    }

    [HttpPut]
    public async Task<IActionResult> PutAsync([FromBody] Product product)
    {
        product.PartitionKey = "Product";
        product.RowKey = product.Id;

        await UpsertProduct(product);
        return Ok(product);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        var tableClient = await GetTableClient();
        await tableClient.DeleteEntityAsync("Product", id);
        return NoContent();
    }
}