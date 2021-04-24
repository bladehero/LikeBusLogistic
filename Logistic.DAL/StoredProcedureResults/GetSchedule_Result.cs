using System;

namespace Logistic.DAL.StoredProcedureResults
{
    public class GetSchedule_Result
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RouteId { get; set; }
        public bool NeedsSync { get; set; }
        public bool HasConfirmedTrips { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public string RouteName { get; set; }
        public bool IsDeleted { get; set; }
    }
}
