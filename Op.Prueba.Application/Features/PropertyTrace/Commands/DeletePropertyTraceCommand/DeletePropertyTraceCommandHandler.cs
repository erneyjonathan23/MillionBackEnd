using MediatR;
using Op.Prueba.Application.Interfaces;
using OP.Prueba.Application.Exceptions;
using OP.Prueba.Application.Wrappers;

namespace Op.Prueba.Application.Features.PropertyTrace.Commands.DeletePropertyTraceCommand
{
    public class DeletePropertyTraceCommandHandler : IRequestHandler<DeletePropertyTraceCommand, Response<bool>>
    {
        private readonly IRepository<Domain.Entities.PropertyTrace> _repository;

        public DeletePropertyTraceCommandHandler(IRepository<Domain.Entities.PropertyTrace> repository)
        {
            _repository = repository;
        }

        public async Task<Response<bool>> Handle(DeletePropertyTraceCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (existing is null) throw new ApiException($"No se ha encontrado objeto con el id {request.Id}");

            await _repository.DeleteAsync(request.Id, cancellationToken);
            return new Response<bool>(true, $"Se ha eliminado exitosamente {request.Id}");
        }
    }

}
