using MediatR;
using Op.Prueba.Application.DTOs;
using Op.Prueba.Application.Interfaces;
using Op.Prueba.Application.Specifications;
using OP.Prueba.Application.Wrappers;

namespace Op.Prueba.Application.Features.PropertyTrace.Queries.GetAllPropertyTracesQuery
{
    public class GetAllPropertyTracesQueryHandler : IRequestHandler<GetAllPropertyTracesQuery, PagedResponse<List<PropertyTraceDto>>>
    {
        private readonly IRepository<Domain.Entities.PropertyTrace> _repository;

        public GetAllPropertyTracesQueryHandler(IRepository<Domain.Entities.PropertyTrace> repository)
        {
            _repository = repository;
        }

        public async Task<PagedResponse<List<PropertyTraceDto>>> Handle(GetAllPropertyTracesQuery request, CancellationToken cancellationToken)
        {
            var spec = new PropertyTracesByPropertySpec(request.PropertyId, request.PageNumber, request.PageSize);
            var totalCount = await _repository.CountAsync(new PropertyTracesByPropertySpec(request.PropertyId, request.PageNumber, request.PageSize));
            var traces = await _repository.ListAsync(spec, cancellationToken);

            var response = traces.Select(t => new PropertyTraceDto
            {
                Id = t.IdPropertyTrace,
                IdProperty = t.IdProperty,
                DateSale = t.DateSale,
                Name = t.Name,
                Value = t.Value,
                Tax = t.Tax
            }).ToList();

            return new PagedResponse<List<PropertyTraceDto>>(response, request.PageNumber, request.PageSize, totalCount);
        }
    }

}
