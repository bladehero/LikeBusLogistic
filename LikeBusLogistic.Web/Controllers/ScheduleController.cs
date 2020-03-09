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
        public IActionResult _MergeSchedule(int? scheduleId)
        {
            return PartialView();
        }

        [HttpPost]
        public IActionResult MergeSchedule(ScheduleVM scheduleVM)
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