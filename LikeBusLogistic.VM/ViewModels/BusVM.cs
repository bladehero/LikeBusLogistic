using System;

namespace LikeBusLogistic.VM.ViewModels
{
    public class BusVM
    {
        public int BusId { get; set; }
        public int VehicleId { get; set; }
        public string Fullname { get; set; }
        public int PassengerCapacity { get; set; }
        public string Number { get; set; }
        public int CrewCapacity { get; set; }
    }
}
