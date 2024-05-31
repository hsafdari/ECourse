using static Infrastructure.Utility.SD;

namespace Infrastructure.Models
{
    public class RequestDto
    {
        public ApiType ApiType { get; set; } = ApiType.GET;
        public string Url { get; set; } = default!;
        public object Data { get; set; } = default!;
        public string AccessToken { get; set; } = default!;

        public ContentType ContentType { get; set; } = ContentType.Json;
    }
}
