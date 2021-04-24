using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Logistic.BLL.Services.TomTom.Models.Routing
{

    public partial class CalculateRouteResult
    {
        [JsonProperty("formatVersion")]
        public string FormatVersion { get; set; }

        [JsonProperty("routes")]
        public List<Route> Routes { get; set; }
    }

    public partial class Route
    {
        [JsonProperty("summary")]
        public Summary Summary { get; set; }

        [JsonProperty("legs")]
        public List<Leg> Legs { get; set; }

        [JsonProperty("sections")]
        public List<Section> Sections { get; set; }
    }

    public partial class Leg
    {
        [JsonProperty("summary")]
        public Summary Summary { get; set; }

        [JsonProperty("points")]
        public List<Point> Points { get; set; }
    }

    public partial class Point
    {
        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }
    }

    public partial class Summary
    {
        [JsonProperty("lengthInMeters")]
        public long LengthInMeters { get; set; }

        [JsonProperty("travelTimeInSeconds")]
        public long TravelTimeInSeconds { get; set; }

        [JsonProperty("trafficDelayInSeconds")]
        public long TrafficDelayInSeconds { get; set; }

        [JsonProperty("departureTime")]
        public DateTimeOffset DepartureTime { get; set; }

        [JsonProperty("arrivalTime")]
        public DateTimeOffset ArrivalTime { get; set; }
    }

    public partial class Section
    {
        [JsonProperty("startPointIndex")]
        public long StartPointIndex { get; set; }

        [JsonProperty("endPointIndex")]
        public long EndPointIndex { get; set; }

        [JsonProperty("sectionType")]
        public string SectionType { get; set; }

        [JsonProperty("travelMode")]
        public string TravelMode { get; set; }
    }
}
