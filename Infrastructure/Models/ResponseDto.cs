namespace Infrastructure.Models
{
    public class ResponseDto
    {
        public object? Result { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = "";
        /// <summary>
        /// Total Row Affected for Grid
        /// </summary>
        public int Count { get; set; }
    }
}
