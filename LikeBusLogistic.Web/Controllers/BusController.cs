using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LikeBusLogistic.BLL;
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
        public IActionResult _MergeVehicle(int? vehicleId)
        {
            var vehicle = ServiceFactory.BusManagement.GetVehicle(vehicleId);

            var model = new MergeVehicleVM
            {
                Vehicle = vehicle.Data
            };
            return PartialView(model);
        }
    }
}