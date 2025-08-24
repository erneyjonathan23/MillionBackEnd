using MediatR;
using Op.Prueba.Application.DTOs;
using OP.Prueba.Application.Wrappers;

namespace Op.Prueba.Application.Features.PropertyTrace.Queries.GetPropertyTraceByIdQuery
{
    public record GetPropertyTraceByIdQuery(string Id) : IRequest<Response<PropertyTraceDto?>>;
}