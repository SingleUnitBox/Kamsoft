using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs;
using KamSoft.Models;
using KamSoft.Patterns;
using KamSoft.ServiceBus;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddSingleton(new StorageConfiguration("con", "con"));

builder.Services.AddKeyedSingleton<IPersonCreationStrategy>("simple", new StrategySimple());
builder.Services.AddKeyedSingleton<IPersonCreationStrategy>("pesel", new StrategyWithPesel());
builder.Services.AddKeyedSingleton<IPersonCreationStrategy>("address", new StrategyWithAddress());
builder.Services.AddSingleton(sp =>
    new AzureBlobService(
        builder.Configuration["Azure:ConnectionString"],
        builder.Configuration["Azure:ContainerName"]
    ));
builder.Services.AddSingleton(sp =>
    new AzurePersonStorage(
        builder.Configuration["Azure:ConnectionString"],
        builder.Configuration["Azure:ContainerName"]
    ));
builder.Services.AddSingleton<AzureServiceBus>(sp =>
    new AzureServiceBus(
        builder.Configuration["Azure:ConnectionStringBus"],
        builder.Configuration["Azure:QueueName"]
    ));
builder.Services.AddSingleton<AzureServiceBusConsumer>(sp =>
    new AzureServiceBusConsumer(
        builder.Configuration["Azure:ConnectionStringBus"],
        builder.Configuration["Azure:QueueName"]
    ));
builder.Services.AddAzureClients(builder =>
{
    builder.AddServiceBusClient("Endpoint=sb://warsztaty00.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=wnKaVmDSEXvg//7J1yxOQoGfsjN8H8Eaw+ASbP6nIa0=")
        .WithName("serviceBusClient");
});

builder.Services.AddSingleton<PersonPolicyFactory>();
builder.Services.AddHostedService<ServiceBusManager>();

// builder.Services.AddSingleton<IEventDispatcher>(sp => new EventDispatcher(sp));
// builder.Services.AddScoped<IEventHandler<KamSoft.Handlers.LocalDTO.PersonAdded>, PersonAddedHandler>();
// builder.Services.AddScoped<ICommandHandler<AddPerson>>(sp => new AddPersonHandler(sp.GetService<IModuleClient>()));
// builder.Services.AddModuleRegistry();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseHttpsRedirection();
app.MapControllers();
// rfc, add endpoints more, headers, poprawic dzialanie storage w endpoicie
// 2, rozbudowac przyklady klasy person
// 3. wykorzystac startegie w ramach wzoraca biuilder
// app.MapGet("/{id}", async (Guid id) =>
// {
//     var config = app.Services.GetRequiredService<StorageConfiguration>();
//     var storage = Factory.Create(config);
//
//     // todo
//     return await storage.Get<Person>(id);
// });
// app.MapGet("person", () =>
// {
//     var factory = app.Services.GetRequiredService<PersonPolicyFactory>();
//     var policy = factory.GetStrategy(PersonType.WithAddress);
//     var builder = new PersonBuilder(policy);
//     var person = builder
//         .AddId(Guid.NewGuid())
//         .AddName("Janek")
//         .Build();
//
//     return person;
// });
app.MapGet("trigger", () =>
{
    var game = new Game();
    var player = new Player();
    //player.Subscribe(game);
    //player.SubscribeToGame(game);
    
    var spectator = new Spectator();
    spectator.Subscribe(game);
    
    game.AddPoints(100);
    game.CheckHomeTeam(false);
});
app.MapPost("azure", async () =>
{
    var connString =
        "DefaultEndpointsProtocol=https;AccountName=warsztaty00;AccountKey=jRwx9YCcMIV9RC+6G058uZvumVtQCLwBR7gHrCLoUBr3QBvaIH7KNFfMVY7hASBQchRNoZpYod1u+AStLp1Mrw==;EndpointSuffix=core.windows.net";
    
    var containerName = "cezary-zukowski-container";
    var blobName = "cezary_zukowski_blob.txt";
    var localFile = "cezary_zukowski_file.txt";

    var blobService = new BlobServiceClient(connString);
    var containerClient = blobService.GetBlobContainerClient(containerName);

    // Create container if it doesn't exist
    await containerClient.CreateIfNotExistsAsync();

    var blobClient = containerClient.GetBlobClient(blobName);

    // Upload only if blob doesn't already exist
    var blobExists = await blobClient.ExistsAsync();
    if (!blobExists)
    {
        using var stream = File.OpenRead(localFile);
        await blobClient.UploadAsync(stream);
        Console.WriteLine("Blob uploaded successfully.");
    }
    else
    {
        Console.WriteLine("Blob already exists, skipping upload.");
    }
});
app.MapGet("/azure/{filename}", async (string filename, AzureBlobService blobService) =>
{
    var stream = await blobService.DownloadAsync(filename);
    if (stream == null)
        return Results.NotFound();

    return Results.File(stream, "application/octet-stream", filename);
});

app.MapGet("person/{personId}", async (AzurePersonStorage storage, Guid personId) =>
{
    await storage.Get<Person>(personId);
});
app.MapPost("person", async (AzurePersonStorage storage) =>
{
    // use mapper
    var person = new Person.Simple(Guid.NewGuid(), "Simple", "Simplowski");
    await storage.Save(person);
});
app.MapPost("bus", async (AzureServiceBus serviceBus) =>
{
    await serviceBus.SendMessageAsync("Hello world from Kamsoft Workshops. - Cezary Zukowski");
});
// app.MapGet("bus", async () =>
// {
//     var client = new ServiceBusClient(app.Configuration["Azure:ConnectionStringBus"]);
//     var receiver = client.CreateReceiver(app.Configuration["Azure:QueueName"]);
//
//     var message = await receiver.ReceiveMessageAsync();
//     if (message != null)
//     {
//         Console.WriteLine($"Received: {message.Body}");
//         await receiver.CompleteMessageAsync(message);
//     }
// });
app.MapGet("/bus", async (IConfiguration config) =>
{
    var connectionString = config["Azure:ConnectionStringBus"];
    var queueName = config["Azure:QueueName"];

    var client = new ServiceBusClient(connectionString);
    var receiver = client.CreateReceiver(queueName);

    var message = await receiver.ReceiveMessageAsync();
    if (message != null)
    {
        Console.WriteLine($"Received: {message.Body}");
        await receiver.CompleteMessageAsync(message);
        return Results.Ok(message.Body.ToString());
    }

    return Results.NotFound("No message in queue.");
});

// app.MapGet("event", async ([FromServices] ICommandHandler<AddPerson> commandHandler) =>
// {
//     await commandHandler.HandleAsync(new AddPerson("Event", "Eventowski"));
// });


app.Run();
