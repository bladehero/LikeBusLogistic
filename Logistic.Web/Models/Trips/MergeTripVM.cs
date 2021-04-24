using Logistic.VM.ViewModels;
using System;
using System.Collections.Generic;

namespace Logistic.Web.Models.Trips
{
    public class MergeTripVM
    {
        public int? Id { get; set; }
        public DateTime Departure { get; set; }
        public string Color { get; set; }
        public string Status { get; set; }
        public bool IsEditable { get; set; }

        public int? ScheduleId { get; set; }
        public ScheduleVM SelectedSchedule { get; set; }
        public IEnumerable<ScheduleVM> Schedules { get; set; }

        public int? BusId { get; set; }
        public BusVM SelectedBus { get; set; }
        public IEnumerable<BusVM> Buses { get; set; }

        public List<int> DriverIds { get; set; }
        public IEnumerable<DriverInfoVM> SelectedDrivers { get; set; }
        public IEnumerable<DriverInfoVM> Drivers { get; set; }

        public MergeTripVM()
        {
            Schedules = new List<ScheduleVM>();
            Buses = new List<BusVM>();
            Drivers = new List<DriverInfoVM>();
            SelectedDrivers = new List<DriverInfoVM>();
        }
    }
}
