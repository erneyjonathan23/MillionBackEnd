using MediatR;
using Op.Prueba.Application.DTOs;
using OP.Prueba.Application.Wrappers;

namespace Op.Prueba.Application.Features.Property.Queries.GetPropertyByIdQuery
{
    public record GetPropertyByIdQuery(string Id) : IRequest<Response<PropertyDto?>>;
}