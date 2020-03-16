using LikeBusLogistic.BLL.Services;
using System;

namespace LikeBusLogistic.BLL
{
    public class ServiceFactory : IDisposable
    {
        private int? _accountId;

        public int? AccountId
        {
            get => _accountId;
            set
            {
                _accountId = value;
                AccountManagement.AccountId = value;
                BusManagement.AccountId = value;
                DriverManagement.AccountId = value;
                GeolocationManagement.AccountId = value;
                RouteManagement.AccountId = value;
                ScheduleManagement.AccountId = value;
                TripManagement.AccountId = value;
            }
        }
        public AccountManagementService AccountManagement { get; set; }
        public BusManagementService BusManagement { get; set; }
        public DriverManagementService DriverManagement { get; set; }
        public GeolocationService GeolocationManagement { get; set; }
        public RouteManagementService RouteManagement { get; set; }
        public ScheduleManagementService ScheduleManagement { get; set; }
        public TripManagementService TripManagement { get; set; }

        public ServiceFactory(string connection)
        {
            AccountManagement = new AccountManagementService(connection);
            BusManagement = new BusManagementService(connection);
            DriverManagement = new DriverManagementService(connection);
            GeolocationManagement = new GeolocationService(connection);
            RouteManagement = new RouteManagementService(connection);
            ScheduleManagement = new ScheduleManagementService(connection, RouteManagement);
            TripManagement = new TripManagementService(connection);
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
        }
    }
}
