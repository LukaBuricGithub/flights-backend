namespace flights.Models.Domain
{
    public class Itinerary
    {
        public string duration { get; set; }
        public List<Segment> segments { get; set; }

    }
}
