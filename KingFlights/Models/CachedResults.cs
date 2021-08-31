using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KingFlights
{
    public class CachedResults
    {
        const string ConnectionString = "cachedflights.db";

        public FlightSearch Search { get; set; }
        public List<FlightSearchResult> Results { get; set; }
        public bool HasCachedResults => Results.Count > 0;

        public CachedResults()
        {
            Results = new List<FlightSearchResult>();
        }

        public CachedResults(string searchParams):this()
        {
            using (var db = new LiteDatabase(ConnectionString))
            {
                var flightSearch = db.GetCollection<FlightSearch>("flightSearches");
                
                var s = flightSearch.FindOne(r => r.SearchParams == searchParams);

                if (s != null)
                {
                    var flightSearchResults = db.GetCollection<FlightSearchResult>("flightSearchesResults");
                    Results = flightSearchResults.Query().Where(r => r.SearchId == s.Id).ToList();
                    Search = s;
                }
                else
                {
                    Search = new FlightSearch(searchParams);
                }
            }
        }

        public void SaveSearch(List<Flight> flights)
        {
            using (var db = new LiteDatabase(ConnectionString))
            {
                var flightSearch = db.GetCollection<FlightSearch>("flightSearches");
                flightSearch.Insert(Search);
                flightSearch.EnsureIndex(f => f.Id);
                flightSearch.EnsureIndex(f => f.SearchParams);

                var flightSearchResults = db.GetCollection<FlightSearchResult>("flightSearchesResults");

                Results = new List<FlightSearchResult>();
                flights.ForEach(f => Results.Add(new FlightSearchResult(Search.Id, f.Transfers, f.Passengers, f.Price)));
                flightSearchResults.InsertBulk(Results);
                flightSearchResults.EnsureIndex(f => f.SearchId);
            }
        }
    }
}
