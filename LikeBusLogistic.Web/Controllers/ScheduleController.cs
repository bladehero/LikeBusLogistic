using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LikeBusLogistic.BLL;
using LikeBusLogistic.VM.ViewModels;
using LikeBusLogistic.Web.Models;
using LikeBusLogistic.Web.Models.Schedule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LikeBusLogistic.Web.Controllers
{
    [Authorize]
    public class ScheduleController : BaseController
    {
        public ScheduleController(ServiceFactory serviceFactory) : base(serviceFactory) { }

        [HttpGet]
        public IActionResult _FullInformation(ScheduleTab tab = ScheduleTab.Schedule)
        {
            var model = new FullInformationVM
            {
                Tab = tab
            };
            return PartialView(model);
        }

        [HttpGet]
        public IActionResult _Schedule()
        {
            var schedules = ServiceFactory.ScheduleManagement.GetSchedules().Data;

            var model = new Models.Schedule.ScheduleVM
            {
                Schedules = schedules,
            };
            return PartialView(model);
        }

        [HttpGet]
        public IActionResult _MergeSchedule(int? scheduleId)
        {
            var schedule = ServiceFactory.ScheduleManagement.GetSchedule(scheduleId).Data;
            var routes = ServiceFactory.RouteManagement.GetRoutes().Data;

            var model = new MergeScheduleVM
            {
                Schedule = schedule,
                Routes = routes,
            };
            return PartialView(model);
        }

        public IActionResult _ScheduleInfo(int? scheduleId, bool isPrint = false)
        {
            var schedule = ServiceFactory.ScheduleManagement.GetSchedule(scheduleId).Data;
            var scheduleRouteLocations = ServiceFactory.ScheduleManagement.GetScheduleRouteLocations(scheduleId).Data;


            double totalDistance = Math.Round(scheduleRouteLocations.Sum(x => x.ScheduleLocationDistance), 1);
            int totalTime = 0;
            ScheduleRouteLocationVM previousLocation = null;
            foreach (var currentLocation in scheduleRouteLocations)
            {
                if (previousLocation != null)
                {
                    var minutes = (currentLocation.ScheduleLocationArrivalTime - previousLocation.ScheduleLocationDepartureTime).Value.TotalMinutes;
                    var duration = (int)(minutes < 0 ? minutes + new TimeSpan(1, 0, 0, 0).TotalMinutes : minutes);
                    totalTime += duration;
                }
                previousLocation = currentLocation;
            }
            var first = scheduleRouteLocations.First();
            var last = scheduleRouteLocations.Last();

            var model = new ScheduleInfoVM
            {
                Schedule = schedule,
                ScheduleRouteLocations = scheduleRouteLocations,
                First = first,
                Last = last,
                TotalDistance = totalDistance,
                TotalTime = new TimeSpan(0, totalTime, 0),
                IsPrint = isPrint
            };
            if (isPrint)
            {
                return View(model);
            }
            else
            {
                return PartialView(model);
            }
        }

        [HttpGet]
        public IActionResult _ScheduleRouteLocations(int? routeId, int? scheduleId)
        {
            if (!scheduleId.HasValue && !routeId.HasValue)
            {
                return Content("");
            }

            var model = new ScheduleRouteLocationsVM();
            if (scheduleId.HasValue)
            {
                var schedule = ServiceFactory.ScheduleManagement.GetSchedule(scheduleId).Data;
                if (!routeId.HasValue)
                {
                    var scheduleRouteLocations = ServiceFactory.ScheduleManagement.GetScheduleRouteLocations(scheduleId).Data;
                    model.ScheduleRouteLocations = scheduleRouteLocations;
                }
                var hasConfirmedTrips = ServiceFactory.ScheduleManagement.HasConfirmedTripsByRouteId(schedule.RouteId).Data;
                model.HideAutoSelect = schedule.NeedsSync && !routeId.HasValue;
                model.HasConfirmedTrips = hasConfirmedTrips;
                model.NeedsSync = schedule.NeedsSync;
            }

            if (routeId.HasValue)
            {
                var routeLocations = ServiceFactory.RouteManagement.GetRouteLocations(routeId).Data;
                var scheduleRouteLocations = new List<ScheduleRouteLocationVM>(routeLocations.Count());
                foreach (var routeLocation in routeLocations)
                {
                    scheduleRouteLocations.Add(new ScheduleRouteLocationVM
                    {
                        ScheduleLocationCurrentCityName = routeLocation.CurrentCityName,
                        ScheduleLocationCurrentCountryName = routeLocation.CurrentCountryName,
                        ScheduleLocationCurrentDistrictName = routeLocation.CurrentDistrictName,
                        ScheduleLocationCurrentName = routeLocation.CurrentName,
                        ScheduleCurrentLocationId = routeLocation.CurrentLocationId,
                        SchedulePreviousLocationId = routeLocation.PreviousLocationId,
                        ScheduleLocationDistance = routeLocation.Distance,
                        ScheduleLocationIsBoundary = (routeLocation.PreviousLocationId.HasValue && routeLocation.PreviousLocationId == routeLocation.CurrentLocationId)
                    });
                }

                model.ScheduleRouteLocations = scheduleRouteLocations;
            }
            return PartialView(model);
        }

        [HttpPost]
        public IActionResult MergeSchedule(VM.ViewModels.ScheduleVM schedule, IEnumerable<ScheduleRouteLocationVM> locations)
        {
            var result = new Result();
            try
            {
                var mergeScheduleResult = ServiceFactory.ScheduleManagement.MergeSchedule(schedule, locations);
                result.Success = mergeScheduleResult.Success;
                result.Message = mergeScheduleResult.Message;
            }
            catch (Exception)
            {
                result.Success = false;
            }
            return Json(result);
        }

        [HttpPost]
        public IActionResult DeleteOrRestoreSchedule(int scheduleId)
        {
            var result = new Result();
            try
            {
                var deleteOrRestoreScheduleResult =
                    ServiceFactory.ScheduleManagement.DeleteOrRestoreSchedule(scheduleId);
                result.Success = deleteOrRestoreScheduleResult.Success;
                result.Message = deleteOrRestoreScheduleResult.Message;
            }
            catch (Exception)
            {
                result.Success = false;
            }
            return Json(result);
        }
    }
}