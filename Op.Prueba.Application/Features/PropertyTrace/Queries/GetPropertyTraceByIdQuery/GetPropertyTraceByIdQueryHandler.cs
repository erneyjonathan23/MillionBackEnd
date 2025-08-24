using MediatR;
using Op.Prueba.Application.DTOs;
using Op.Prueba.Application.Interfaces;
using OP.Prueba.Application.Exceptions;
using OP.Prueba.Application.Wrappers;

namespace Op.Prueba.Application.Features.PropertyTrace.Queries.GetPropertyTraceByIdQuery
{
    public class GetPropertyTraceByIdQueryHandler : IRequestHandler<GetPropertyTraceByIdQuery, Response<PropertyTraceDto?>>
    {
        private readonly IRepository<Domain.Entities.PropertyTrace> _repository;

        public GetPropertyTraceByIdQueryHandler(IRepository<Domain.Entities.PropertyTrace> repository)
        {
            _repository = repository;
        }

        public async Task<Response<PropertyTraceDto?>> Handle(GetPropertyTraceByIdQuery request, CancellationToken cancellationToken)
        {
            var trace = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (trace is null) throw new ApiException($"No se ha encontrado objeto con el id {request.Id}");

            var response = new PropertyTraceDto
            {
                Id = trace.IdPropertyTrace,
                IdProperty = trace.IdProperty,
                DateSale = trace.DateSale,
                Name = trace.Name,
                Value = trace.Value,
                Tax = trace.Tax
            };

            return new Response<PropertyTraceDto?>(response, "Se ha obtenido el objeto exitosamente!");
        }
    }

}
