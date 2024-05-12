using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

using amadeus;
using amadeus.resources;
using flights.Models.Domain;
using Newtonsoft.Json;
using resources.referenceData;
using System.Diagnostics.Eventing.Reader;

namespace flights.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class FlightController : ControllerBase
    {


        private static Dictionary<FlightSearchParameters, List<Models.DTO.Flight>> flightCache = new Dictionary<FlightSearchParameters, List<Models.DTO.Flight>>();


        private readonly ILogger<FlightController> _logger;

        public FlightController(ILogger<FlightController> logger)
        {
            _logger = logger;
        }



        [HttpGet("GetIATACode")]
        public IEnumerable<Models.Domain.Location> GetIATACode(string location)
        {
            Amadeus amadeus = Amadeus.builder("G89UDVBA4otrdnJ1AmoZNsFWy6RKEtsm", "QCrDZ3ioLyKGm0M4").build();

            amadeus.resources.Location[] locationsAmadeus = amadeus.referenceData.locations.get(Params
                .with("keyword", location.ToUpper())
                .and("subType", Locations.ANY));


            if(locationsAmadeus[0].response.statusCode != 200)
            {
                return Enumerable.Empty<Models.Domain.Location>();
            }

            /*Console.Write(locations[0].response.body); //the raw response, as a string */

            var aquiredLocations = JsonConvert.DeserializeObject<LocationList>(locationsAmadeus[0].response.body);

            List<Models.Domain.Location> locations = new List<Models.Domain.Location>();

            foreach (var loc in aquiredLocations.Data)
            {
                locations.Add(loc);
            }
            return locations.ToArray();
        }


        [HttpGet("GetFlightOffers")]
        public IEnumerable<Models.DTO.Flight> GetFlightOffers(string originLocationCode, string destinationLocationCode, string departureDate, string currencyCode,string passengers, string returnDate="")
        {

            FlightSearchParameters searchParams = new FlightSearchParameters(originLocationCode, destinationLocationCode, departureDate, currencyCode, passengers, returnDate);

            if (flightCache.ContainsKey(searchParams))
            {
                _logger.LogInformation("Flight offers found in cache.");
                return flightCache[searchParams].ToArray();
            }


            Amadeus amadeus = Amadeus.builder("G89UDVBA4otrdnJ1AmoZNsFWy6RKEtsm", "QCrDZ3ioLyKGm0M4").build();


            List<Models.DTO.Flight> flightOffers = new List<Models.DTO.Flight>();

            if (returnDate == "")
            {
                Response res = amadeus.get("/v2/shopping/flight-offers", Params
                .with("originLocationCode", originLocationCode)
                .and("destinationLocationCode", destinationLocationCode)
                .and("departureDate", departureDate)
                .and("adults", passengers)
                .and("currencyCode", currencyCode)
                .and("max", "20"));

                if (res.statusCode != 200)
                {
                    return Enumerable.Empty<Models.DTO.Flight>();
                }

                var aquiredFlightOffers = JsonConvert.DeserializeObject<FlightList>(res.body);

                //List<Models.DTO.Flight> flightOffers = new List<Models.DTO.Flight>();

                foreach (var flightOffer in aquiredFlightOffers.Data)
                {
                    //Departure transfers
                    var departureTransfersUnformated = flightOffer.itineraries[0].segments.Count - 1;
                    string departureTransfers = departureTransfersUnformated.ToString();

                    //Price and currency
                    string price = flightOffer.price.total;
                    string currency = flightOffer.price.currency;           

                    Models.DTO.Flight flight = new Models.DTO.Flight(originLocationCode, destinationLocationCode, departureDate, currencyCode, passengers, returnDate,price,departureTransfers,"");
                    flightOffers.Add(flight);

                    //Console.WriteLine($"Number of transfers: {flightOffer.itineraries[0].segments.Count - 1}");
                    //var segmentsSize = flightOffer.itineraries[0].segments.Count - 1;

                    //Console.WriteLine($"Departure: {flightOffer.itineraries[0].segments[0].departure.iataCode}");
                    //var dateDepartureUnformated = flightOffer.itineraries[0].segments[0].departure.at;
                    //int indexOfTDeparture = dateDepartureUnformated.IndexOf('T');
                    //string dateDeparture = dateDepartureUnformated.Substring(0, indexOfTDeparture);
                    //Console.WriteLine($"Date of departure: {dateDeparture}");


                    //Console.WriteLine($"Arrival: {flightOffer.itineraries[0].segments[flightOffer.itineraries[0].segments.Count - 1].arrival.iataCode}");
                    //var dateArrivalUnformed = flightOffer.itineraries[0].segments[0].arrival.at;
                    //int indexOfArrival = dateArrivalUnformed.IndexOf('T');
                    //string dateArrival = dateArrivalUnformed.Substring(0, indexOfArrival);
                    //Console.WriteLine($"Date of arrival: {dateArrival}");

                    //Console.WriteLine($"Total cost of travel: {flightOffer.price.total} {flightOffer.price.currency}");
                    //Console.WriteLine($"Number of passengers: {flightOffer.travelerPricings.Count}");


                    //foreach (var segment in flightOffer.itineraries[0].segments)
                    //{
                        //Console.WriteLine("Departure:");
                        //Console.WriteLine(segment.departure.iataCode);
                        //Console.WriteLine(segment.departure.at);
                        //Console.WriteLine("Arrival:");
                        //Console.WriteLine(segment.arrival.iataCode);
                        //Console.WriteLine(segment.arrival.at);
                    //}
                    //Console.WriteLine();

                }


            }

            else if (returnDate != "")
            {
                Response res = amadeus.get("/v2/shopping/flight-offers", Params
                    .with("originLocationCode", originLocationCode)
                    .and("destinationLocationCode", destinationLocationCode)
                    .and("departureDate", departureDate)
                    .and("returnDate", returnDate)
                    .and("adults", passengers)
                    .and("currencyCode", currencyCode)
                    .and("max","20"));

                if (res.statusCode != 200)
                {
                    return Enumerable.Empty<Models.DTO.Flight>();
                }

                var aquiredFlightOffers = JsonConvert.DeserializeObject<FlightList>(res.body);

                //List<Models.DTO.Flight> flightOffers = new List<Models.DTO.Flight>();

                foreach (var flightOffer in aquiredFlightOffers.Data)
                {
                    //Departure transfers
                    var departureTransfersUnformated = flightOffer.itineraries[0].segments.Count - 1;
                    string departureTransfers = departureTransfersUnformated.ToString();

                    //Return transfers
                    var returnTransfersUnformated = flightOffer.itineraries[1].segments.Count - 1;
                    string returnTransfers = departureTransfersUnformated.ToString();


                    //Price and currency
                    string price = flightOffer.price.total;
                    string currency = flightOffer.price.currency;



                    Models.DTO.Flight flight = new Models.DTO.Flight(originLocationCode, destinationLocationCode, departureDate, currencyCode, passengers, returnDate, price, departureTransfers, returnTransfers);
                    flightOffers.Add(flight);
                }

            }


            flightCache[searchParams] = flightOffers;


            foreach (var offer in flightOffers)
            {
                Console.WriteLine($"Departure: {offer.departureCode}");
                Console.WriteLine($"Arrival: {offer.arrivalCode}");
                Console.WriteLine($"Departure date: {offer.departureDate}");
                Console.WriteLine($"Return date: {offer.returnDate}");
                Console.WriteLine($"Departure transfers: {offer.departureTransfers}");
                Console.WriteLine($"Return transfers: {offer.returnTransfers}");
                Console.WriteLine($"Passengers: {offer.numberOfPassengers}");
                Console.WriteLine($"Price: {offer.price}");
                Console.WriteLine($"Currency: {offer.currency}");
                Console.WriteLine();
            }

            return flightOffers.ToArray();

            //return Enumerable.Empty<Flight>();
        }
    }

} 
