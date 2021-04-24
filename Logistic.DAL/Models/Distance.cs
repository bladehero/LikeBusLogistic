namespace Logistic.DAL.Models
{
    public class Distance : UserTrackedEntity
    {
        public int Location1 { get; set; }
        public int Location2 { get; set; }
        public string TomTomInfo { get; set; }
    }
}