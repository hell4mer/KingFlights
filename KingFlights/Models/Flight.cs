using System;

namespace KingFlights
{
    public class Flight
    {
        public string Id { get; set; }

        public string OriginLocationCode { get; set; }
        public string DestinationLocationCode { get; set; }

        public DateTime DepartureDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public int Transfers { get; set; }
        public int Passengers { get; set; }

        public double Price { get; set; }

        public string CurrencyCode { get; set; }

        public Flight()
        {
            
        }

        public Flight(string id, string originLocationCode, string destinationLocationCode, DateTime departureDate, DateTime? returnDate, 
            int transfers = 0, int passengers = 0, double price = 0, string currencyCode = "EUR")
        {
            Id = id;
            OriginLocationCode = originLocationCode;
            DestinationLocationCode = destinationLocationCode;
            DepartureDate = departureDate;
            ReturnDate = returnDate;
            Transfers = transfers;
            Passengers = passengers;
            Price = price;
            CurrencyCode = currencyCode;
        }
    }

    public class FlightSearch
    {
        public int Id { get; set; }
        public string SearchParams { get; set; }

        public FlightSearch()
        {

        }

        public FlightSearch(string srcParams)
        {
            SearchParams = srcParams;
        }
    }

    public class FlightSearchResult
    {
        public int Id { get; set; }
        public int SearchId { get; set; }
        public int Transfers { get; set; }
        public int Passengers { get; set; }
        public double Price { get; set; }

        public FlightSearchResult()
        {

        }

        public FlightSearchResult(int searchId, int transfers, int passengers, double price)
        {
            SearchId = searchId;
            Transfers = transfers;
            Passengers = passengers;
            Price = price;
        }
    }

    public class Airport
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string IATA { get; set; }
    }

    public class AirportAutoComplete
    {
        public string ValueData { get; set; }
        public string DisplayData { get; set; }

        public AirportAutoComplete()
        {

        }

        public AirportAutoComplete(string iata, string name, string city, string country)
        {
            ValueData = iata;
            DisplayData = $"{name} ({city}, {country}) [{iata}]";
        }
    }
}
