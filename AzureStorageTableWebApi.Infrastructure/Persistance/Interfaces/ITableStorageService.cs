using AzureStorageTableWebApi.Infrastructure.Entites;

namespace AzureStorageTableWebApi.Infrastructure.Persistance.Interfaces;

public interface ITableStorageService
{
    Task<Product> GetEntityAsync(string id);
    Task<Product> UpsertEntityAsync(Product product);
    Task DeleteEntityAsync(string id);
}