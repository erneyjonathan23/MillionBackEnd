using MediatR;
using OP.Prueba.Application.Wrappers;

namespace Op.Prueba.Application.Features.Property.Commands.DeletePropertyCommand
{
    public record DeletePropertyCommand(string Id) : IRequest<Response<bool>>;
}
