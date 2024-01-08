using AzureStorageTableWebApi.Infrastructure.Entites;
using AzureStorageTableWebApi.Infrastructure.Persistance.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AzureStorageTableWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly ITableStorageService _storageService;

    public ProductController(ITableStorageService storageService)
    {
        _storageService = storageService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync(string id)
    {
        return Ok(await _storageService.GetEntityAsync(id));
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] Product product)
    {
        string id = Guid.NewGuid().ToString("N");
        product.Id = id;
        product.RowKey = id;
        product.PartitionKey = "Product";
        var createdEntity = await _storageService.UpsertEntityAsync(product);
        return Ok(createdEntity);
    }

    [HttpPut]
    public async Task<IActionResult> PutAsync([FromBody] Product entity)
    {
        entity.PartitionKey = "Product";
        entity.RowKey = entity.Id;
        var updateEntity = await _storageService.UpsertEntityAsync(entity);
        return Ok(updateEntity);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        await _storageService.DeleteEntityAsync(id);
        return NoContent();
    }
}