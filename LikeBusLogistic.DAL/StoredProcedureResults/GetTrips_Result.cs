using System;

namespace LikeBusLogistic.DAL.StoredProcedureResults
{
    public class GetTrips_Result
    {
        public int Id { get; set; }
        public DateTime Departure { get; set; }
        public string Status { get; set; }
        public string Color { get; set; }
        public int ScheduleId { get; set; }
        public string ScheduleName { get; set; }
        public int RouteId { get; set; }
        public string RouteName { get; set; }
        public int BusId { get; set; }
        public int BusCrewCapacity { get; set; }
        public string BusNumber { get; set; }
        public int VehicleId { get; set; }
        public string VehicleModel { get; set; }
        public int VehiclePassengerCapacity { get; set; }
        public string VehicleProducer { get; set; }
        public double TotalDistance { get; set; }
        public bool IsDeleted { get; set; }
    }
}
