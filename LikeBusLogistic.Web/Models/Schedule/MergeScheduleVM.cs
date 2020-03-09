using LikeBusLogistic.VM.ViewModels;
using System.Collections.Generic;

namespace LikeBusLogistic.Web.Models.Schedule
{
    public class MergeScheduleVM
    {
        public VM.ViewModels.ScheduleVM Schedule { get; set; }
        public IEnumerable<RouteVM> Routes { get; set; }

        public MergeScheduleVM()
        {
            Routes = new List<RouteVM>();
        }
    }
}
