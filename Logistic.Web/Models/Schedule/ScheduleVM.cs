﻿using System.Collections.Generic;

namespace Logistic.Web.Models.Schedule
{
    public class ScheduleVM
    {
        public IEnumerable<VM.ViewModels.ScheduleVM> Schedules { get; set; }

        public ScheduleVM()
        {
            Schedules = new List<VM.ViewModels.ScheduleVM>();
        }
    }
}
