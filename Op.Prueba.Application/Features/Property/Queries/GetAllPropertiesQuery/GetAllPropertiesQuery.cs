using MediatR;
using Op.Prueba.Application.DTOs;
using OP.Prueba.Application.Wrappers;

namespace Op.Prueba.Application.Features.Property.Queries.GetAllPropertiesQuery
{
    public record GetAllPropertiesQuery(string? Name, string? Address, decimal? MinPrice, decimal? MaxPrice, int PageNumber = 1, int PageSize = 10)
    : IRequest<PagedResponse<List<PropertyDto>>>;
}
