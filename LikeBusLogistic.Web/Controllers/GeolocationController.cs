using LikeBusLogistic.BLL;
using LikeBusLogistic.BLL.Services.TomTom.Models;
using LikeBusLogistic.VM.ViewModels;
using LikeBusLogistic.Web.Models;
using LikeBusLogistic.Web.Models.Geolocations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LikeBusLogistic.Web.Controllers
{
    [Authorize]
    public class GeolocationController : BaseController
    {
        public GeolocationController(ServiceFactory serviceFactory) : base(serviceFactory) { }

        [HttpGet]
        public IActionResult GetDistrictsByCountry(int? id)
        {
            var result = ServiceFactory.GeolocationManagement.GetDistrictsByCountry(id);

            return Json(result);
        }
        [HttpGet]
        public IActionResult GetCitiesByCountry(int? id)
        {
            var result = ServiceFactory.GeolocationManagement.GetCitiesByCountry(id);

            return Json(result);
        }
        
        [HttpGet]
        public IActionResult GetCitiesByDistricts(int? id)
        {
            var result = ServiceFactory.GeolocationManagement.GetCitiesByDistrict(id);

            return Json(result);
        }

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
        public async Task<IActionResult> TryGetLocationByPoint(double latitude, double longitude)
        {
            var result = new Result<VM.ViewModels.LocationVM>();
            try
            {
                var point = new LocationPoint { Latitude = latitude, Longitude = longitude };
                var geocode = await ServiceFactory.TomTom.ReverseGeocode(point);
                if (geocode.Success)
                {
                    var address = geocode.Data?.Addresses?.FirstOrDefault()?.Address;

                    if (address != null)
                    {
                        var city = ServiceFactory.GeolocationManagement.GetCities().Data
                            .FirstOrDefault(x => x.Name == address.LocalName
                                            || x.Name == address.Municipality
                                            || x.Name == address.MunicipalitySubdivision
                                            || x.Name == address.CountrySecondarySubdivision);

                        var district = ServiceFactory.GeolocationManagement.GetDistricts().Data
                                .FirstOrDefault(x => x.Id == city?.DistrictId 
                                                || x.Name == address.CountrySubdivision);

                        var country = ServiceFactory.GeolocationManagement.GetCountries().Data
                                .FirstOrDefault(x => x.Id == city?.CountryId
                                                || x.Id == district?.CountryId
                                                || x.Name == address.Country
                                                || x.ShortName == address.CountryCode);

                        result.Data = new VM.ViewModels.LocationVM
                        {
                            CityId = city?.Id,
                            CityName = city?.Name,
                            DistrictId = city?.DistrictId ?? district?.Id,
                            DistrictName = city?.DistrictName ?? district?.Name,
                            CountryId = city?.CountryId ?? district?.CountryId ?? country?.Id,
                            CountryName = city?.CountryName ?? district?.CountryName ?? country?.Name,
                            Name = address.Street
                        };
                    }
                }
                result.Success = geocode.Success;
                result.Message = geocode.Message;
            }
            catch (Exception ex)
            {
                result.Success = false;
            }
            return Json(result);
        }

        [HttpGet]
        public IActionResult _Locations()
        {
            var locations = ServiceFactory.GeolocationManagement.GetLocations().Data;
            return PartialView(locations);
        }
        [HttpGet]
        public IActionResult _Cities()
        {
            var cities = ServiceFactory.GeolocationManagement.GetCities().Data;
            return PartialView(cities);
        }
        [HttpGet]
        public IActionResult _Districts()
        {
            var districts = ServiceFactory.GeolocationManagement.GetDistricts().Data;
            return PartialView(districts);
        }
        [HttpGet]
        public IActionResult _Countries()
        {
            var countries = ServiceFactory.GeolocationManagement.GetCountries().Data;
            return PartialView(countries);
        }

        [HttpGet]
        public IActionResult _Location(int? id)
        {
            var location = ServiceFactory.GeolocationManagement.GetLocation(id).Data;
            var isCarRepair = ServiceFactory.GeolocationManagement.GetRepairSpecialistsByLocationId(id).Data?.Count() > 0;

            var model = new Models.Geolocations.LocationVM
            {
                Location = location,
                Countries = ServiceFactory.GeolocationManagement.GetCountries().Data,
                IsCarRepair = isCarRepair
            };

            if (id.HasValue)
            {
                if (location.CountryId.HasValue)
                {
                    var districts = ServiceFactory.GeolocationManagement.GetDistrictsByCountry(location.CountryId.Value).Data;
                    model.Districts = districts;
                }

                if (location.DistrictId.HasValue)
                {
                    var cities = ServiceFactory.GeolocationManagement.GetCitiesByDistrict(location.DistrictId.Value).Data;
                    model.Cities = cities;
                }
            }

            return PartialView(model);
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
        [HttpGet]
        public IActionResult _RepairSpecialistsForLocation(int? locationId)
        {
            var location = ServiceFactory.GeolocationManagement.GetLocation(locationId).Data;
            var specialists = ServiceFactory.GeolocationManagement.GetRepairSpecialistsByLocationId(locationId).Data;

            var model = new RepairSpecialistsForLocationVM
            {
                Location = location,
                RepairSpecialists = specialists
            };
            return PartialView(model);
        }
        [HttpGet]
        public IActionResult GetRepairSpecialist(int specialistId)
        {
            var result = ServiceFactory.GeolocationManagement.GetRepairSpecialist(specialistId);

            return Json(result);
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
        [HttpPost]
        public IActionResult MergeRepairSpecialist(RepairSpecialistVM repairSpecialistVM)
        {
            var result = new Result();
            try
            {
                var mergeRepairSpecialistResult = 
                    ServiceFactory.GeolocationManagement.MergeRepairSpecialist(repairSpecialistVM);
                result.Success = mergeRepairSpecialistResult.Success;
                result.Message = mergeRepairSpecialistResult.Message;
            }
            catch (Exception ex)
            {
                result.Success = false;
            }
            return Json(result);
        }


        [HttpPost]
        public IActionResult DeleteOrRestoreLocation(int id)
        {
            var result = new Result();
            try
            {
                var deleteOrRestoreLocationResult = ServiceFactory.GeolocationManagement.DeleteOrRestoreLocation(id);
                result.Success = deleteOrRestoreLocationResult.Success;
                result.Message = deleteOrRestoreLocationResult.Message;
            }
            catch (Exception)
            {
                result.Success = false;
            }
            return Json(result);
        }
        [HttpPost]
        public IActionResult DeleteOrRestoreCountry(int id)
        {
            var result = new Result();
            try
            {
                var deleteOrRestoreCountryResult = ServiceFactory.GeolocationManagement.DeleteOrRestoreCountry(id);
                result.Success = deleteOrRestoreCountryResult.Success;
                result.Message = deleteOrRestoreCountryResult.Message;
            }
            catch (Exception)
            {
                result.Success = false;
            }
            return Json(result);
        }
        [HttpPost]
        public IActionResult DeleteOrRestoreDistrict(int id)
        {
            var result = new Result();
            try
            {
                var deleteOrRestoreDistrictResult = ServiceFactory.GeolocationManagement.DeleteOrRestoreDistrict(id);
                result.Success = deleteOrRestoreDistrictResult.Success;
                result.Message = deleteOrRestoreDistrictResult.Message;
            }
            catch (Exception)
            {
                result.Success = false;
            }
            return Json(result);
        }
        [HttpPost]
        public IActionResult DeleteOrRestoreCity(int id)
        {
            var result = new Result();
            try
            {
                var deleteOrRestoreCityResult = ServiceFactory.GeolocationManagement.DeleteOrRestoreCity(id);
                result.Success = deleteOrRestoreCityResult.Success;
                result.Message = deleteOrRestoreCityResult.Message;
            }
            catch (Exception)
            {
                result.Success = false;
            }
            return Json(result);
        }
        [HttpPost]
        public IActionResult DeleteOrRestoreSpecialist(int id)
        {
            var result = new Result();
            try
            {
                var deleteOrRestoreCityResult = 
                    ServiceFactory.GeolocationManagement.DeleteOrRestoreRepairSpecialist(id);
                result.Success = deleteOrRestoreCityResult.Success;
                result.Message = deleteOrRestoreCityResult.Message;
            }
            catch (Exception)
            {
                result.Success = false;
            }
            return Json(result);
        }
    }
}