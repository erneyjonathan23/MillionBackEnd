namespace Op.Prueba.Common.Tests1;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using OP.Prueba.Application.Interfaces;
using OP.Prueba.Common;
using OP.Prueba.Common.Services;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class ServiceExtensionsTests
{
    [Test]
    public void AddSharedInfraestructure_ShouldRegister_DateTimeService()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder().Build();

        // Act
        services.AddSharedInfraestructure(configuration);
        var provider = services.BuildServiceProvider();

        // Assert
        var service = provider.GetService<IDateTimeService>();
        Assert.IsNotNull(service);
        Assert.IsInstanceOf<DateTimeService>(service);
    }

    [Test]
    public void AddSharedInfraestructure_ShouldProvideDifferentInstances_WhenTransient()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder().Build();

        // Act
        services.AddSharedInfraestructure(configuration);
        var provider = services.BuildServiceProvider();

        var instance1 = provider.GetService<IDateTimeService>();
        var instance2 = provider.GetService<IDateTimeService>();

        // Assert
        Assert.IsNotNull(instance1);
        Assert.IsNotNull(instance2);
        Assert.AreNotSame(instance1, instance2);
    }
}
