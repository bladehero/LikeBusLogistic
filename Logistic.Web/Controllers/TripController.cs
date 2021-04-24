using Logistic.BLL;
using Logistic.VM.ViewModels;
using Logistic.Web.Models;
using Logistic.Web.Models.Trips;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logistic.Web.Controllers
{
    [Authorize]
    public class TripController : BaseController
    {
        public TripController(ServiceFactory serviceFactory) : base(serviceFactory) { }

        [HttpGet]
        public IActionResult _FullInformation(TripTab tab = TripTab.StartedTrips)
        {
            var model = new FullInformationVM
            {
                Tab = tab
            };
            return PartialView(model);
        }

        [HttpGet]
        public IActionResult _Trip(TripTab tab = TripTab.StartedTrips)
        {
            var status = GetTripStatus(tab);
            var trips = ServiceFactory.TripManagement.GetTrips(status?.ToString()).Data;

            var model = new TripsVM
            {
                Trips = trips,
                Tab = tab
            };
            return PartialView(model);
        }

        [HttpGet]
        public IActionResult _MergeTrip(int? tripId)
        {
            var trip = ServiceFactory.TripManagement.GetTrip(tripId).Data;
            var selectedSchedule = ServiceFactory.ScheduleManagement.GetSchedule(trip?.ScheduleId).Data;
            var selectedBus = ServiceFactory.TripManagement.GetLastBusForTrip(tripId).Data;

            var schedules = ServiceFactory.ScheduleManagement.GetSchedules(false).Data;
            var buses = ServiceFactory.BusManagement.GetBuses(false).Data;

            var model = new MergeTripVM
            {
                Id = trip?.Id,
                Departure = (trip?.Departure).GetValueOrDefault(),
                Color = trip?.Color,
                Status = trip?.Status ?? TripStatus.P.ToString(),
                IsEditable = trip?.Status != TripStatus.S.ToString()
                && trip?.Status != TripStatus.D.ToString(),

                Buses = buses,
                Schedules = schedules,

                SelectedBus = selectedBus,
                SelectedSchedule = selectedSchedule
            };
            return PartialView(model);
        }

        [HttpGet]
        public IActionResult _TripDrivers(int? tripId, int? busId)
        {
            if (!tripId.HasValue && !busId.HasValue)
            {
                return Content("");
            }

            IEnumerable<DriverInfoVM> drivers;
            if (busId.HasValue)
            {
                drivers = ServiceFactory.DriverManagement.GetDriversOrderByBus(busId, false).Data;
            }
            else
            {
                drivers = ServiceFactory.DriverManagement.GetDrivers(false).Data;
            }

            var trip = ServiceFactory.TripManagement.GetTrip(tripId).Data;
            var selectedDrivers = ServiceFactory.TripManagement.GetLastDriverInfoForTrip(tripId).Data;
            var driversAmount = selectedDrivers?.Count() ?? 0;
            if (busId.HasValue && driversAmount == 0)
            {
                var bus = ServiceFactory.BusManagement.GetBus(busId.Value).Data;
                driversAmount = bus.CrewCapacity;
            }

            var model = new TripDriversVM
            {
                DriversAmount = driversAmount,
                Drivers = drivers,
                SelectedDrivers = selectedDrivers,
                IsEditable = trip?.Status != TripStatus.S.ToString()
                && trip?.Status != TripStatus.D.ToString()
            };
            return PartialView(model);
        }

        [HttpPost]
        public IActionResult MergeTrip(TripVM trip, IEnumerable<int> drivers)
        {
            var result = new Result();
            try
            {
                var oldTrip = ServiceFactory.TripManagement.GetTrip(trip?.Id).Data;
                trip.Status = oldTrip?.Status ?? TripStatus.P.ToString();
                var mergeTripResult = ServiceFactory.TripManagement.MergeTrip(trip,
                    drivers.Select(x => new DriverInfoVM { DriverId = x }));
                result.Success = mergeTripResult.Success;
                result.Message = mergeTripResult.Message;
            }
            catch (Exception)
            {
                result.Success = false;
            }
            return Json(result);
        }

        [HttpPost]
        public IActionResult DeleteOrRestoreTrip(int tripId)
        {
            var result = new Result();
            try
            {
                var deleteOrRestoreTripResult =
                    ServiceFactory.TripManagement.DeleteOrRestoreTrip(tripId);
                var trip = ServiceFactory.TripManagement.GetTrip(tripId).Data;
                if (trip.IsDeleted)
                {
                    var changeStatusTripResult = ServiceFactory.TripManagement.ChangeStatusTrip(tripId, TripStatus.C);
                }
                result.Success = deleteOrRestoreTripResult.Success;
                result.Message = deleteOrRestoreTripResult.Message;
            }
            catch (Exception)
            {
                result.Success = false;
            }
            return Json(result);
        }

        private TripStatus? GetTripStatus(TripTab tab)
        {
            TripStatus? status;
            switch (tab)
            {
                case TripTab.PendingTrips:
                    status = TripStatus.P;
                    break;
                case TripTab.StartedTrips:
                    status = TripStatus.S;
                    break;
                case TripTab.DelayedTrips:
                    status = TripStatus.D;
                    break;
                case TripTab.FinishedTrips:
                    status = TripStatus.F;
                    break;
                case TripTab.CancelledTrips:
                    status = TripStatus.C;
                    break;
                case TripTab.AllTrips:
                default:
                    status = null;
                    break;
            }
            return status;
        }
    }
}