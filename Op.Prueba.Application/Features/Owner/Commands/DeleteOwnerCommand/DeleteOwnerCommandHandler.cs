using MediatR;
using Op.Prueba.Application.Interfaces;
using OP.Prueba.Application.Exceptions;
using OP.Prueba.Application.Wrappers;

namespace Op.Prueba.Application.Features.Owner.Commands.DeleteOwnerCommand
{
    public class DeleteOwnerCommandHandler : IRequestHandler<DeleteOwnerCommand, Response<bool>>
    {
        private readonly IRepository<Domain.Entities.Owner> _repository;

        public DeleteOwnerCommandHandler(IRepository<Domain.Entities.Owner> repository)
        {
            _repository = repository;
        }

        public async Task<Response<bool>> Handle(DeleteOwnerCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (existing is null) throw new ApiException($"No se ha encontrado dueño con el id {request.Id}");

            await _repository.DeleteAsync(request.Id, cancellationToken);
            return new Response<bool>(true, $"Se ha eliminado exitosamente {request.Id}");
        }
    }
}