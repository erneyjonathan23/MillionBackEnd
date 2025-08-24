using MediatR;
using Op.Prueba.Application.Interfaces;
using OP.Prueba.Application.Exceptions;
using OP.Prueba.Application.Wrappers;

namespace Op.Prueba.Application.Features.PropertyTrace.Commands.UpdatePropertyTraceCommand
{
    public class UpdatePropertyTraceCommandHandler : IRequestHandler<UpdatePropertyTraceCommand, Response<bool>>
    {
        private readonly IRepository<Domain.Entities.PropertyTrace> _repository;

        public UpdatePropertyTraceCommandHandler(IRepository<Domain.Entities.PropertyTrace> repository)
        {
            _repository = repository;
        }

        public async Task<Response<bool>> Handle(UpdatePropertyTraceCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (existing is null) throw new ApiException($"No se ha encontrado objeto con el id {request.Id}");

            existing.IdProperty = request.IdProperty;
            existing.DateSale = request.DateSale;
            existing.Name = request.Name;
            existing.Value = request.Value;
            existing.Tax = request.Tax;

            await _repository.UpdateAsync(request.Id, existing, cancellationToken);

            return new Response<bool>(true, "Se ha actualizado exitosamente");
        }
    }
}
