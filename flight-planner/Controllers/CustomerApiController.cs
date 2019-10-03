using flight_planner.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;


namespace flight_planner.Controllers
{
    public class CustomerApiControllerController : ApiController
    {
        // GET: api/CustomerApi
        [HttpGet]
        [Route("api/FlightSearchRequest/{id}")]

        public async Task <HttpResponseMessage> Get(HttpRequestMessage request, int id)
        {
            var flight = FlightStorage.GetFlightById(id);
            if (flight == null)
            {
                request.CreateResponse(HttpStatusCode.NotFound);
            }
            return request.CreateResponse(HttpStatusCode.OK, flight);
        }

        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        // GET: api/CustomerApi/5

        [HttpGet]
        [Route("api/airports")]
        public AirportRequest[] GetAirports(string search)
        {
            var flights = FlightStorage.GetFlights();
            var result = new HashSet<AirportRequest>();
            flights.ForEach(f =>
            {
                result.Add(f.From);
                result.Add(f.To);
            });
            return result.Where(a => a.Airport.ToLower().Contains(search.ToLower().Trim()) ||
                            a.City.ToLower().Contains(search.ToLower().Trim()) ||
                            a.Country.ToLower().Contains(search.ToLower().Trim()))
                .ToArray();
        }

        // POST: api/CustomerApi
        [HttpPost]
        [Route("api/flights/search")]
        public async Task<HttpResponseMessage> FlightSearch(HttpRequestMessage request, FlightSearchRequest search)
        {
            if (IsValid(search) && NotSameAirport(search))
            {
                var result = FlightStorage.GetFlights();
                var matchedItems = result.Where(f => f.From.Airport.ToLower().Contains(search.From.ToLower()) ||
                                                     f.To.Airport.ToLower().Contains(search.To.ToLower()) ||
                                                     DateTime.Parse(f.DepartureTime) ==
                                                     DateTime.Parse(search.DepartureDate)).ToList();
                var response = new FlightSearchResult
                {
                    TotalItems = result.Length,
                    Items = matchedItems,
                    Page = matchedItems.Any() ? 1 : 0
                };
                return request.CreateResponse(HttpStatusCode.OK, response);
            }
            return request.CreateResponse(HttpStatusCode.BadRequest);
        }


        private bool NotSameAirport(FlightSearchRequest search)
        {
            return !string.Equals(search.From, search.To, StringComparison.InvariantCultureIgnoreCase);
        }
        private bool IsValid(FlightSearchRequest search)
        {
            return search != null && !string.IsNullOrEmpty(search.From) && 
                                     !string.IsNullOrEmpty(search.To) &&
                                     !string.IsNullOrEmpty(search.DepartureDate);
        }

        [HttpGet]
        [Route("api/flights/{id}")]
        public async Task<HttpResponseMessage> FlightSearchById(HttpRequestMessage request, int id)
        {
            var flight = FlightStorage.GetFlightById(id);
            if (flight == null)
            {
                return request.CreateResponse(HttpStatusCode.NotFound);
            }
            return request.CreateResponse(HttpStatusCode.OK, flight);
        }


        // PUT: api/CustomerApi/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/CustomerApi/5
        public void Delete(int id)
        {
        }
    }
}
