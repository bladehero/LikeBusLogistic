﻿namespace Logistic.DAL.Models
{
    public class Bus : UserTrackedEntity
    {
        public int? VehicleId { get; set; }
        public string Number { get; set; }
        public int CrewCapacity { get; set; }
    }
}