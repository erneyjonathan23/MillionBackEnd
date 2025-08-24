using System.Net;

namespace OP.Prueba.Application.Wrappers
{
    public class PagedResponse<T> : Response<T>
    {
        public int? PageNumber { get; set; } = 1;
        public int? PageSize { get; set; } = 10;
        public int? TotalItems { get; set; }
        public PagedResponse(T data, int pageNumber, int pageSize, int? totalItems = null, HttpStatusCode httpCode = HttpStatusCode.OK)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.Data = data;
            this.Message = null;
            this.Succeeded = true;
            this.Erros = null;
            this.TotalItems = totalItems;
            this.HttpCode = httpCode;
        }
    }
}