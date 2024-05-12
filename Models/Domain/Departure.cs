using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace flights.Models.Domain
{
    public class Departure
    {
        public string iataCode { get; set; }
        public string at {  get; set; }
    }
}
