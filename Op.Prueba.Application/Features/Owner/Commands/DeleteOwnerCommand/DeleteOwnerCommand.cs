using MediatR;
using OP.Prueba.Application.Wrappers;

namespace Op.Prueba.Application.Features.Owner.Commands.DeleteOwnerCommand
{
    public record DeleteOwnerCommand(string Id) : IRequest<Response<bool>>;
}