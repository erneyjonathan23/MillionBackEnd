using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Op.Prueba.Application.Interfaces;
using Op.Prueba.Persistence.Repository;

public static class ServiceExtensions
{
    public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Obtener configuración de Mongo
        var connectionString = configuration.GetSection("MongoDbSettings:ConnectionString").Value;
        var databaseName = configuration.GetSection("MongoDbSettings:DatabaseName").Value;

        // Cliente de Mongo
        var mongoClient = new MongoClient(connectionString);
        var database = mongoClient.GetDatabase(databaseName);

        // Registrar IMongoDatabase como Singleton
        services.AddSingleton(database);

        // Registrar el repositorio genérico
        services.AddScoped(typeof(IRepository<>), typeof(MongoRepository<>));
    }
}