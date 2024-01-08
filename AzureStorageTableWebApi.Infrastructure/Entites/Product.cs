using Azure;
using Azure.Data.Tables;

namespace AzureStorageTableWebApi.Infrastructure.Entites;

public class Product : ITableEntity
{
    public Product()
    {
        
    }
    
    public Product(string id, string name, string description, int price)
    {
        PartitionKey = "Product";
        RowKey = id;
        Id = id;
        Name = name;
        Description = description;
        Price = price;
    }
    public string  Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }
    
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}