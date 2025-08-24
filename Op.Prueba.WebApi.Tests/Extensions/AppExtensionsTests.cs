namespace OP.Prueba.WebAPI.Tests.Extensions;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using OP.Prueba.WebAPI.Extensions;
using System;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class AppExtensionsTests
{
    [Test]
    public void UseErrorHandleMiddleware_Should_CallUseMethod()
    {
        // Arrange
        var mockAppBuilder = new Mock<IApplicationBuilder>();
        mockAppBuilder
            .Setup(x => x.Use(It.IsAny<Func<RequestDelegate, RequestDelegate>>()))
            .Returns(mockAppBuilder.Object);

        // Act
        mockAppBuilder.Object.UseErrorHandleMiddleware();

        // Assert
        mockAppBuilder.Verify(
            x => x.Use(It.IsAny<Func<RequestDelegate, RequestDelegate>>()),
            Times.Once);
    }
}
