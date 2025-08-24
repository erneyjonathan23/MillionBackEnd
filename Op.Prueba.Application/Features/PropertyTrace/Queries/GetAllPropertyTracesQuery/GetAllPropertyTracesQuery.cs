using MediatR;
using Op.Prueba.Application.DTOs;
using OP.Prueba.Application.Wrappers;

namespace Op.Prueba.Application.Features.PropertyTrace.Queries.GetAllPropertyTracesQuery
{
    public record GetAllPropertyTracesQuery(string? PropertyId, int PageNumber = 1, int PageSize = 10) : IRequest<PagedResponse<List<PropertyTraceDto>>>;
}
