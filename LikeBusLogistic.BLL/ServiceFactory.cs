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
            }
        }
        public AccountManagementService AccountManagement { get; set; }
        public BusManagementService BusManagement { get; set; }

        public ServiceFactory(string connection)
        {
            AccountManagement = new AccountManagementService(connection);
            BusManagement = new BusManagementService(connection);
        }

        public void Dispose()
        {
            AccountManagement.Dispose();
            BusManagement.Dispose();
        }
    }
}
