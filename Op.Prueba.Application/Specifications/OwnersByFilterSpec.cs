using Ardalis.Specification;
using Op.Prueba.Domain.Entities;

namespace OP.Prueba.Application.Specifications
{
    public class OwnersByFilterSpec : Specification<Owner>
    {
        public OwnersByFilterSpec(string? name, string? address, int? pageNumber = null, int? pageSize = null)
        {
            if (!string.IsNullOrEmpty(name))
                Query.Where(o => o.Name.Contains(name));

            if (!string.IsNullOrEmpty(address))
                Query.Where(o => o.Address.Contains(address));

            if (pageSize != null && pageNumber != null)
                Query.Skip(((int)pageNumber - 1) * (int)pageSize)
                    .Take((int)pageSize);
        }
    }
}