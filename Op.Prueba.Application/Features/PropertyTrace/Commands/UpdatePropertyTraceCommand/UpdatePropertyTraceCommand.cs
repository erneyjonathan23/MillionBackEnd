using MediatR;
using OP.Prueba.Application.Wrappers;

namespace Op.Prueba.Application.Features.PropertyTrace.Commands.UpdatePropertyTraceCommand
{
    public record UpdatePropertyTraceCommand(string Id, string IdProperty, DateTime DateSale, string Name, decimal Value, decimal Tax) : IRequest<Response<bool>>;
}
