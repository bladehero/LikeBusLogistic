﻿namespace LikeBusLogistic.DAL.StoredProcedureResults
{
    public class GetDriverInfo_Result
    {
        public int BusId { get; set; }
        public string BusInfo { get; set; }
        public int DriverId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
    }
}
