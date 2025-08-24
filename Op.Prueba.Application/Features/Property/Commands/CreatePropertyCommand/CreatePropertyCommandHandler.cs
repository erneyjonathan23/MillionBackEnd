using MediatR;
using Op.Prueba.Application.Interfaces;
using Op.Prueba.Domain.Entities;
using OP.Prueba.Application.Wrappers;

namespace Op.Prueba.Application.Features.Property.Commands.CreatePropertyCommand
{
    public class CreatePropertyCommandHandler : IRequestHandler<CreatePropertyCommand, Response<string>>
    {
        private readonly IRepository<Domain.Entities.Property> _repository;

        public CreatePropertyCommandHandler(IRepository<Domain.Entities.Property> repository)
        {
            _repository = repository;
        }

        public async Task<Response<string>> Handle(CreatePropertyCommand request, CancellationToken cancellationToken)
        {
            var property = new Domain.Entities.Property
            {
                IdOwner = request.IdOwner,
                Name = request.Name,
                Address = request.Address,
                Price = request.Price,
                CodeInternal = request.CodeInternal,
                Year = request.Year,
                Images = request.Images.Select(i => new PropertyImage { File = i.File, Enabled = i.Enabled }).ToList()
            };

            await _repository.AddAsync(property, cancellationToken);
            return new Response<string>(property.IdProperty, "Se ha registrado exitosamente");
        }
    }
}
