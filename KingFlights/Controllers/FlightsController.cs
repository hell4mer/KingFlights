using Microsoft.AspNetCore.Http;
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
        public async Task<IEnumerable<Flight>> Get([FromServices] AmadeusAPI api, string originLocationCode, string destinationLocationCode, DateTime departureDate, DateTime? returnDate, int passengers, string currencyCode)
        {
            var flights = new List<Flight>();

            if (!string.IsNullOrEmpty(originLocationCode) && !string.IsNullOrEmpty(destinationLocationCode) && departureDate != null)
            {
                var cachedResults = new CachedResults(HttpContext.Request.QueryString.ToString());

                if (cachedResults.HasCachedResults)
                    cachedResults.Results.ForEach(f=> flights.Add(new Flight(originLocationCode, destinationLocationCode, departureDate, returnDate, f.Transfers, f.Passengers, f.Price, currencyCode)));
                else
                {
                    await api.ConnectOAuth();
                    var results = await api.GetCheapFlightOffers(originLocationCode, destinationLocationCode, departureDate, returnDate, passengers, currencyCode);
                    if (results.data != null)
                    {
                        results.data.ForEach(f => flights.Add(new Flight(originLocationCode, destinationLocationCode, departureDate, returnDate, f.itineraries.Count, f.numberOfBookableSeats, double.Parse(f.price.total, CultureInfo.InvariantCulture), currencyCode)));
                        cachedResults.SaveSearch(flights);
                    }
                }
            }
            return flights;
        }
    }
}
