using System.Collections.Generic;
using Newtonsoft.Json;

namespace Logistic.BLL.Services.TomTom.Models.ReverseGeocoding
{
    public class ReverseGeocodeResult
    {
        [JsonProperty("summary")]
        public Summary Summary { get; set; }

        [JsonProperty("addresses")]
        public List<AddressElement> Addresses { get; set; }
    }

    public class AddressElement
    {
        [JsonProperty("address")]
        public AddressAddress Address { get; set; }

        [JsonProperty("position")]
        public string Position { get; set; }
    }

    public partial class AddressAddress
    {
        [JsonProperty("buildingNumber")]
        public long BuildingNumber { get; set; }

        [JsonProperty("streetNumber")]
        public long StreetNumber { get; set; }

        [JsonProperty("routeNumbers")]
        public List<object> RouteNumbers { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("streetName")]
        public string StreetName { get; set; }

        [JsonProperty("streetNameAndNumber")]
        public string StreetNameAndNumber { get; set; }

        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }

        [JsonProperty("countrySubdivision")]
        public string CountrySubdivision { get; set; }

        [JsonProperty("countrySecondarySubdivision")]
        public string CountrySecondarySubdivision { get; set; }

        [JsonProperty("municipality")]
        public string Municipality { get; set; }

        [JsonProperty("municipalitySubdivision")]
        public string MunicipalitySubdivision { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("countryCodeISO3")]
        public string CountryCodeIso3 { get; set; }

        [JsonProperty("freeformAddress")]
        public string FreeformAddress { get; set; }

        [JsonProperty("boundingBox")]
        public BoundingBox BoundingBox { get; set; }

        [JsonProperty("localName")]
        public string LocalName { get; set; }
    }

    public class BoundingBox
    {
        [JsonProperty("northEast")]
        public string NorthEast { get; set; }

        [JsonProperty("southWest")]
        public string SouthWest { get; set; }

        [JsonProperty("entity")]
        public string Entity { get; set; }
    }

    public class Summary
    {
        [JsonProperty("queryTime")]
        public long QueryTime { get; set; }

        [JsonProperty("numResults")]
        public long NumResults { get; set; }
    }
}
