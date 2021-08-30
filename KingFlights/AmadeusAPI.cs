using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace KingFlights
{
    public class AmadeusAPI
    {
        private string apiKey;
        private string apiSecret;
        private string bearerToken;
        private HttpClient http;

        public AmadeusAPI(IConfiguration config, IHttpClientFactory httpFactory)
        {
            apiKey = config.GetValue<string>("AmadeusAPI:APIKey");
            apiSecret = config.GetValue<string>("AmadeusAPI:APISecret");
            http = httpFactory.CreateClient("AmadeusAPI");
        }

        public async Task ConnectOAuth()
        {
            var message = new HttpRequestMessage(HttpMethod.Post, "/v1/security/oauth2/token");
            message.Content = new StringContent(
                $"grant_type=client_credentials&client_id={apiKey}&client_secret={apiSecret}",
                Encoding.UTF8, "application/x-www-form-urlencoded"
            );

            var results = await http.SendAsync(message);
            await using var stream = await results.Content.ReadAsStreamAsync();
            var oauthResults = await JsonSerializer.DeserializeAsync<OAuthResults>(stream);

            bearerToken = oauthResults.access_token;
        }

        private class OAuthResults
        {
            public string access_token { get; set; }
        }

        private void ConfigBearerTokenHeader()
        {
            http.DefaultRequestHeaders.Add("Authorization", $"Bearer {bearerToken}");
        }

        public async Task<FlightOfferResults> GetCheapFlightOffers(string originLocationCode, string destinationLocationCode, DateTime departureDate, DateTime? returnDate, int passengers, string currencyCode)
        {
            var message = new HttpRequestMessage(HttpMethod.Get,
                $"/v2/shopping/flight-offers?originLocationCode={originLocationCode}&destinationLocationCode={destinationLocationCode}&departureDate={departureDate:yyyy-MM-dd}&returnDate={returnDate:yyyy-MM-dd}&adults={passengers}&currencyCode={currencyCode}");

            ConfigBearerTokenHeader();
            var response = await http.SendAsync(message);
            using var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<FlightOfferResults>(stream);
        }
    }
    public class FlightOfferResults
    {
        public string description { get; set; }
        public List<FlightOffer> data { get; set; }
    }

    public class FlightOffer
    {
        public string id { get; set; }
        public bool oneWay { get; set; }
        public int numberOfBookableSeats { get; set; }
        public List<Itinerary> itineraries { get; set; }
        public Price price { get; set; }
    }

    public class Price
    {
        public string currency { get; set; }
        public string total { get; set; }
    }

    public class Itinerary
    {
        public string duration { get; set; }
        public List<Segment> segments { get; set; }
    }

    public class Segment
    {
        public int numberOfStops { get; set; }
    }
}
