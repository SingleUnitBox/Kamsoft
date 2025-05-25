using KamSoft.Models;

namespace KamSoft.Patterns;

public static class Factory
{
    public static T Create<T>(IConfigurationFor<T> config) where T : class, IRepository
    {
        switch (config)
        {
            case StorageConfiguration storageConfig:
                return new StorageRepository(storageConfig.ConnectionString, storageConfig.Container) as T;
            
            case CosmosConfiguration cosmosConfig:
                return new CosmosRepository(
                    cosmosConfig.ConnectionString, 
                    cosmosConfig.Dbase, 
                    cosmosConfig.Container) as T;
        }

        return null;
    }
}

public class PersonPolicyFactory
{
    public IPersonCreationStrategy GetStrategy(PersonType personType)
    {
        return personType switch
        {
            PersonType.Simple => new StrategySimple(),
            PersonType.WithPesel => new StrategyWithPesel(),
            PersonType.WithAddress => new StrategyWithAddress(),
            _ => null
        };
    }
}

public enum PersonType
{
    Simple,
    WithPesel,
    WithAddress
}