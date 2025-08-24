using MediatR;
using Op.Prueba.Application.DTOs;
using OP.Prueba.Application.Wrappers;

namespace Op.Prueba.Application.Features.Owner.Queries.GetOwnerByIdQuery
{
    public record GetOwnerByIdQuery(string Id) : IRequest<Response<OwnerDto?>>;
}