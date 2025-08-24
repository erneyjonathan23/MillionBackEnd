using MediatR;
using Op.Prueba.Application.Interfaces;
using OP.Prueba.Application.Exceptions;
using OP.Prueba.Application.Wrappers;

namespace Op.Prueba.Application.Features.Property.Commands.DeletePropertyCommand
{
    public class DeletePropertyCommandHandler : IRequestHandler<DeletePropertyCommand, Response<bool>>
    {
        private readonly IRepository<Domain.Entities.Property> _repository;

        public DeletePropertyCommandHandler(IRepository<Domain.Entities.Property> repository)
        {
            _repository = repository;
        }

        public async Task<Response<bool>> Handle(DeletePropertyCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (existing is null) throw new ApiException($"No se ha encontrado objeto con el id {request.Id}");

            await _repository.DeleteAsync(request.Id, cancellationToken);
            return new Response<bool>(true, $"Se ha eliminado exitosamente {request.Id}");
        }
    }

}
