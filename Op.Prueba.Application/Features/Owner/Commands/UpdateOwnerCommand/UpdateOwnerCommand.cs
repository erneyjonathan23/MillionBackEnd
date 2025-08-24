using MediatR;
using OP.Prueba.Application.Wrappers;

namespace Op.Prueba.Application.Features.Owner.Commands.UpdateOwnerCommand
{
    public record UpdateOwnerCommand(string Id, string Name, string Address, string Photo, DateTime Birthday) : IRequest<Response<bool>>;
}
