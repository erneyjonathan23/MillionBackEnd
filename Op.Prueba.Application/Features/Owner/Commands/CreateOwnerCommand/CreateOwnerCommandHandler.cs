using MediatR;
using Op.Prueba.Application.Interfaces;
using OP.Prueba.Application.Wrappers;

namespace Op.Prueba.Application.Features.Owner.Commands.CreateOwnerCommand
{
    public class CreateOwnerCommandHandler : IRequestHandler<CreateOwnerCommand, Response<string>>
    {
        private readonly IRepository<Domain.Entities.Owner> _repository;

        public CreateOwnerCommandHandler(IRepository<Domain.Entities.Owner> repository)
        {
            _repository = repository;
        }

        public async Task<Response<string>> Handle(CreateOwnerCommand request, CancellationToken cancellationToken)
        {
            var owner = new Domain.Entities.Owner
            {
                Name = request.Name,
                Address = request.Address,
                Photo = request.Photo,
                Birthday = request.Birthday
            };

            await _repository.AddAsync(owner, cancellationToken);
            return new Response<string>(owner.IdOwner, "Se ha registrado exitosamente");
        }
    }
}