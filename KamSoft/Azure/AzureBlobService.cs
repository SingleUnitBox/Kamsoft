using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

public class AzureBlobService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly BlobContainerClient _containerClient;

    public AzureBlobService(string connectionString, string containerName)
    {
        _blobServiceClient = new BlobServiceClient(connectionString);
        _containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        _containerClient.CreateIfNotExists();
    }

    public async Task UploadAsync(string blobName, Stream content)
    {
        var blobClient = _containerClient.GetBlobClient(blobName);
        await blobClient.UploadAsync(content, overwrite: true);
    }

    public async Task<Stream?> DownloadAsync(string blobName)
    {
        var blobClient = _containerClient.GetBlobClient(blobName);

        if (await blobClient.ExistsAsync())
        {
            var download = await blobClient.DownloadAsync();
            return download.Value.Content;
        }

        return null;
    }
}
public class AzureServiceBus
{
    private readonly ServiceBusClient _client;
    private readonly ServiceBusSender _sender;

    public AzureServiceBus(string connectionString, string queueName)
    {
        var adminClient = new ServiceBusAdministrationClient(connectionString);

        // Create the queue if it doesn't exist
        if (!adminClient.QueueExistsAsync(queueName).GetAwaiter().GetResult())
        {
            adminClient.CreateQueueAsync(queueName).GetAwaiter().GetResult();
        }

        _client = new ServiceBusClient(connectionString);
        _sender = _client.CreateSender(queueName);
    }

    public async Task SendMessageAsync(string messageBody)
    {
        var message = new ServiceBusMessage(messageBody);
        
        await _sender.SendMessageAsync(message);
        Console.WriteLine("Message sent!");
    }
}

public class AzureServiceBusConsumer
{
    private readonly ServiceBusClient _client;
    private readonly ServiceBusProcessor _processor;

    public AzureServiceBusConsumer(string connectionString, string queueName)
    {
        _client = new ServiceBusClient(connectionString);

        _processor = _client.CreateProcessor(queueName, new ServiceBusProcessorOptions
        {
            AutoCompleteMessages = false, // You handle completion manually
            MaxConcurrentCalls = 1        // Adjust as needed
        });

        _processor.ProcessMessageAsync += MessageHandler;
        _processor.ProcessErrorAsync += ErrorHandler;
    }

    public async Task StartProcessingAsync()
    {
        await _processor.StartProcessingAsync();
    }

    public async Task StopProcessingAsync()
    {
        await _processor.StopProcessingAsync();
    }

    private async Task MessageHandler(ProcessMessageEventArgs args)
    {
        string body = args.Message.Body.ToString();
        Console.WriteLine($"Received: {body}");

        // Acknowledge the message
        await args.CompleteMessageAsync(args.Message);
    }

    private Task ErrorHandler(ProcessErrorEventArgs args)
    {
        Console.WriteLine($"Error: {args.Exception.Message}");
        return Task.CompletedTask;
    }
}

