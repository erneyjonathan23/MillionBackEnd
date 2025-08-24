using MediatR;
using Op.Prueba.Application.DTOs;
using OP.Prueba.Application.Wrappers;

namespace Op.Prueba.Application.Features.Property.Commands.CreatePropertyCommand
{
    public record CreatePropertyCommand(string IdOwner, string Name, string Address, decimal Price, string CodeInternal, int Year, List<PropertyImageDto> Images) : IRequest<Response<string>>;
}
