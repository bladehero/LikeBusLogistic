using System;
namespace LikeBusLogistic.DAL.Models
{
    public class Trip
    {
        public int BusId { get; set; }
        public int RouteId { get; set; }
        public DateTime Departure { get; set; }
        public DateTime Arrival { get; set; }
    }
}