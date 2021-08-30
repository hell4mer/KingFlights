using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KingFlights.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FlightsController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public FlightsController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;

            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("hr");
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("hr");
        }

        [HttpGet]
        [Route("~/Flights/Get")]
        public IEnumerable<Flight> Get(string currency = "EUR")
        {
            var flights = new List<Flight>();
            flights.Add(new Flight("ZAG", "LJU", new DateTime(2021, 9, 1), new DateTime(2021, 9, 5), 0, 300, 100));
            flights.Add(new Flight("BEG", "MUC", new DateTime(2021, 9, 2), new DateTime(2021, 9, 4), 1, 200, 120, "HRK"));
            flights.Add(new Flight("FRA", "ZAG", new DateTime(2021, 9, 3), new DateTime(2021, 9, 6), 0, 300, 180));
            flights.Add(new Flight("PUY", "GRZ", new DateTime(2021, 9, 2), new DateTime(2021, 9, 9), 1, 200, 300, "USD"));
            flights.Add(new Flight("BUD", "WAW", new DateTime(2021, 9, 1), new DateTime(2021, 9, 8), 0, 300, 150));
            return flights.Where(p => p.CurrencyCode == currency);
        }

        [HttpGet]
        [Route("~/Flights/GetCheapFlights")]
        public async Task<IEnumerable<Flight>> GetCheapFlights([FromServices] AmadeusAPI api, string originLocationCode, string destinationLocationCode, DateTime departureDate, DateTime? returnDate, int passengers, string currencyCode)
        {
            var flights = new List<Flight>();
            if (!string.IsNullOrEmpty(originLocationCode))
            {
                await api.ConnectOAuth();
                var results = await api.GetCheapFlightOffers(originLocationCode, destinationLocationCode, departureDate, returnDate, passengers, currencyCode);
                if (results.data != null)
                {
                    results.data.ForEach(delegate (FlightOffer flightOffer)
                    {
                        flights.Add(new Flight(originLocationCode, destinationLocationCode, departureDate, returnDate, flightOffer.itineraries.Count, flightOffer.numberOfBookableSeats, double.Parse(flightOffer.price.total, CultureInfo.InvariantCulture), flightOffer.price.currency));
                    });
                }
            }
            return flights;
        }
    }
}
