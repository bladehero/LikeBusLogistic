﻿using System;
namespace Logistic.DAL.Models
{
    public class Trip : UserTrackedEntity
    {
        public int ScheduleId { get; set; }
        public DateTime Departure { get; set; }
        public string Status { get; set; }
        public string Color { get; set; }
    }
}