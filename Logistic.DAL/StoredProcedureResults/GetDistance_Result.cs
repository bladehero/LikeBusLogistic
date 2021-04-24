namespace Logistic.DAL.StoredProcedureResults
{
    public class GetDistance_Result
    {
        public int Id { get; set; }
        public string TomTomInfo { get; set; }

        public int Location1 { get; set; }
        public string FirstLocationFullName { get; set; }
        public string FirstLocationName { get; set; }
        public double FirstLocationLatitude { get; set; }
        public double FirstLocationLongitude { get; set; }
        public bool FirstLocationIsCarRepair { get; set; }
        public bool FirstLocationIsParking { get; set; }

        public int Location2 { get; set; }
        public string SecondLocationFullName { get; set; }
        public string SecondLocationName { get; set; }
        public double SecondLocationLatitude { get; set; }
        public double SecondLocationLongitude { get; set; }
        public bool SecondLocationIsCarRepair { get; set; }
        public bool SecondLocationIsParking { get; set; }

        public bool IsDeleted { get; set; }
    }
}
