using LikeBusLogistic.BLL;
using LikeBusLogistic.VM.ViewModels;
using LikeBusLogistic.Web.Models;
using LikeBusLogistic.Web.Models.Drivers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

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

        [HttpPost]
        public IActionResult MergeDriver(DriverInfoVM driverInfoVM)
        {
            var result = new Result();
            try
            {
                var mergeDriverResult = ServiceFactory.DriverManagement.MergeDriver(driverInfoVM);
                result.Success = mergeDriverResult.Success;
                result.Message = mergeDriverResult.Message;
            }
            catch (Exception)
            {
                result.Success = false;
            }
            return Json(result);
        }

        [HttpPost]
        public IActionResult MergeContact(DriverContactVM driverContactVM)
        {
            var result = new Result();
            try
            {
                var mergeContactResult = ServiceFactory.DriverManagement.MergeDriverContact(driverContactVM);
                result.Success = mergeContactResult.Success;
                result.Message = mergeContactResult.Message;
            }
            catch (Exception)
            {
                result.Success = false;
            }
            return Json(result);
        }

    }
}