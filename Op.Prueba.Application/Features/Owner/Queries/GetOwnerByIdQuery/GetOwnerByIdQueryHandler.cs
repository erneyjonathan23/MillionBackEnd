using AutoMapper;
using MediatR;
using Op.Prueba.Application.DTOs;
using Op.Prueba.Application.Interfaces;
using OP.Prueba.Application.Exceptions;
using OP.Prueba.Application.Wrappers;

namespace Op.Prueba.Application.Features.Owner.Queries.GetOwnerByIdQuery
{
    public class GetOwnerByIdHandler : IRequestHandler<GetOwnerByIdQuery, Response<OwnerDto?>>
    {
        private readonly IRepository<Domain.Entities.Owner> _repository;
        private IMapper _mapper;

        public GetOwnerByIdHandler(IRepository<Domain.Entities.Owner> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<OwnerDto?>> Handle(GetOwnerByIdQuery request, CancellationToken cancellationToken)
        {
            var owner = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (owner is null) throw new ApiException($"No se ha encontrado objeto con el id {request.Id}");

            return new Response<OwnerDto?>(_mapper.Map<OwnerDto>(owner), "Se ha obtenido el objeto exitosamente!");
        }
    }
}