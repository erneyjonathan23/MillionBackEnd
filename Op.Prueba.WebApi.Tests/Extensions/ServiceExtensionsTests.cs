namespace OP.Prueba.WebAPI.Extensions.Tests;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using Moq;
using Serilog;
using System.Linq;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class ServiceExtensionsTests
{
    [Test]
    public void AddApiVersioningExtension_Should_RegisterApiVersioning()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddApiVersioningExtension();
        var provider = services.BuildServiceProvider();

        // Assert
        var apiVersionReader = provider.GetService<Microsoft.AspNetCore.Mvc.Versioning.IApiVersionReader>();
        Assert.That(apiVersionReader, Is.Not.Null);
    }

    [Test]
    public void RegisterLog_Should_AddSerilogLogging()
    {
        // Arrange
        var services = new ServiceCollection();
        var configurationMock = new Mock<IConfiguration>();
        var hostBuilderMock = new Mock<IHostBuilder>();

        // Act
        services.RegisterLog(configurationMock.Object, hostBuilderMock.Object);
        var provider = services.BuildServiceProvider();

        var loggerFactory = provider.GetService<Microsoft.Extensions.Logging.ILoggerFactory>();

        // Assert
        Assert.That(Log.Logger, Is.Not.Null);
        Assert.That(loggerFactory, Is.Not.Null);
    }
}
