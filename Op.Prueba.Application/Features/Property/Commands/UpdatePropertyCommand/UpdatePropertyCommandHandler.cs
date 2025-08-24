using MediatR;
using Op.Prueba.Application.Interfaces;
using Op.Prueba.Domain.Entities;
using OP.Prueba.Application.Exceptions;
using OP.Prueba.Application.Wrappers;

namespace Op.Prueba.Application.Features.Property.Commands.UpdatePropertyCommand
{
    public class UpdatePropertyCommandHandler : IRequestHandler<UpdatePropertyCommand, Response<bool>>
    {
        private readonly IRepository<Domain.Entities.Property> _repository;

        public UpdatePropertyCommandHandler(IRepository<Domain.Entities.Property> repository)
        {
            _repository = repository;
        }

        public async Task<Response<bool>> Handle(UpdatePropertyCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (existing is null) throw new ApiException($"No se ha encontrado objeto con el id {request.Id}");

            existing.IdOwner = request.IdOwner;
            existing.Name = request.Name;
            existing.Address = request.Address;
            existing.Price = request.Price;
            existing.CodeInternal = request.CodeInternal;
            existing.Year = request.Year;
            existing.Images = request.Images.Select(i => new PropertyImage { File = i.File, Enabled = i.Enabled }).ToList();

            await _repository.UpdateAsync(request.Id, existing, cancellationToken);

            return new Response<bool>(true, "Se ha actualizado exitosamente");
        }
    }

}
