using LikeBusLogistic.DAL.Dao;
using System;
using System.Data;

namespace LikeBusLogistic.DAL
{
    public sealed class UnitOfWork : IDisposable
    {
        private IDbConnection _connection;

        public AccountDao AccountDao { get; set; }
        public BusDao BusDao { get; set; }
        public BusDriverDao BusDriverDao { get; set; }
        public CityDao CityDao { get; set; }
        public CountryDao CountryDao { get; set; }
        public DistrictDao DistrictDao { get; set; }
        public DriverDao DriverDao { get; set; }
        public DriverContactDao DriverContactDao { get; set; }
        public LocationDao LocationDao { get; set; }
        public RepairSpecialistDao RepairSpecialistDao { get; set; }
        public RoleDao RoleDao { get; set; }
        public RouteDao RouteDao { get; set; }
        public RouteLocationDao RouteLocationDao { get; set; }
        public TripDao TripDao { get; set; }
        public TripBusDriverDao TripBusDriverDao { get; set; }
        public UserDao UserDao { get; set; }
        public VehicleDao VehicleDao { get; set; }
        public StoredProcedureDao StoredProcedureDao { get; set; }


        public UnitOfWork(IDbConnection connection)
        {
            _connection = connection;

            AccountDao = new AccountDao(connection);
            BusDao = new BusDao(connection);
            BusDriverDao = new BusDriverDao(connection);
            CityDao = new CityDao(connection);
            CountryDao = new CountryDao(connection);
            DistrictDao = new DistrictDao(connection);
            DriverDao = new DriverDao(connection);
            DriverContactDao = new DriverContactDao(connection);
            LocationDao = new LocationDao(connection);
            RoleDao = new RoleDao(connection);
            RouteDao = new RouteDao(connection);
            RouteLocationDao = new RouteLocationDao(connection);
            TripDao = new TripDao(connection);
            TripBusDriverDao = new TripBusDriverDao(connection);
            UserDao = new UserDao(connection);
            VehicleDao = new VehicleDao(connection);

            StoredProcedureDao = new StoredProcedureDao(connection);
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}
