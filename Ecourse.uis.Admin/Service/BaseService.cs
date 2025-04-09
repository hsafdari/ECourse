using ECourse.Admin.Models;
using ECourse.Admin.Utility;
using Newtonsoft.Json;
using Radzen;
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
        protected async Task<ResponseDto?> SendAsync(RequestDto requestDto, bool withBearer = false)
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("ECourseAPI");
                HttpRequestMessage message = new();
                message.Headers.Add("Accept", "application/json");
                //token
                if (withBearer)
                {
                    string? token = _tokenProvider.GetToken();
                    message.Headers.Add("Authorization", $"Bearer {token}");
                }
                message.RequestUri = new Uri(requestDto.Url);
                client.DefaultRequestHeaders.Clear();
                if (requestDto.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, "application/json");
                }
                message.Method = requestDto.ApiType switch
                {
                    ApiType.POST => HttpMethod.Post,
                    ApiType.DELETE => HttpMethod.Delete,
                    ApiType.PUT => HttpMethod.Put,
                    _ => HttpMethod.Get,
                };
                HttpResponseMessage? apiResponse = message.Method == HttpMethod.Delete ? await client.DeleteAsync(message.RequestUri) : await client.SendAsync(message);
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
                        string apiContent = await apiResponse.Content.ReadAsStringAsync();
                        ResponseDto? apiResponseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
                        return apiResponseDto;
                }
            }
            catch (Exception ex)
            {
                ResponseDto dto = new()
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
                ContentType = ContentType.Json
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
            System.Collections.Specialized.NameValueCollection queryString = HttpUtility.ParseQueryString(string.Empty);
            ids.ForEach(id => queryString.Add("ids", id));            
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
                ContentType = ContentType.Json
            });
        }
        public async Task<ResponseDto?> GetAllAsync()
        {
            return await SendAsync(new RequestDto()
            {
                Url = ApiUrl
            });
        }
        public async Task<ResponseDto?> GetGrid(GridQuery query)
        {
            _ = gridfilterurl(ApiUrl + $"/Grid", query);
            return await SendAsync(new RequestDto()
            {
                Url = gridfilterurl(ApiUrl + $"/Grid", query)
            });
        }
        public string gridfilterurl(string url, GridQuery query)
        {
            Dictionary<string, object> dictionary = [];
            if (query.Page.HasValue)
            {
                dictionary.Add("page", query.Page);
            }
            if (query.Skip.HasValue)
            {
                dictionary.Add("skip", query.Skip);
            }

            if (query.Top.HasValue)
            {
                //equal page size for grid
                dictionary.Add("top", query.Top);
            }

            if (!string.IsNullOrEmpty(query.SortColumn))
            {
                dictionary.Add("sortColumn", query.SortColumn);
            }
            if (!string.IsNullOrEmpty(query.SortOrder))
            {
                dictionary.Add("sortOrder", query.SortOrder);
            }

            if (!string.IsNullOrEmpty(query.FilterColumn))
            {
                dictionary.Add("filterColumn", UrlEncoder.Default.Encode(query.FilterColumn));
            }
            if (!string.IsNullOrEmpty(query.FilterValue))
            {
                dictionary.Add("filterValue", query.FilterValue);
            }
            if (!string.IsNullOrEmpty(query.Filter))
            {
                dictionary.Add("Filter", query.Filter);
            }

            return string.Format("{0}{1}", url, dictionary.Any() ? ("?" + string.Join("&", dictionary.Select((KeyValuePair<string, object> a) => $"{a.Key}={a.Value}"))) : "");
        }


    }
}
