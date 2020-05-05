using LikeBusLogistic.BLL.Helpers;
using LikeBusLogistic.BLL.Results;
using LikeBusLogistic.BLL.Services.TomTom.Models;
using LikeBusLogistic.BLL.Services.TomTom.Models.ReverseGeocoding;
using LikeBusLogistic.BLL.Services.TomTom.Models.Routing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

namespace LikeBusLogistic.BLL.Services.TomTom
{
    public class TomTomService : BaseService
    {
        private const string ApiKey = "F1GGRXkIAs6J0ToYZhgxoSafiTEjjKN0";
        private const string Url = "https://api.tomtom.com";
        private const string RouteEndPoint = "routing";
        private const string SearchEndPoint = "search";
        private const string Version_1 = "1";
        private const string Version_2 = "2";
        private const string ResultType = "json";
        private const string TravelMode = "bus";
        private const string Language = "ru-RU";

        private readonly CustomHttpClient _httpClient = new CustomHttpClient();

        public TomTomService(string connection) : base(connection) { }

        [Description("calculateRoute")]
        public async Task<BaseResult<CalculateRouteResult>> CalculateRoute(LocationPoint point1, LocationPoint point2)
        {
            var result = new BaseResult<CalculateRouteResult>();
            try
            {
                var action = MethodBase.GetCurrentMethod().GetRealMethodFromAsyncMethod().GetCustomAttribute<DescriptionAttribute>(true).Description;
                var routeLocations = HttpUtility.UrlEncode($"{point1}:{point2}");
                var endPoint = CustomHttpClient.UrlCombine(RouteEndPoint,
                                                           Version_1,
                                                           action,
                                                           routeLocations,
                                                           ResultType);
                var dictionary = new Dictionary<string, object>() { { "key", ApiKey }, { "travelMode", TravelMode } };

                var response = await _httpClient.HttpSendAsync<CalculateRouteResult>(Url,
                                                                                     endPoint,
                                                                                     method: HttpMethod.Get,
                                                                                     queryParameters: dictionary);
                result.OriginalString = response.OriginalDataString;
                if (result.Success = response.Success)
                {
                    result.Data = response.Data;
                    result.Message = GeneralSuccessMessage;
                }
                else
                {
                    result.Message = GeneralErrorMessage;
                }
            }
            catch (Exception ex)
            {
                result.Data = null;
                result.Message = GeneralErrorMessage;
            }
            return result;
        }

        [Description("reverseGeocode")]
        public async Task<BaseResult<ReverseGeocodeResult>> ReverseGeocode(LocationPoint point)
        {
            var result = new BaseResult<ReverseGeocodeResult>();
            try
            {
                var action = MethodBase.GetCurrentMethod().GetRealMethodFromAsyncMethod().GetCustomAttribute<DescriptionAttribute>(true).Description;
                var location = HttpUtility.UrlEncode($"{point}.{ResultType}");
                var endPoint = CustomHttpClient.UrlCombine(SearchEndPoint,
                                                           Version_2,
                                                           action,
                                                           location);
                var dictionary = new Dictionary<string, object>() { { "key", ApiKey }, { "language", Language } };

                var response = await _httpClient.HttpSendAsync<ReverseGeocodeResult>(Url,
                                                                                     endPoint,
                                                                                     method: HttpMethod.Get,
                                                                                     queryParameters: dictionary);
                result.OriginalString = response.OriginalDataString;
                if (result.Success = response.Success)
                {
                    result.Data = response.Data;
                    result.Message = GeneralSuccessMessage;
                }
                else
                {
                    result.Message = GeneralErrorMessage;
                }
            }
            catch (Exception ex)
            {
                result.Data = null;
                result.Message = GeneralErrorMessage;
            }
            return result;
        }
    }
}
