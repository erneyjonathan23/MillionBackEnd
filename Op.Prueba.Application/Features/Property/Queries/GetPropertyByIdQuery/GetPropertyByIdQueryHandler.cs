using MediatR;
using Op.Prueba.Application.DTOs;
using Op.Prueba.Application.Interfaces;
using OP.Prueba.Application.Exceptions;
using OP.Prueba.Application.Wrappers;

namespace Op.Prueba.Application.Features.Property.Queries.GetPropertyByIdQuery
{
    public class GetPropertyByIdHandler : IRequestHandler<GetPropertyByIdQuery, Response<PropertyDto?>>
    {
        private readonly IRepository<Domain.Entities.Property> _repository;

        public GetPropertyByIdHandler(IRepository<Domain.Entities.Property> repository)
        {
            _repository = repository;
        }

        public async Task<Response<PropertyDto?>> Handle(GetPropertyByIdQuery request, CancellationToken cancellationToken)
        {
            var property = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (property is null) throw new ApiException($"No se ha encontrado objeto con el id {request.Id}");

            var response = new PropertyDto
            {
                Id = property.IdProperty,
                IdOwner = property.IdOwner,
                Name = property.Name,
                Address = property.Address,
                Price = property.Price,
                CodeInternal = property.CodeInternal,
                Year = property.Year,
                Images = property.Images.Select(i => new PropertyImageDto { File = i.File, Enabled = i.Enabled }).ToList()
            };

            return new Response<PropertyDto?>(response, "Se ha obtenido el objeto exitosamente!");
        }
    }

}
