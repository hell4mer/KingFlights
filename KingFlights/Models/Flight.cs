using System;

namespace KingFlights
{
    public class Flight
    {
        public string Id { get; set; }

        public string OriginLocationCode { get; set; }
        public string DestinationLocationCode { get; set; }

        public DateTime DepartureDate { get; set; }
        public DateTime ReturnDate { get; set; }

        public int Transfers { get; set; }
        public int Passengers { get; set; }

        public double Price { get; set; }

        public string Currency { get; set; }

        public Flight()
        {
            Id = Guid.NewGuid().ToString();
        }

        public Flight(string originLocationCode, string destinationLocationCode, DateTime departureDate, DateTime returnDate, 
            int transfers = 0, int passengers = 0, double price = 0, string currency = "EUR")
        {
            OriginLocationCode = originLocationCode;
            DestinationLocationCode = destinationLocationCode;
            DepartureDate = departureDate;
            ReturnDate = returnDate;
            Transfers = transfers;
            Passengers = passengers;
            Price = price;
            Currency = currency;
        }
    }
}
