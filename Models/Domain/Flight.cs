using System.Numerics;
using System.Runtime.Serialization;

namespace flights.Models.Domain
{
    public class Flight
    {
        public List<Itinerary> itineraries { get; set; }

        public Price price { get; set; }

        public List<TravelerPricings> travelerPricings { get; set; }
    }
}
