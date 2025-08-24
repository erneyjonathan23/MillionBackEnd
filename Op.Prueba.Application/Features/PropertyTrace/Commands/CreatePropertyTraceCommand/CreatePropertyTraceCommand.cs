using MediatR;
using OP.Prueba.Application.Wrappers;

namespace Op.Prueba.Application.Features.PropertyTrace.Commands.CreatePropertyTraceCommand
{
    public record CreatePropertyTraceCommand(string IdProperty, DateTime DateSale, string Name, decimal Value, decimal Tax) : IRequest<Response<string>>;

}
