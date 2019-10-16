using flight_planner.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using flight_planner.core.Models;
using flight_planner.services;
using flight_planner.Controllers;




namespace flight_planner.Controllers
{
    public class CustomerApiController : ApiController
    {
        private readonly FlightService _flightService;
        public CustomerApiController()
        {
            _flightService = new FlightService();

        }
        [HttpGet]
        [Route("api/FlightSearchRequest/{id}")]
        public async Task<IHttpActionResult> Get(int id)
        {
            var flight = await _flightService.GetFlightById(id);
            if (flight == null)
            {
                return NotFound();
            }
            return Ok(convertFlightToFlightRequest(flight));
        }
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        // GET: api/CustomerApi/5
        [HttpGet]
        [Route("api/airports")]
        public async Task<IHttpActionResult> GetAirports(string search)
        {
            var airports = await _flightService.GetAirports();//SearchAirports
            var result = new HashSet<AirportRequest>();
            airports.ToList().ForEach(a =>
            {
                result.Add(ConvertAirportToAirportRequest(a));

            });
            return Ok(result
                .Where(a => a.Airport.ToLower().Contains(search.ToLower().Trim()) ||
                            a.City.ToLower().Contains(search.ToLower().Trim()) ||
                            a.Country.ToLower().Contains(search.ToLower().Trim()))
                .ToArray());
        }
        // POST: api/CustomerApi
        [HttpPost]
        [Route("api/flights/search")]
        public async Task<HttpResponseMessage> FlightSearch(HttpRequestMessage request, FlightSearchRequest search)
        {
            if (IsValid(search) && NotSameAirport(search))
            {
                var result = await _flightService.GetFlights();
                var matchedItems = result.DistinctBy(f => f.From.AirportCode.ToLower().Contains(search.From.ToLower()) ||
                                                     f.To.AirportCode.ToLower().Contains(search.To.ToLower()) ||
                                                     DateTime.Parse(f.DepartureTime) ==
                                                     DateTime.Parse(search.DepartureDate)).ToList();
                var response = new FlightSearchResult
                {
                    TotalItems = matchedItems.Count,
                    Items = matchedItems,
                    Page = matchedItems.Any() ? 1 : 0
                };
                return request.CreateResponse(HttpStatusCode.OK, response);
            }
            else
            {
                return request.CreateResponse(HttpStatusCode.BadRequest);
            }
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
        public async Task<IHttpActionResult> FlightSearchById(int id)
        {
            var flight = await _flightService.GetFlightById(id);
            if (flight == null)
            {
                return NotFound();
            }
            return Ok(convertFlightToFlightRequest(flight));
        }
        // PUT: api/CustomerApi/5
        public void Put(int id, [FromBody]string value)
        {
        }
        // DELETE: api/CustomerApi/5
        public void Delete(int id)
        {
        }
        protected FlightRequest convertFlightToFlightRequest(Flight flight)
        {
            return new FlightRequest
            {
                Id = flight.Id,
                To = ConvertAirportToAirportRequest(flight.To),
                From = ConvertAirportToAirportRequest(flight.From),
                Carrier = flight.Carrier,
                ArrivalTime = flight.ArrivalTime,
                DepartureTime = flight.DepartureTime
            };
        }
        private AirportRequest ConvertAirportToAirportRequest(Airport airport)
        {
            return new AirportRequest
            {
                City = airport.City,
                Country = airport.Country,
                Airport = airport.AirportCode
            };
        }
    }
}
