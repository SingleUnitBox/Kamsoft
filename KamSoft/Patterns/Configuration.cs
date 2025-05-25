namespace KamSoft.Patterns;

public interface IConfigurationFor<in T>;

public class StorageConfiguration(string connectionString, string container) : IConfigurationFor<StorageRepository>
{
    public string ConnectionString { get; } = connectionString;
    public string Container { get; } = container;
}

public class CosmosConfiguration(string connectionString, string dbase, string container) : IConfigurationFor<CosmosRepository>
{
    public string ConnectionString { get; } = connectionString;
    public string Dbase { get; } = dbase;
    public string Container { get; } = container;
}