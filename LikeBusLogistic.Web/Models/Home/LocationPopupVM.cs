namespace LikeBusLogistic.Web.Models.Home
{
    public class LocationPopupVM
    {
        public VM.ViewModels.LocationVM Location { get; set; }
        public VM.ViewModels.RouteVM Route { get; set; }
        public VM.ViewModels.RouteLocationVM RouteLocation { get; set; }
        public bool IsRoute { get; set; }
    }
}
