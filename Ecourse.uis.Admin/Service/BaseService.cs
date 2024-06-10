using ECourse.Admin.Models;
using ECourse.Admin.Utility;
using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json;
using Radzen;
using System;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Web;
using static ECourse.Admin.Utility.SD;

namespace ECourse.Admin.Service
{
    public class BaseService<TModel> : IBaseService<TModel> where TModel : class
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenProvider _tokenProvider;
        
        protected string ApiUrl { get; set; } = string.Empty;
        public BaseService(IHttpClientFactory httpClientFactory, ITokenProvider tokenProvider)
        {
            _httpClientFactory = httpClientFactory;
            _tokenProvider = tokenProvider;

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
                        // if (value is FormFile) 
                        if (value is IBrowserFile)
                        {
                            var file = (IBrowserFile)value;
                            if (file != null)
                            {
                                content.Add(new StreamContent(file.OpenReadStream()), prop.Name, file.Name);
                            }
                        }
                        else
                        {
                            if (value == null)
                            {
                                content.Add(new StringContent(""), prop.Name);
                            }
                            else
                            {
                                content.Add(new StringContent(Convert.ToString(value)), prop.Name);
                            }
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
                message.Method = requestDto.ApiType switch
                {
                    ApiType.POST => HttpMethod.Post,
                    ApiType.DELETE => HttpMethod.Delete,
                    ApiType.PUT => HttpMethod.Put,
                    _ => HttpMethod.Get,
                };
                HttpResponseMessage? apiResponse = null;
                if (message.Method== HttpMethod.Delete)
                {
                  apiResponse = await client.DeleteAsync(message.RequestUri);
                }else
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
                    case HttpStatusCode.BadRequest:
                        return new() { IsSuccess = false, Message = "Bad Request" };
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
        public async Task<ResponseDto?> CreateAsync(TModel entity)
        {
            return await SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Data = entity,
                Url = ApiUrl,
                ContentType = ContentType.MultipartFormData
            });
        }

        public async Task<ResponseDto?> DeleteAsync(string id)
        {
            return await SendAsync(new RequestDto()
            {
                ApiType = ApiType.DELETE,
                Url = ApiUrl + $"/{id}"
            });
        }
        public async Task<ResponseDto?> DeleteAsync(List<string> ids)
        {
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            foreach (var item in ids)
            {
                queryString.Add("ids", item);
            }
            return await SendAsync(new RequestDto()
            {
                ApiType = ApiType.DELETE,
                Url = ApiUrl + "/deletemany?" + queryString.ToString()
            }); ;
        }

        public async Task<ResponseDto?> GetByIdAsync(string id)
        {
            return await SendAsync(new RequestDto()
            {
                Url = ApiUrl + $"/{id}"
            });
        }
        public async Task<ResponseDto?> UpdateAsync(TModel entity)
        {
            return await SendAsync(new RequestDto()
            {
                ApiType = ApiType.PUT,
                Data = entity,
                Url = ApiUrl,
                ContentType = ContentType.MultipartFormData
            });
        }
        public async Task<ResponseDto?> GetAllAsync()
        {
            return await SendAsync(new RequestDto()
            {
                Url = ApiUrl
            });
        }
        public async Task<ResponseDto?> GetGrid(Query query)
        {
            return await SendAsync(new RequestDto()
            {
                //Url = ApiUrl + $"/Grid?&filter={query.Filter}&top={query.Top}&skip={query.Skip}&orderby={query.OrderBy}"
                Url = gridfilterurl(ApiUrl + $"/Grid", query)
            });
        }
        public string gridfilterurl(string url,Query query)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            if (query.Skip.HasValue)
            {
                dictionary.Add("skip", query.Skip.Value);
            }

            if (query.Top.HasValue)
            {
                dictionary.Add("top", query.Top.Value);
            }

            if (!string.IsNullOrEmpty(query.OrderBy))
            {
                dictionary.Add("orderBy", query.OrderBy);
            }

            if (!string.IsNullOrEmpty(query.Filter))
            {
                dictionary.Add("filter", UrlEncoder.Default.Encode(query.Filter));
            }
            if (!string.IsNullOrEmpty(query.Select))
            {
                dictionary.Add("select", query.Select);
            }

            return string.Format("{0}{1}", url, dictionary.Any() ? ("?" + string.Join("&", dictionary.Select((KeyValuePair<string, object> a) => $"{a.Key}={a.Value}"))) : "");
        }

        
    }
}
