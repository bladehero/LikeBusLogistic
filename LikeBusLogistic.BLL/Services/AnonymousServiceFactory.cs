using LikeBusLogistic.BLL.Services.TomTom;
using System;

namespace LikeBusLogistic.BLL.Services
{
    public class AnonymousServiceFactory : IDisposable
    {
        public virtual int? AccountId { get => null; set { } }
        public AccountManagementService AccountManagement { get; set; }
        public BusManagementService BusManagement { get; set; }
        public DriverManagementService DriverManagement { get; set; }
        public GeolocationService GeolocationManagement { get; set; }
        public RouteManagementService RouteManagement { get; set; }
        public ScheduleManagementService ScheduleManagement { get; set; }
        public TripManagementService TripManagement { get; set; }
        public TomTomService TomTom { get; set; }

        public AnonymousServiceFactory(string connection)
        {
            AccountManagement = new AccountManagementService(connection);
            BusManagement = new BusManagementService(connection);
            DriverManagement = new DriverManagementService(connection);
            GeolocationManagement = new GeolocationService(connection);
            RouteManagement = new RouteManagementService(connection);
            ScheduleManagement = new ScheduleManagementService(connection, RouteManagement);
            TripManagement = new TripManagementService(connection);
            TomTom = new TomTomService(connection);
        }

        public void Dispose()
        {
            AccountManagement.Dispose();
            BusManagement.Dispose();
            DriverManagement.Dispose();
            GeolocationManagement.Dispose();
            RouteManagement.Dispose();
            ScheduleManagement.Dispose();
            TripManagement.Dispose();
            TomTom.Dispose();
        }
    }
}
