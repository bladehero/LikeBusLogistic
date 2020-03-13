using System;

namespace LikeBusLogistic.VM.ViewModels
{
    public class TripVM
    {
        public int Id { get; set; }

        /// <summary>
        /// Bus Info
        /// </summary>
        public int BusId { get; set; }
        public string Fullname { get; set; }
        public int PassengerCapacity { get; set; }
        public string Number { get; set; }
        public int CrewCapacity { get; set; }

        /// <summary>
        /// Location Bus Info
        /// </summary>
        public int? CurrentLocationId { get; set; }
        public string FullName { get; set; }
        public string Name { get; set; }
        public double LocationLatitude { get; set; }
        public double LocationLongtitude { get; set; }
        public int? CityId { get; set; }
        public string CityName { get; set; }
        public int? DistrictId { get; set; }
        public string DistrictName { get; set; }
        public int? CountryId { get; set; }
        public string CountryName { get; set; }
        public bool IsRepair { get; set; }
        public bool IsParking { get; set; }

        /// <summary>
        /// Bus Coordinates
        /// </summary>
        public double CoordinateLatitude { get; set; }
        public double CoordinateLongtitude { get; set; }

        /// <summary>
        /// Schedule Info
        /// </summary>
        public int ScheduleId { get; set; }
        public string ScheduleName { get; set; }
        public string RouteName { get; set; }

        public DateTime Departure { get; set; }
        public DateTime Arrival { get; set; }

        public bool IsStarted { get; set; }

        public bool IsDeleted { get; set; }
    }
}
