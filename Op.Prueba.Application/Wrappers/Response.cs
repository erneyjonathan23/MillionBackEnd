using System.Net;

namespace OP.Prueba.Application.Wrappers
{
    public class Response<T>
    {
        public Response()
        {

        }
        public Response(T data, string message = null, HttpStatusCode httpCode = HttpStatusCode.OK)
        {
            Succeeded = true;
            Message = message;
            Data = data;
            HttpCode = httpCode;
        }
        public Response(string message, HttpStatusCode httpCode = HttpStatusCode.NotFound)
        {
            Succeeded = false;
            Message = message;
            HttpCode = httpCode;
        }
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public List<string> Erros { get; set; }
        public T Data { get; set; }
        public HttpStatusCode HttpCode { get; set; }
    }
}
