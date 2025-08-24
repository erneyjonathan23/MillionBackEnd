namespace Op.Prueba.WebApi.Tests.Controllers.v1;

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OP.Prueba.WebAPI.Controllers;
using Op.Prueba.Application.DTOs;
using Op.Prueba.Application.Features.Owner.Commands.DeleteOwnerCommand;
using Op.Prueba.Application.Features.Owner.Commands.UpdateOwnerCommand;
using Op.Prueba.Application.Features.Owner.Queries.GetAllOwnersQuery;
using Op.Prueba.Application.Features.Owner.Queries.GetOwnerByIdQuery;
using OP.Prueba.Application.Wrappers;
using System.Net;
using System;
using System.Collections.Generic;

[TestFixture]
[Parallelizable(ParallelScope.None)]
public class OwnerControllerTests
{
    private Mock<IMediator> _mediatorMock = null!;
    private OwnerController _controller = null!;

    [SetUp]
    public void Setup()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new OwnerController();

        typeof(BaseApiController)
            .GetField("_mediator", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(_controller, _mediatorMock.Object);
    }

    [Test]
    public async Task Get_ShouldReturnPagedResponse_WhenMediatorReturnsPaged()
    {
        // Arrange
        var query = new GetAllOwnersQuery(null, null, PageNumber: 1, PageSize: 10);

        var owners = new List<OwnerDto>
        {
            new OwnerDto { IdOwner = "1", Name = "Owner1", Address = "Addr1", Photo = "p1", Birthday = DateTime.UtcNow },
            new OwnerDto { IdOwner = "2", Name = "Owner2", Address = "Addr2", Photo = "p2", Birthday = DateTime.UtcNow }
        };

        var expectedPagedResponse = new PagedResponse<List<OwnerDto>>(owners, 1, 10, owners.Count);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllOwnersQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedPagedResponse);

        // Act
        var result = await _controller.Get(query) as ObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo((int)expectedPagedResponse.HttpCode));
        Assert.That(result.Value, Is.EqualTo(expectedPagedResponse));

        _mediatorMock.Verify(
            m => m.Send(It.Is<GetAllOwnersQuery>(q => q.PageNumber == 1 && q.PageSize == 10), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task GetById_ShouldReturnOwnerResponse_WhenFound()
    {
        // Arrange
        var id = "123";
        var ownerDto = new OwnerDto { IdOwner = id, Name = "Owner 123", Address = "Some", Photo = "p", Birthday = DateTime.UtcNow };

        var expectedResponse = new Response<OwnerDto>(ownerDto);

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetOwnerByIdQuery>(q => q.Id == id), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetById(id) as ObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo((int)expectedResponse.HttpCode));
        Assert.That(result.Value, Is.EqualTo(expectedResponse));

        _mediatorMock.Verify(
            m => m.Send(It.Is<GetOwnerByIdQuery>(q => q.Id == id), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task Put_WithMismatchedId_ShouldReturnBadRequest()
    {
        // Arrange
        var command = new UpdateOwnerCommand("456", "Name", "Addr", "Photo", DateTime.UtcNow);

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
        var command = new UpdateOwnerCommand(id, "Name", "Addr", "Photo", DateTime.UtcNow);

        var expectedResponse = new Response<bool>(true) { HttpCode = HttpStatusCode.Accepted };

        _mediatorMock
            .Setup(m => m.Send(It.Is<UpdateOwnerCommand>(c => c.Id == id), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Put(id, command) as ObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo((int)expectedResponse.HttpCode));
        Assert.That(result.Value, Is.EqualTo(expectedResponse));

        _mediatorMock.Verify(
            m => m.Send(It.Is<UpdateOwnerCommand>(c => c.Id == id), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task Delete_ShouldReturnResponseFromMediator()
    {
        // Arrange
        var id = "789";

        var expectedResponse = new Response<bool>(true) { HttpCode = HttpStatusCode.Accepted };

        _mediatorMock
            .Setup(m => m.Send(It.Is<DeleteOwnerCommand>(c => c.Id == id), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Delete(id) as ObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo((int)expectedResponse.HttpCode));
        Assert.That(result.Value, Is.EqualTo(expectedResponse));

        _mediatorMock.Verify(
            m => m.Send(It.Is<DeleteOwnerCommand>(c => c.Id == id), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
