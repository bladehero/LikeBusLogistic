namespace Logistic.DAL.Models
{
    public class BusDriver : UserTrackedEntity
    {
        public int BusId { get; set; }
        public int DriverId { get; set; }
    }
}