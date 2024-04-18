using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;
using System.Net;
using System.Text;
using static ECourse.Admin.Utility.SD;
using ECourse.Admin.Models.CourseAPI.CourseLevel;
using Newtonsoft.Json;
using ECourse.Admin.Models;

namespace ECourse.Admin.Service
{
    public class BaseService<TModel> : IBaseService<TModel> where TModel : class
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenProvider _tokenProvider;
        protected string _apiUrl { get; set; }
        public BaseService(IHttpClientFactory httpClientFactory, ITokenProvider tokenProvider)
        {
            _httpClientFactory = httpClientFactory;
            _tokenProvider = tokenProvider;

        }
        public async Task<ResponseDto?> CreateAsync(TModel entity)
        {
            return await SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Data = entity,
                Url = _apiUrl,
                ContentType = ContentType.MultipartFormData
            });
        }

        public async Task<ResponseDto?> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto?> GetByIdAsync(string id)
        {
            return await SendAsync(new RequestDto()
            {
                Url = _apiUrl + "/{id}"
            });
        }

        private async Task<ResponseDto?> SendAsync(RequestDto requestDto, bool withBearer = false)
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("ECourseAPI");
                HttpRequestMessage message = new();
                if (requestDto.ContentType == ContentType.MultipartFormData)
                {
                    message.Headers.Add("Accept", "*/*");
                }
                else
                {
                    message.Headers.Add("Accept", "application/json");
                }
                //token
                if (withBearer)
                {
                    var token = _tokenProvider.GetToken();
                    message.Headers.Add("Authorization", $"Bearer {token}");
                }

                message.RequestUri = new Uri(requestDto.Url);

                if (requestDto.ContentType == ContentType.MultipartFormData)
                {
                    var content = new MultipartFormDataContent();

                    foreach (var prop in requestDto.Data.GetType().GetProperties())
                    {
                        var value = prop.GetValue(requestDto.Data);
                        if (value is FormFile)
                        {
                            var file = (FormFile)value;
                            if (file != null)
                            {
                                content.Add(new StreamContent(file.OpenReadStream()), prop.Name, file.FileName);
                            }
                        }
                        else
                        {
                            content.Add(new StringContent(value == null ? "" : value.ToString()), prop.Name);
                        }
                    }
                    message.Content = content;
                }
                else
                {
                    if (requestDto.Data != null)
                    {
                        message.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, "application/json");
                    }
                }
                HttpResponseMessage? apiResponse = null;

                switch (requestDto.ApiType)
                {
                    case ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    case ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }

                apiResponse = await client.SendAsync(message);

                switch (apiResponse.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return new() { IsSuccess = false, Message = "Not Found" };
                    case HttpStatusCode.Forbidden:
                        return new() { IsSuccess = false, Message = "Access Denied" };
                    case HttpStatusCode.Unauthorized:
                        return new() { IsSuccess = false, Message = "Unauthorized" };
                    case HttpStatusCode.InternalServerError:
                        return new() { IsSuccess = false, Message = "Internal Server Error" };
                    default:
                        var apiContent = await apiResponse.Content.ReadAsStringAsync();
                        var apiResponseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
                        return apiResponseDto;
                }
            }
            catch (Exception ex)
            {
                var dto = new ResponseDto
                {
                    Message = ex.Message.ToString(),
                    IsSuccess = false
                };
                return dto;
            }
        }

        public async Task<ResponseDto?> UpdateAsync(TModel entity)
        {
            throw new NotImplementedException();
        }
        public async Task<ResponseDto?> GetAllAsync()
        {
            return await SendAsync(new RequestDto()
            {
                Url = _apiUrl
            });
        }
    }
}
