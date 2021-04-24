namespace Logistic.VM.ViewModels.TomTom
{
    public struct LocationPoint
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public override string ToString()
        {
            return $"{Latitude},{Longitude}";
        }
    }
}
