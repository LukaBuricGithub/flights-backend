namespace flights.Models.DTO
{
    public class Flight
    {
        public string departureCode { get; set; }
        public string arrivalCode { get; set; }
        public string numberOfPassengers { get; set; }
        public string currency { get; set; }
        public string price { get; set; }
        public string departureTransfers { get; set; }
        public string returnTransfers { get; set; }
        public string departureDate { get; set; }
        public string returnDate { get; set; }

        public Flight()
        {
        }



        public Flight(string departureCode, string arrivalCode, string departureDate, string currency, string numberOfPassengers, string returnDate, string price, string departureTransfers, string returnTransfers)
        {
            this.departureCode = departureCode;
            this.arrivalCode = arrivalCode;
            this.numberOfPassengers = numberOfPassengers;
            this.currency = currency;
            this.price = price;
            this.departureTransfers = departureTransfers;
            this.returnTransfers = returnTransfers;
            this.departureDate = departureDate;
            this.returnDate = returnDate;
        }


    }
}
