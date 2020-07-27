using System.Net;

namespace Globe.Identity.Middlewares
{
    public class ApiServerError
    {
        public string Message { get; set; }
        public string Details { get; set; }
        public string StackTrace { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
