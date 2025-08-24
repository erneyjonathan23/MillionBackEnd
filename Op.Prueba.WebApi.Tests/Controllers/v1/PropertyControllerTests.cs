namespace Op.Prueba.WebApi.Tests.Controllers.v1;

using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OP.Prueba.WebAPI.Controllers;
using Op.Prueba.Application.DTOs;
using Op.Prueba.Application.Features.Property.Commands.DeletePropertyCommand;
using Op.Prueba.Application.Features.Property.Commands.UpdatePropertyCommand;
using Op.Prueba.Application.Features.Property.Queries.GetAllPropertiesQuery;
using Op.Prueba.Application.Features.Property.Queries.GetPropertyByIdQuery;
using OP.Prueba.Application.Wrappers;

[TestFixture]
[Parallelizable(ParallelScope.None)]
public class PropertyControllerTests
{
    private Mock<IMediator> _mediatorMock = null!;
    private PropertyController _controller = null!;

    [SetUp]
    public void Setup()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new PropertyController();

        typeof(BaseApiController)
            .GetField("_mediator", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(_controller, _mediatorMock.Object);
    }

    [Test]
    public async Task Get_ShouldReturnPagedResponse_WhenMediatorReturnsPaged()
    {
        // Arrange
        var query = new GetAllPropertiesQuery(null, null, 1, 10);

        var properties = new List<PropertyDto>
        {
            new PropertyDto { Id = "1", IdOwner = "o1", Name = "Prop1", Address = "Addr1", Price = 100m, CodeInternal = "C1", Year = 2020 },
            new PropertyDto { Id = "2", IdOwner = "o2", Name = "Prop2", Address = "Addr2", Price = 200m, CodeInternal = "C2", Year = 2021 }
        };

        var expectedPagedResponse = new PagedResponse<List<PropertyDto>>(properties, 1, 10, properties.Count);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllPropertiesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedPagedResponse);

        // Act
        var result = await _controller.Get(query) as ObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo((int)expectedPagedResponse.HttpCode));
        Assert.That(result.Value, Is.EqualTo(expectedPagedResponse));

        _mediatorMock.Verify(
            m => m.Send(It.Is<GetAllPropertiesQuery>(q => q.PageNumber == 1 && q.PageSize == 10), It.IsAny<CancellationToken>()),
            Times.Once);

        _mediatorMock.VerifyNoOtherCalls();
    }

    [Test]
    public async Task GetById_ShouldReturnPropertyResponse_WhenFound()
    {
        // Arrange
        var id = "123";
        var dto = new PropertyDto { Id = id, IdOwner = "o1", Name = "Prop 123", Address = "Some", Price = 123m, CodeInternal = "CI", Year = 2022 };

        var expectedResponse = new Response<PropertyDto>(dto) { HttpCode = HttpStatusCode.OK };

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetPropertyByIdQuery>(q => q.Id == id), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetById(id) as ObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo((int)expectedResponse.HttpCode));
        Assert.That(result.Value, Is.EqualTo(expectedResponse));

        _mediatorMock.Verify(
            m => m.Send(It.Is<GetPropertyByIdQuery>(q => q.Id == id), It.IsAny<CancellationToken>()),
            Times.Once);

        _mediatorMock.VerifyNoOtherCalls();
    }

    [Test]
    public async Task Put_WithMismatchedId_ShouldReturnBadRequest_AndNotCallMediator()
    {
        // Arrange
        var images = new List<PropertyImageDto>();
        var command = new UpdatePropertyCommand("456", "owner-1", "Name", "Addr", 100m, "C", 2020, images);

        // Act
        var result = await _controller.Put("123", command);

        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestResult>());

        _mediatorMock.Verify(m => m.Send(It.IsAny<IRequest<object>>(), It.IsAny<CancellationToken>()), Times.Never);
        _mediatorMock.VerifyNoOtherCalls();
    }

    [Test]
    public async Task Put_WithValidId_ShouldReturnResponseFromMediator()
    {
        // Arrange
        var id = "123";
        var images = new List<PropertyImageDto>();
        var command = new UpdatePropertyCommand(id, "owner-1", "Name", "Addr", 100m, "C", 2020, images);

        var expectedResponse = new Response<bool>(true) { HttpCode = HttpStatusCode.NoContent };

        _mediatorMock
            .Setup(m => m.Send(It.Is<UpdatePropertyCommand>(c => c.Id == id), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Put(id, command) as ObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo((int)expectedResponse.HttpCode));
        Assert.That(result.Value, Is.EqualTo(expectedResponse));

        _mediatorMock.Verify(
            m => m.Send(It.Is<UpdatePropertyCommand>(c => c.Id == id), It.IsAny<CancellationToken>()),
            Times.Once);

        _mediatorMock.VerifyNoOtherCalls();
    }

    [Test]
    public async Task Delete_ShouldReturnResponseFromMediator()
    {
        // Arrange
        var id = "789";

        var expectedResponse = new Response<bool>(true) { HttpCode = HttpStatusCode.NoContent };

        _mediatorMock
            .Setup(m => m.Send(It.Is<DeletePropertyCommand>(c => c.Id == id), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Delete(id) as ObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo((int)expectedResponse.HttpCode));
        Assert.That(result.Value, Is.EqualTo(expectedResponse));

        _mediatorMock.Verify(
            m => m.Send(It.Is<DeletePropertyCommand>(c => c.Id == id), It.IsAny<CancellationToken>()),
            Times.Once);

        _mediatorMock.VerifyNoOtherCalls();
    }
}
