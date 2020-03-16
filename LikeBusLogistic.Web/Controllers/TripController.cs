using LikeBusLogistic.BLL;
using LikeBusLogistic.Web.Models.Trips;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LikeBusLogistic.Web.Controllers
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
            var schedule = ServiceFactory.ScheduleManagement.GetSchedule(trip?.ScheduleId);
            var bus = ServiceFactory.BusManagement.GetBus(trip?.BusId);
            var drivers = ServiceFactory.TripManagement.GetLastDriverInfoForTrip(tripId);


            var model = new MergeTripVM
            {

            };
            return PartialView(model);
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
                case TripTab.AllTrips:
                default:
                    status = null;
                    break;
            }
            return status;
        }
    }
}