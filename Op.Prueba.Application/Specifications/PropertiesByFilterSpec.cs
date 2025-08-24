using Ardalis.Specification;
using Op.Prueba.Domain.Entities;

namespace Op.Prueba.Application.Specifications
{
    public class PropertiesByFilterSpec : Specification<Property>
    {
        public PropertiesByFilterSpec(string? name, string? address, decimal? minPrice, decimal? maxPrice, int? pageNumber = null, int? pageSize = null)
        {
            if (!string.IsNullOrEmpty(name))
                Query.Where(p => p.Name.Contains(name));

            if (!string.IsNullOrEmpty(address))
                Query.Where(p => p.Address.Contains(address));

            if (minPrice.HasValue && maxPrice.HasValue)
                Query.Where(p => p.Price >= minPrice.Value && p.Price <= maxPrice.Value);

            if (pageSize != null && pageNumber != null)
                Query.OrderBy(p => p.Name)
                    .Skip(((int)pageNumber - 1) * (int)pageSize)
                    .Take((int)pageSize);
        }
    }

}