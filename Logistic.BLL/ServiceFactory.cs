using Logistic.BLL.Services;

namespace Logistic.BLL
{
    public class ServiceFactory : AnonymousServiceFactory
    {
        private int? _accountId;

        public override int? AccountId
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
                TomTom.AccountId = value;
            }
        }

        public ServiceFactory(string connection) : base(connection) { }
    }
}
