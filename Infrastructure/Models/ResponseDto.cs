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
        public long Count { get; set; }
        public ResponseDto ResultSuccessful(object result,string message)
        {
            return new ResponseDto()
            {
                Result = result,
                Message = message,
            };
        }
        public ResponseDto ResultGridSuccessful(object result, string message,long count)
        {
            return new ResponseDto()
            {
                Result = result,
                Message = message,
                Count=count
            };
        }
        public ResponseDto ResultErrorBadRequest(string message)
        {
            return new ResponseDto()
            {
                Result = null,
                Message = message,
                IsSuccess = false
            };
        }
        public ResponseDto ResultErrorException(string message)
        {
            return new ResponseDto()
            {
                Result = null,
                Message = message,
                IsSuccess = false
            };
        }
    }
}
