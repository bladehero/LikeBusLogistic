using System.Collections.Generic;

namespace LikeBusLogistic.Web.Models.Schedule
{
    public class FullInformationVM
    {
        public IEnumerable<VM.ViewModels.ScheduleVM> Schedules { get; set; }
        public ScheduleTab Tab { get; set; }

        public FullInformationVM()
        {
            Schedules = new List<VM.ViewModels.ScheduleVM>();
        }
    }
}
