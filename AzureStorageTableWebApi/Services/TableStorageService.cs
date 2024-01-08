using Azure.Data.Tables;
using AzureStorageTableWebApi.Infrastructure.Entites;
using AzureStorageTableWebApi.Infrastructure.Persistance.Interfaces;

namespace AzureStorageTableWebApi.Services;

public class TableStorageService : ITableStorageService
{
    private const string TableName = "Product";
    private readonly IConfiguration _configuration;

    public TableStorageService(IConfiguration configuration)
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
    
    public async Task<Product> GetEntityAsync(string id)
    {
        var tableClient = await GetTableClient();
        
        var product = await tableClient.GetEntityAsync<Product>("Product", id);
        return product.Value;
    }

    public async Task<Product> UpsertEntityAsync(Product product)
    {
        var tableClient = await GetTableClient();
        await tableClient.UpsertEntityAsync(product);
        return product;
    }

    public async Task DeleteEntityAsync(string id)
    {
        var tableClient = await GetTableClient();
        await tableClient.DeleteEntityAsync("Product", id); 
    }
}