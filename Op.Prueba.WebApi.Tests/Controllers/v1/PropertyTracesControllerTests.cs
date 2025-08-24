namespace Op.Prueba.WebApi.Tests.Controllers.v1;

using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OP.Prueba.Application.Wrappers;
using OP.Prueba.WebAPI.Controllers;
using Op.Prueba.Application.DTOs;
using Op.Prueba.Application.Features.PropertyTrace.Commands.DeletePropertyTraceCommand;
using Op.Prueba.Application.Features.PropertyTrace.Commands.UpdatePropertyTraceCommand;
using Op.Prueba.Application.Features.PropertyTrace.Queries.GetAllPropertyTracesQuery;
using Op.Prueba.Application.Features.PropertyTrace.Queries.GetPropertyTraceByIdQuery;

[TestFixture]
[Parallelizable(ParallelScope.None)]
public class PropertyTracesControllerTests
{
    private Mock<IMediator> _mediatorMock = null!;
    private PropertyTracesController _controller = null!;

    [SetUp]
    public void Setup()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new PropertyTracesController();

        typeof(BaseApiController)
            .GetField("_mediator", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(_controller, _mediatorMock.Object);
    }

    [Test]
    public async Task Get_ShouldReturnPagedResponse_WhenMediatorReturnsPaged()
    {
        // Arrange
        var query = new GetAllPropertyTracesQuery(null, 1, 10);

        var traces = new List<PropertyTraceDto>
        {
            new PropertyTraceDto { Id = "1", IdProperty = "P1", Name = "Trace1", Value = 100, Tax = 10, DateSale = DateTime.UtcNow },
            new PropertyTraceDto { Id = "2", IdProperty = "P2", Name = "Trace2", Value = 200, Tax = 20, DateSale = DateTime.UtcNow }
        };

        var expectedPagedResponse = new PagedResponse<List<PropertyTraceDto>>(traces, 1, 10, traces.Count);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllPropertyTracesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedPagedResponse);

        // Act
        var result = await _controller.Get(query) as ObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo((int)expectedPagedResponse.HttpCode));
        Assert.That(result.Value, Is.EqualTo(expectedPagedResponse));
    }

    [Test]
    public async Task GetById_ShouldReturnTraceResponse_WhenFound()
    {
        // Arrange
        var id = "123";
        var dto = new PropertyTraceDto { Id = id, IdProperty = "P123", Name = "Trace123", Value = 300, Tax = 30, DateSale = DateTime.UtcNow };

        var expectedResponse = new Response<PropertyTraceDto>(dto);

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetPropertyTraceByIdQuery>(q => q.Id == id), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetById(id) as ObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo((int)expectedResponse.HttpCode));
        Assert.That(result.Value, Is.EqualTo(expectedResponse));
    }

    [Test]
    public async Task Put_WithMismatchedId_ShouldReturnBadRequest()
    {
        // Arrange
        var command = new UpdatePropertyTraceCommand("456", "PropertyX", DateTime.UtcNow, "TraceName", 600m, 60m);

        // Act
        var result = await _controller.Put("123", command);

        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestResult>());

        _mediatorMock.Verify(m => m.Send(It.IsAny<IRequest<object>>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    public async Task Put_WithValidId_ShouldReturnResponseFromMediator()
    {
        // Arrange
        var id = "123";
        var command = new UpdatePropertyTraceCommand(id, "PropertyX", DateTime.UtcNow, "TraceName", 600m, 60m);

        var expectedResponse = new Response<bool>(true) { HttpCode = HttpStatusCode.Accepted };

        _mediatorMock
            .Setup(m => m.Send(It.Is<UpdatePropertyTraceCommand>(c => c.Id == id), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Put(id, command) as ObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo((int)expectedResponse.HttpCode));
        Assert.That(result.Value, Is.EqualTo(expectedResponse));
    }

    [Test]
    public async Task Delete_ShouldReturnResponseFromMediator()
    {
        // Arrange
        var id = "789";

        var expectedResponse = new Response<bool>(true) { HttpCode = HttpStatusCode.Accepted };

        _mediatorMock
            .Setup(m => m.Send(It.Is<DeletePropertyTraceCommand>(c => c.Id == id), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Delete(id) as ObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo((int)expectedResponse.HttpCode));
        Assert.That(result.Value, Is.EqualTo(expectedResponse));
    }
}
