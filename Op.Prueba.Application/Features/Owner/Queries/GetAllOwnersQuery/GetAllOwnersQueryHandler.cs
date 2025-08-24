using AutoMapper;
using MediatR;
using Op.Prueba.Application.DTOs;
using Op.Prueba.Application.Interfaces;
using OP.Prueba.Application.Specifications;
using OP.Prueba.Application.Wrappers;

namespace Op.Prueba.Application.Features.Owner.Queries.GetAllOwnersQuery
{
    public class GetAllOwnersQueryHandler : IRequestHandler<GetAllOwnersQuery, PagedResponse<List<OwnerDto>>>
    {
        private readonly IRepository<Domain.Entities.Owner> _repository;
        private IMapper _mapper;

        public GetAllOwnersQueryHandler(IRepository<Domain.Entities.Owner> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<List<OwnerDto>>> Handle(GetAllOwnersQuery request, CancellationToken cancellationToken)
        {
            var spec = new OwnersByFilterSpec(request.Name, request.Address, request.PageNumber, request.PageSize);

            var totalCount = await _repository.CountAsync(new OwnersByFilterSpec(request.Name, request.Address, request.PageNumber, request.PageSize), cancellationToken);
            var response = _mapper.Map<List<OwnerDto>>(await _repository.ListAsync(spec, cancellationToken));

            return new PagedResponse<List<OwnerDto>>(response, request.PageNumber, request.PageSize, totalCount);
        }
    }
}