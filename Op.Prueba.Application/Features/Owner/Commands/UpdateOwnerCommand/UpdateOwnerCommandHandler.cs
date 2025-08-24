using MediatR;
using Op.Prueba.Application.Interfaces;
using OP.Prueba.Application.Exceptions;
using OP.Prueba.Application.Wrappers;

namespace Op.Prueba.Application.Features.Owner.Commands.UpdateOwnerCommand
{
    public class UpdateOwnerCommandHandler : IRequestHandler<UpdateOwnerCommand, Response<bool>>
    {
        private readonly IRepository<Domain.Entities.Owner> _repository;

        public UpdateOwnerCommandHandler(IRepository<Domain.Entities.Owner> repository)
        {
            _repository = repository;
        }

        public async Task<Response<bool>> Handle(UpdateOwnerCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (existing is null) throw new ApiException($"No se ha encontrado dueño con el id {request.Id}");

            existing.Name = request.Name;
            existing.Address = request.Address;
            existing.Photo = request.Photo;
            existing.Birthday = request.Birthday;

            await _repository.UpdateAsync(request.Id, existing, cancellationToken);

            return new Response<bool>(true, "Se ha actualizado exitosamente");
        }
    }
}
