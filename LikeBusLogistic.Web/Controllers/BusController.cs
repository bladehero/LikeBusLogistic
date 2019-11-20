using System;
using LikeBusLogistic.BLL;
using LikeBusLogistic.VM.ViewModels;
using LikeBusLogistic.Web.Models;
using LikeBusLogistic.Web.Models.Buses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LikeBusLogistic.Web.Controllers
{
    [Authorize]
    public class BusController : BaseController
    {
        public BusController(ServiceFactory serviceFactory) : base(serviceFactory)
        {
        }

        [HttpGet]
        public IActionResult _FullInformation()
        {
            var buses = ServiceFactory.BusManagement.GetBuses();
            var vehicles = ServiceFactory.BusManagement.GetVehicles();

            var model = new FullInformationVM
            {
                Buses = buses.Data,
                Vehicles = vehicles.Data
            };
            return PartialView(model);
        }

        [HttpGet]
        public IActionResult _Buses()
        {
            var buses = ServiceFactory.BusManagement.GetBuses();

            var model = new BusesVM
            {
                Buses = buses.Data
            };
            return PartialView(model);
        }

        [HttpGet]
        public IActionResult _Vehicles()
        {
            var vehicles = ServiceFactory.BusManagement.GetVehicles();

            var model = new VehiclesVM
            {
                Vehicles = vehicles.Data
            };
            return PartialView(model);
        }

        [HttpGet]
        //[Authorize(Policy = "CanChange")]
        public IActionResult _MergeBus(int? busId)
        {
            var bus = ServiceFactory.BusManagement.GetBus(busId);
            var vehicles = ServiceFactory.BusManagement.GetVehicles();

            var model = new MergeBusVM
            {
                Bus = bus.Data,
                Vehicles = vehicles.Data
            };
            return PartialView(model);
        }

        [HttpGet]
        //[Authorize(Policy = "CanChange")]
        public IActionResult _MergeVehicle(int? vehicleId)
        {
            var vehicle = ServiceFactory.BusManagement.GetVehicle(vehicleId);

            var model = new MergeVehicleVM
            {
                Vehicle = vehicle.Data
            };
            return PartialView(model);
        }


        [HttpPost]
        //[Authorize(Policy = "CanChange")]
        public IActionResult MergeBus(BusVM busVM)
        {
            var result = new Result();
            try
            {
                var mergeBusResult = ServiceFactory.BusManagement.MergeBus(busVM);
                result.Success = mergeBusResult.Success;
                result.Message = mergeBusResult.Message;
            }
            catch (Exception)
            {
                result.Success = false;
            }
            return Json(result);
        }

        [HttpPost]
        //[Authorize(Policy = "CanChange")]
        public IActionResult MergeVehicle(VehicleVM vehicleVM)
        {
            var result = new Result();
            try
            {
                var mergeVehicleResult = ServiceFactory.BusManagement.MergeVehicle(vehicleVM);
                result.Success = mergeVehicleResult.Success;
                result.Message = mergeVehicleResult.Message;
            }
            catch (Exception)
            {
                result.Success = false;
            }
            return Json(result);
        }

        [HttpPost]
        //[Authorize(Policy = "CanDelete")]
        public IActionResult DeleteOrRestoreVehicle(int vehicleId)
        {
            var result = new Result();
            try
            {
                var deleteOrRestoreVehicleResult = ServiceFactory.BusManagement.DeleteOrRestoreVehicle(vehicleId);
                result.Success = deleteOrRestoreVehicleResult.Success;
                result.Message = deleteOrRestoreVehicleResult.Message;
            }
            catch (Exception)
            {
                result.Success = false;
            }
            return Json(result);
        }

        [HttpPost]
        //[Authorize(Policy = "CanDelete")]
        public IActionResult DeleteOrRestoreBus(int busId)
        {
            var result = new Result();
            try
            {
                var deleteOrRestoreBusResult = ServiceFactory.BusManagement.DeleteOrRestoreBus(busId);
                result.Success = deleteOrRestoreBusResult.Success;
                result.Message = deleteOrRestoreBusResult.Message;
            }
            catch (Exception)
            {
                result.Success = false;
            }
            return Json(result);
        }
    }
}