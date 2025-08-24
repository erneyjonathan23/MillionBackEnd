using MediatR;
using OP.Prueba.Application.Wrappers;

namespace Op.Prueba.Application.Features.PropertyTrace.Commands.DeletePropertyTraceCommand
{
    public record DeletePropertyTraceCommand(string Id) : IRequest<Response<bool>>;
}
