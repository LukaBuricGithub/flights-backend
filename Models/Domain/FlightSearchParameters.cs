namespace flights.Models.Domain
{
    public class FlightSearchParameters
    {
        string originLocationCode { get; set; }
        string destinationLocationCode { get; set; }
        string departureDate {  get; set; } 
        string currencyCode { get; set; }
        string passengers {  get; set; }
        string returnDate {  get; set; }

        public FlightSearchParameters() 
        { 
        
        }


        public FlightSearchParameters(string originLocationCode, string destinationLocationCode, string departureDate, string currencyCode, string passengers, string returnDate)
        {
            this.originLocationCode = originLocationCode;
            this.destinationLocationCode = destinationLocationCode;
            this.departureDate = departureDate;
            this.currencyCode = currencyCode;
            this.passengers = passengers;
            this.returnDate = returnDate;
        }


        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            FlightSearchParameters other = (FlightSearchParameters)obj;
            return originLocationCode == other.originLocationCode &&
                   destinationLocationCode == other.destinationLocationCode &&
                   departureDate == other.departureDate &&
                   currencyCode == other.currencyCode &&
                   passengers == other.passengers &&
                   returnDate == other.returnDate;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + originLocationCode.GetHashCode();
                hash = hash * 23 + destinationLocationCode.GetHashCode();
                hash = hash * 23 + departureDate.GetHashCode();
                hash = hash * 23 + currencyCode.GetHashCode();
                hash = hash * 23 + passengers.GetHashCode();
                hash = hash * 23 + returnDate.GetHashCode();
                return hash;
            }
        }


    }
}
