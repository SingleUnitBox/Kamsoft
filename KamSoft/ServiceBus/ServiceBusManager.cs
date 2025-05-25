using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Azure;

namespace KamSoft.ServiceBus;

public class ServiceBusManager : IHostedService
{
    private readonly ServiceBusClient _serviceBusClient;
    private readonly ServiceBusProcessor _serviceBusProcessor;
    
    public ServiceBusManager(IAzureClientFactory<ServiceBusClient> clientFactory)
    {
        _serviceBusClient = clientFactory.CreateClient("serviceBusClient");
        _serviceBusProcessor = _serviceBusClient.CreateProcessor("cezary_zukowski_queue", new ServiceBusProcessorOptions
        {
            MaxConcurrentCalls = 10,
        });
        _serviceBusProcessor.ProcessMessageAsync += ProcessMessage;
        _serviceBusProcessor.ProcessErrorAsync += ProcessError;
    }

    private async Task ProcessMessage(ProcessMessageEventArgs args)
    {
        string body = args.Message.Body.ToString();
        Console.WriteLine($"Received message: {body}");
    }

    private async Task ProcessError(ProcessErrorEventArgs args)
    {
        
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return _serviceBusProcessor.StartProcessingAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return _serviceBusProcessor.StopProcessingAsync(cancellationToken);
    }
}