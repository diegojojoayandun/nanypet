using System.Net;

namespace NanyPet.Api.Models
{
    public class APIResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public List<string> ErrorMessages { get; set; } = null!;
        public object Result { get; set; } = null!;
    }
}
