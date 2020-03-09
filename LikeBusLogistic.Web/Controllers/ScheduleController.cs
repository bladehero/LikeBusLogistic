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
            var schedules = ServiceFactory.ScheduleManagement.GetSchedules().Data;

            var model = new FullInformationVM
            {
                Schedules = schedules,
                Tab = tab
            };
            return PartialView(model);
        }

        [HttpGet]
        public IActionResult _Schedule(ScheduleTab tab = ScheduleTab.Schedule)
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

        [HttpGet]
        public IActionResult _ScheduleRouteLocations(int? routeId)
        {
            var routeLocations = ServiceFactory.RouteManagement.GetRouteLocations(routeId).Data;

            var scheduleRouteLocations = new List<ScheduleRouteLocationVM>(routeLocations.Count());
            foreach (var routeLocation in routeLocations)
            {
                scheduleRouteLocations.Add(new ScheduleRouteLocationVM
                {
                   CityName = routeLocation.CurrentCityName,
                   CountryName = routeLocation.CurrentCountryName,
                   DistrictName = routeLocation.CurrentDistrictName,
                   LocationName = routeLocation.CurrentName,
                   RouteLocationId = routeLocation.RouteLocationId
                });
            }

            var model = new ScheduleRouteLocationsVM
            {
                ScheduleRouteLocations = scheduleRouteLocations
            };
            return PartialView(model);
        }

        [HttpPost]
        public IActionResult MergeSchedule(VM.ViewModels.ScheduleVM scheduleVM)
        {
            var result = new Result();
            try
            {
                var mergeScheduleResult = ServiceFactory.ScheduleManagement.MergeSchedule(scheduleVM);
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