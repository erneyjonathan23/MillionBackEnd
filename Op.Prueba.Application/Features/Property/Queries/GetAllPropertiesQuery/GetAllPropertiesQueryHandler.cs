using MediatR;
using Op.Prueba.Application.DTOs;
using Op.Prueba.Application.Interfaces;
using Op.Prueba.Application.Specifications;
using OP.Prueba.Application.Wrappers;

namespace Op.Prueba.Application.Features.Property.Queries.GetAllPropertiesQuery
{
    public class GetAllPropertiesQueryHandler : IRequestHandler<GetAllPropertiesQuery, PagedResponse<List<PropertyDto>>>
    {
        private readonly IRepository<Domain.Entities.Property> _repository;

        public GetAllPropertiesQueryHandler(IRepository<Domain.Entities.Property> repository)
        {
            _repository = repository;
        }
        public async Task<PagedResponse<List<PropertyDto>>> Handle(GetAllPropertiesQuery request, CancellationToken cancellationToken)
        {
            var spec = new PropertiesByFilterSpec(request.Name, request.Address, request.MinPrice, request.MaxPrice, request.PageNumber, request.PageSize);
            var totalCount = await _repository.CountAsync(new PropertiesByFilterSpec(request.Name, request.Address, request.MinPrice, request.MaxPrice), cancellationToken);
            var properties = await _repository.ListAsync(spec, cancellationToken);

            var response = properties.Select(p => new PropertyDto
            {
                Id = p.IdProperty,
                IdOwner = p.IdOwner,
                Name = p.Name,
                Address = p.Address,
                Price = p.Price,
                CodeInternal = p.CodeInternal,
                Year = p.Year,
                Images = p.Images.Select(i => new PropertyImageDto { File = i.File, Enabled = i.Enabled }).ToList()
            }).ToList();

            return new PagedResponse<List<PropertyDto>>(response, request.PageNumber, request.PageSize, totalCount);
        }
    }
}