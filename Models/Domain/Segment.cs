using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace flights.Models.Domain
{
    public class Segment
    {

        public string carrierCode { get; set; }
        public Departure departure { get; set; }
        public Arrival arrival { get; set; }
    }
}
