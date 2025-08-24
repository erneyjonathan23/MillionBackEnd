using MediatR;
using Op.Prueba.Application.DTOs;
using OP.Prueba.Application.Wrappers;

namespace Op.Prueba.Application.Features.Owner.Queries.GetAllOwnersQuery
{
    public record GetAllOwnersQuery(
        string? Name,
        string? Address,
        int PageNumber = 1,
        int PageSize = 10
        ) : IRequest<PagedResponse<List<OwnerDto>>>;
}