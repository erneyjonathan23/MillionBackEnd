namespace Op.Prueba.Persistence.Tests;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using NUnit.Framework;
using Op.Prueba.Application.Interfaces;
using Op.Prueba.Persistence.Repository;
using System.Collections.Generic;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class ServiceExtensionsTests
{
    [Test]
    public void AddPersistenceInfrastructure_ShouldRegister_IMongoDatabase()
    {
        // Arrange
        var provider = BuildServiceProvider();

        // Act
        var database = provider.GetService<IMongoDatabase>();

        // Assert
        Assert.IsNotNull(database);
        Assert.AreEqual("TestDb", database.DatabaseNamespace.DatabaseName);
    }

    [Test]
    public void AddPersistenceInfrastructure_ShouldRegister_IRepository_AsMongoRepository()
    {
        // Arrange
        var provider = BuildServiceProvider();

        // Act
        var repo = provider.GetService<IRepository<string>>();

        // Assert
        Assert.IsNotNull(repo);
        Assert.IsInstanceOf<MongoRepository<string>>(repo);
    }

    [Test]
    public void IMongoDatabase_ShouldBeSingleton()
    {
        // Arrange
        var provider = BuildServiceProvider();

        // Act
        var db1 = provider.GetService<IMongoDatabase>();
        var db2 = provider.GetService<IMongoDatabase>();

        // Assert
        Assert.AreSame(db1, db2);
    }

    [Test]
    public void IRepository_ShouldBeScoped()
    {
        // Arrange
        var services = BuildServiceProvider();

        // Act
        using var scope1 = services.CreateScope();
        using var scope2 = services.CreateScope();

        var repo1 = scope1.ServiceProvider.GetService<IRepository<string>>();
        var repo2 = scope2.ServiceProvider.GetService<IRepository<string>>();

        // Assert
        Assert.AreNotSame(repo1, repo2);
    }
    
    private ServiceProvider BuildServiceProvider()
    {
        // Arrange  
        var inMemorySettings = new Dictionary<string, string>
        {
            {"MongoDbSettings:ConnectionString", "mongodb://localhost:27017"},
            {"MongoDbSettings:DatabaseName", "TestDb"}
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        var services = new ServiceCollection();

        // Act
        services.AddPersistenceInfrastructure(configuration);
        return services.BuildServiceProvider();
    }
}
