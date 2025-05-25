using System.Text;
using System.Text.Json;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace KamSoft.Patterns;

public interface IRepositoryObject
{
    public Guid Id { get; }
}

public interface IRepository
{
    Task<T> Get<T>(Guid id) where T : class, IRepositoryObject;
    Task Save<T>(T input) where T : class, IRepositoryObject;
}

public interface IRepository<T> where T : class, IRepositoryObject
{
    Task<T> Get();
    Task Save(T input);
}

// uncommon
public class PersonRepository
{
    
}

// common
public class StorageRepository(string connectionString, string container) : IRepository
{
    public Task<T> Get<T>(Guid id) where T : class, IRepositoryObject
    {
        throw new NotImplementedException();
    }

    public Task Save<T>(T input) where T : class, IRepositoryObject
    {
        throw new NotImplementedException();
    }
}

public class CosmosRepository(string connectionString, string dbase, string container) : IRepository
{
    public Task<T> Get<T>(Guid id) where T : class, IRepositoryObject
    {
        throw new NotImplementedException();
    }

    public Task Save<T>(T input) where T : class, IRepositoryObject
    {
        throw new NotImplementedException();
    }
}

public class AzurePersonStorage : IRepository
{
    private readonly string _container;
    private readonly BlobServiceClient _client;

    public AzurePersonStorage(string connectionString, string container)
    {
        _container = container;
        _client = new BlobServiceClient(connectionString);
    }
    
    public async Task<T> Get<T>(Guid id) where T : class, IRepositoryObject
    {
        var container = _client.GetBlobContainerClient(_container);
        var blobClient = container.GetBlobClient(_container);
        BlobDownloadResult result = await blobClient.DownloadContentAsync();

        string content = result.Content.ToString();
        return JsonSerializer.Deserialize<T>(content);

        // if (await blobClient.ExistsAsync())
        // {
        //     var download = await blobClient.DownloadAsync();
        //     return download.Value.Content;
        // }
        //
        // return null;
    }
    
    // dodac typ classy do serializera
    public async Task Save<T>(T input) where T : class, IRepositoryObject
    {
        var cont = _client.GetBlobContainerClient(_container);
        var dict = JsonSerializer.SerializeToElement(input).EnumerateObject()
            .ToDictionary(p => p.Name, p => p.Value);

        dict["Type"] = JsonSerializer.SerializeToElement(input.GetType().Name);

        var merged = new Dictionary<string, JsonElement>(dict);
        var data = JsonSerializer.Serialize(dict, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(data));
        await cont.UploadBlobAsync($"{input.Id}.json", stream);
    }
}