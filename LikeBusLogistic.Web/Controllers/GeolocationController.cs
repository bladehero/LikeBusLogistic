using LikeBusLogistic.BLL;
using LikeBusLogistic.VM.ViewModels;
using LikeBusLogistic.Web.Models;
using LikeBusLogistic.Web.Models.Geolocations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LikeBusLogistic.Web.Controllers
{
    [Authorize]
    public class GeolocationController : BaseController
    {
        public GeolocationController(ServiceFactory serviceFactory) : base(serviceFactory) { }

        [HttpGet]
        public IActionResult _FullInformation(GeolocationTab tab = GeolocationTab.Locations)
        {
            var cities = ServiceFactory.GeolocationManagement.GetCities().Data;
            var districts = ServiceFactory.GeolocationManagement.GetDistricts().Data;
            var countries = ServiceFactory.GeolocationManagement.GetCountries().Data;
            var locations = ServiceFactory.GeolocationManagement.GetLocations().Data;

            var model = new FullInformationVM
            {
                Cities = cities,
                Districts = districts,
                Countries = countries,
                Locations = locations,
                Tab = tab
            };
            return PartialView(model);
        }
        [HttpGet]
        public IActionResult _Location()
        {
            return PartialView();
        }
        [HttpGet]
        public IActionResult _MergeCountry(int? id)
        {
            var model = new MergeCountryVM
            {
                Country = ServiceFactory.GeolocationManagement.GetCountry(id).Data
            };
            return PartialView(model);
        }
        [HttpGet]
        public IActionResult _MergeDistrict(int? id)
        {
            var model = new MergeDistrictVM
            {
                District = ServiceFactory.GeolocationManagement.GetDistrict(id).Data,
                Countries = ServiceFactory.GeolocationManagement.GetCountries().Data
            };
            return PartialView(model);
        }
        [HttpGet]
        public IActionResult _MergeCity(int? id)
        {
            var model = new MergeCityVM
            {
                City = ServiceFactory.GeolocationManagement.GetCity(id).Data,
                Districts = ServiceFactory.GeolocationManagement.GetDistricts().Data
            };
            return PartialView(model);
        }


        [HttpPost]
        public IActionResult MergeLocation(VM.ViewModels.LocationVM locationVM)
        {
            var result = new Result();
            try
            {
                var mergeLocationResult = ServiceFactory.GeolocationManagement.MergeLocation(locationVM);
                result.Success = mergeLocationResult.Success;
                result.Message = mergeLocationResult.Message;
            }
            catch (Exception ex)
            {
                result.Success = false;
            }
            return Json(result);
        }
        [HttpPost]
        public IActionResult MergeCountry(CountryVM countryVM)
        {
            var result = new Result();
            try
            {
                var mergeCountryResult = ServiceFactory.GeolocationManagement.MergeCountry(countryVM);
                result.Success = mergeCountryResult.Success;
                result.Message = mergeCountryResult.Message;
            }
            catch (Exception ex)
            {
                result.Success = false;
            }
            return Json(result);
        }
        [HttpPost]
        public IActionResult MergeDistrict(DistrictVM districtVM)
        {
            var result = new Result();
            try
            {
                var mergeDistrictResult = ServiceFactory.GeolocationManagement.MergeDistrict(districtVM);
                result.Success = mergeDistrictResult.Success;
                result.Message = mergeDistrictResult.Message;
            }
            catch (Exception ex)
            {
                result.Success = false;
            }
            return Json(result);
        }
        [HttpPost]
        public IActionResult MergeCity(CityVM cityVM)
        {
            var result = new Result();
            try
            {
                var mergeCityResult = ServiceFactory.GeolocationManagement.MergeCity(cityVM);
                result.Success = mergeCityResult.Success;
                result.Message = mergeCityResult.Message;
            }
            catch (Exception ex)
            {
                result.Success = false;
            }
            return Json(result);
        }
    }
}