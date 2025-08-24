using MediatR;
using OP.Prueba.Application.Wrappers;

namespace Op.Prueba.Application.Features.Owner.Commands.CreateOwnerCommand
{
    public record CreateOwnerCommand(string Name, string Address, string Photo, DateTime Birthday) : IRequest<Response<string>>;
}
