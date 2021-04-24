using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace Logistic.BLL.Helpers
{
    public class CustomHttpClient
    {
        private readonly HttpClient _httpClient;

        protected CookieContainer _cookies = new CookieContainer();
        protected readonly Dictionary<string, string> _headers = new Dictionary<string, string>();
        internal CustomHttpClient()
        {
            var handler = new HttpClientHandler { CookieContainer = _cookies };
            _httpClient = new HttpClient(handler);
        }

        public static string UrlCombine(string url1, string url2)
        {
            if (url1.Length == 0)
            {
                return url2;
            }

            if (url2.Length == 0)
            {
                return url1;
            }

            url1 = url1.TrimEnd('/', '\\');
            url2 = url2.TrimStart('/', '\\');

            return string.Format("{0}/{1}", url1, url2);
        }
        public static string UrlCombine(params string[] urls)
        {
            if (urls.Length == 0)
            {
                return string.Empty;
            }
            else if (urls.Length == 1)
            {
                return urls[0].TrimEnd('/', '\\');
            }
            else if (urls.Length == 2)
            {
                UrlCombine(urls[0], urls[1]);
            }
            return UrlCombine(urls[0], UrlCombine(urls.Skip(1).ToArray()));
        }

        public async Task<BaseApiResponse<TF>> HttpSendAsync<TF>(string uri, string endPoint = "", object data = null,
            string mediaType = "application/json;charset=utf-8", HttpMethod method = null, IDictionary<string, string> headers = null,
            IDictionary<string, object> queryParameters = null, bool useGzip = false, DataType dataType = DataType.Json,
            DataType dataTypeResult = DataType.Json) where TF : new()
        {
            var result = new BaseApiResponse<TF>();
            try
            {
                if (useGzip)
                    _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));

                if (string.IsNullOrWhiteSpace(uri))
                    throw new FormatException("'Uri' cannot be empty or null.");

                var url = $"{uri.TrimEnd(' ', '/')}/{endPoint}".TrimEnd('/');
                var uriBuilder = new UriBuilder(url);
                var query = HttpUtility.ParseQueryString(uriBuilder.Query);

                if (queryParameters?.Count > 0)
                    foreach (var (key, value) in queryParameters.Where(parameter => !string.IsNullOrWhiteSpace(parameter.Key)))
                        query.Add(key, value?.ToString());

                uriBuilder.Query = query.ToString();
                var request = new HttpRequestMessage(method ?? HttpMethod.Get, uriBuilder.ToString());

                foreach (var (key, value) in _headers)
                    request.Headers.Add(key, value);

                if (headers != null)
                    foreach (var (key, value) in headers)
                        request.Headers.Add(key, value);

                if (data != null)
                {
                    switch (dataType)
                    {
                        case DataType.Json:
                            {
                                var stringContent = JsonConvert.SerializeObject(data);
                                request.Content = new StringContent(stringContent, Encoding.UTF8, mediaType);
                                break;
                            }
                        case DataType.Xml:
                            request.Content = new StringContent(data.ToString(), Encoding.UTF8, mediaType);
                            break;
                        case DataType.None:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(dataType), dataType, null);
                    }
                }

                var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    result.OriginalDataString = responseString;
                    result.Cookies = _cookies.GetCookies(request.RequestUri).Cast<Cookie>();

                    result.Success = response.IsSuccessStatusCode;


                    switch (dataTypeResult)
                    {
                        case DataType.Json:
                            result.Data = JsonConvert.DeserializeObject<TF>(responseString);
                            break;
                        case DataType.Xml:
                            {
                                using (var textReader = new StringReader(RemoveTypeTagFromXml(responseString)))
                                using (var reader = new XmlTextReader(textReader))
                                {
                                    var serializer = new XmlSerializer(typeof(TF));
                                    result.Data = (TF)serializer.Deserialize(reader);
                                }

                                break;
                            }
                    }
                }
                else
                    result.Errors = new[] { new Error { ErrorCode = (int)response.StatusCode, ErrorText = response.ReasonPhrase } };
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Errors = new[] { new Error { ErrorCode = 0, ErrorText = ex.Message } };
            }

            return result;
        }

        private string RemoveTypeTagFromXml(string xml)
        {
            if (!string.IsNullOrEmpty(xml) && xml.Contains("xsi:type"))
            {
                xml = Regex.Replace(xml, @"\s+xsi:type=""\w+""", "");
            }
            return xml;
        }
    }

    public enum DataType
    {
        Json,
        Xml,
        None
    }

    public class BaseApiResponse<T> : BaseApiResponse
    {
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public T Data { get; set; }
    }

    public class BaseApiResponse
    {
        [JsonProperty("success", NullValueHandling = NullValueHandling.Ignore)]
        public bool Success { get; set; }
        [JsonProperty("errors", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<Error> Errors { get; set; } = new List<Error>();
        public IEnumerable<Cookie> Cookies { get; set; }
        public string OriginalDataString { get; set; }
    }

    public class Error
    {
        public int ErrorCode { get; set; }
        public string ErrorText { get; set; }
    }
}
