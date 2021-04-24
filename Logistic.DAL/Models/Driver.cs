namespace Logistic.DAL.Models
{
    public class Driver : UserTrackedEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
    }
}