using Ardalis.Specification;
using Op.Prueba.Domain.Entities;
using System.Xml.Linq;

namespace Op.Prueba.Application.Specifications
{
    public class PropertyTracesByPropertySpec : Specification<PropertyTrace>
    {
        public PropertyTracesByPropertySpec(string? propertyId, int? pageNumber = null, int? pageSize = null)
        {
            if (!string.IsNullOrEmpty(propertyId))
                Query.Where(p => p.Name.Contains(propertyId));

            if (pageSize != null && pageNumber != null)
                Query.OrderBy(p => p.Name)
                    .Skip(((int)pageNumber - 1) * (int)pageSize)
                    .Take((int)pageSize);
        }
    }

}