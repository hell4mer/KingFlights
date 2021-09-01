using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LiteDB;

namespace KingFlights.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FlightsController : ControllerBase
    {
        private readonly ILogger<FlightsController> _logger;

        public FlightsController(ILogger<FlightsController> logger)
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
                    cachedResults.Results.ForEach(f => flights.Add(new Flight(f.Id.ToString(), originLocationCode, destinationLocationCode, departureDate, returnDate, f.Transfers, f.Passengers, f.Price, currencyCode)));
                else
                {
                    await api.ConnectOAuth();
                    var results = await api.GetCheapFlightOffers(originLocationCode, destinationLocationCode, departureDate, returnDate, passengers, currencyCode);
                    if (results.data != null)
                    {
                        results.data.ForEach(f => flights.Add(new Flight(f.id, originLocationCode, destinationLocationCode, departureDate, returnDate, f.itineraries.Count, f.numberOfBookableSeats, double.Parse(f.price.total, CultureInfo.InvariantCulture), currencyCode)));
                        cachedResults.SaveSearch(flights);
                    }
                }
            }
            return flights;
        }

        [HttpGet]
        [Route("~/Flights/GetAirports")]
        public IEnumerable<Airport> GetAirports(string search)
        {
            var results = new List<Airport>();
            using (var db = new LiteDatabase("airports.db"))
            {
                var airportsTable = db.GetCollection<Airport>("airports");

                results = airportsTable.Query().Where(a => a.City.StartsWith(search)).ToList();
            }

            return results;
        }
    }
}
