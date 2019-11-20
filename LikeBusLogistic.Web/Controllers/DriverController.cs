using LikeBusLogistic.BLL;
using LikeBusLogistic.Web.Models.Drivers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LikeBusLogistic.Web.Controllers
{
    [Authorize]
    public class DriverController : BaseController
    {
        public DriverController(ServiceFactory serviceFactory) : base(serviceFactory)
        {
        }

        [HttpGet]
        public IActionResult _FullInformation()
        {
            var drivers = ServiceFactory.DriverManagement.GetDrivers();
            var contacts = ServiceFactory.DriverManagement.GetDriverContacts();

            var model = new FullInformationVM
            {
                Drivers = drivers.Data,
                Contacts = contacts.Data
            };
            return PartialView(model);
        }

        [HttpGet]
        public IActionResult _Information()
        {
            var drivers = ServiceFactory.DriverManagement.GetDrivers();

            var model = new InformationVM
            {
                Drivers = drivers.Data,
            };
            return PartialView(model);
        }

        [HttpGet]
        public IActionResult _Contacts()
        {
            var contacts = ServiceFactory.DriverManagement.GetDriverContacts();

            var model = new ContactsVM
            {
                Contacts = contacts.Data,
            };
            return PartialView(model);
        }







        [HttpGet]
        public IActionResult _MergeDriver(int? driverId)
        {
            var driver = ServiceFactory.DriverManagement.GetDriverInfo(driverId);
            var buses = ServiceFactory.BusManagement.GetBuses();

            var model = new MergeDriverVM
            {
                Driver = driver.Data,
                Buses = buses.Data
            };
            return PartialView(model);
        }

        [HttpGet]
        public IActionResult _MergeContact(int? contactId)
        {
            var contact = ServiceFactory.DriverManagement.GetDriverContact(contactId);
            var drivers = ServiceFactory.DriverManagement.GetDrivers();

            var model = new MergeContactVM
            {
                Contact = contact.Data,
                Drivers = drivers.Data
            };
            return PartialView(model);
        }

        //[HttpPost]
        //public IActionResult MergeBus(BusVM busVM)
        //{
        //    var result = new Result();
        //    try
        //    {
        //        var mergeBusResult = ServiceFactory.BusManagement.MergeBus(busVM);
        //        result.Success = mergeBusResult.Success;
        //        result.Message = mergeBusResult.Message;
        //    }
        //    catch (Exception)
        //    {
        //        result.Success = false;
        //    }
        //    return Json(result);
        //}

        //[HttpPost]
        //public IActionResult MergeVehicle(VehicleVM vehicleVM)
        //{
        //    var result = new Result();
        //    try
        //    {
        //        var mergeVehicleResult = ServiceFactory.BusManagement.MergeVehicle(vehicleVM);
        //        result.Success = mergeVehicleResult.Success;
        //        result.Message = mergeVehicleResult.Message;
        //    }
        //    catch (Exception)
        //    {
        //        result.Success = false;
        //    }
        //    return Json(result);
        //}

    }
}