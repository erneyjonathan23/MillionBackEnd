using MediatR;
using Op.Prueba.Application.Interfaces;
using OP.Prueba.Application.Wrappers;

namespace Op.Prueba.Application.Features.PropertyTrace.Commands.CreatePropertyTraceCommand
{
    public class CreatePropertyTraceCommandHandler : IRequestHandler<CreatePropertyTraceCommand, Response<string>>
    {
        private readonly IRepository<Domain.Entities.PropertyTrace> _repository;

        public CreatePropertyTraceCommandHandler(IRepository<Domain.Entities.PropertyTrace> repository)
        {
            _repository = repository;
        }

        public async Task<Response<string>> Handle(CreatePropertyTraceCommand request, CancellationToken cancellationToken)
        {
            var trace = new Domain.Entities.PropertyTrace
            {
                IdProperty = request.IdProperty,
                DateSale = request.DateSale,
                Name = request.Name,
                Value = request.Value,
                Tax = request.Tax
            };

            await _repository.AddAsync(trace, cancellationToken);
            return new Response<string>(trace.IdProperty, "Se ha registrado exitosamente");
        }
    }

}