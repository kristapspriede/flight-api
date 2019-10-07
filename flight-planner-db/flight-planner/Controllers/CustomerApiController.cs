using flight_planner.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using flight_planne.core.Models;
using flight_planner.services;
using flight_planner.Controllers;
using flight_planner.core.Models;


namespace flight_planner.Controllers
{
    public class CustomerApiController : ApiController
    {
        private readonly FlightService _flightService;
        public CustomerApiController()
        {
            _flightService = new FlightService();

        }
        protected Flight ConvertFlightToDomain(FlightRequest flightRequest)
        {
            return new Flight
            {
                Id = flightRequest.Id,
                To = ConvertAirportToDomain(flightRequest.To),
                From = ConvertAirportToDomain(flightRequest.From),
                Carrier = flightRequest.Carrier,
                ArrivalTime = flightRequest.ArrivalTime,
                DepartureTime = flightRequest.DepartureTime
            };
        }

        private Airport ConvertAirportToDomain(AirportRequest airportRequest)
        {
            return new Airport
            {
                City = airportRequest.City,
                Country = airportRequest.Country,
                AirportCode = airportRequest.Airport
            };
        }
        private FlightRequest ConvertFlightFromDomain(Flight flight)
        {
            return new FlightRequest()
            {
                Id = flight.Id,
                To = ConvertAirportFromDomain(flight.To),
                From = ConvertAirportFromDomain(flight.From),
                Carrier = flight.Carrier,
                ArrivalTime = flight.ArrivalTime,
                DepartureTime = flight.DepartureTime
            };
        }

        private AirportRequest ConvertAirportFromDomain(Airport airport)
        {
            return new AirportRequest()
            {
                City = airport.City,
                Country = airport.Country,
                Airport = airport.AirportCode
            };
        }
        // GET: api/CustomerApi
        [HttpGet]
        [Route("api/FlightSearchRequest/{id}")]

        public async Task <HttpResponseMessage> Get(HttpRequestMessage request, int id)
        {
            var flight = _flightService.GetFlightById(id);
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
        public async Task<IHttpActionResult> GetAirports(string search)
        {
            var airports = await _flightService.GetAirports();
            var result = new HashSet<AirportRequest>();
            airports.ToList().ForEach(a =>
            {
                result.Add(ConvertAirportFromDomain(a));
                
            });
            return Ok(result.Where(a => a.Airport.ToLower().Contains(search.ToLower().Trim()) ||
                                         a.City.ToLower().Contains(search.ToLower().Trim()) ||
                                        a.Country.ToLower().Contains(search.ToLower().Trim()))
                .ToArray());
        }

        

        // POST: api/CustomerApi
        [HttpPost]
        [Route("api/flights/search")]
        public async Task<IHttpActionResult> FlightSearch(FlightSearchRequest search)
        {
            if (!IsValid(search) || !NotSameAirport(search)) return BadRequest();
            
                var result = await _flightService.GetFlights();
                var matchedItems = result.Where(f => f.From.AirportCode.ToLower().Contains(search.From.ToLower()) ||
                                                     f.To.AirportCode.ToLower().Contains(search.To.ToLower()) ||
                                                     DateTime.Parse(f.DepartureTime) ==
                                                     DateTime.Parse(search.DepartureDate)).DistinctBy(f => f.Carrier).ToList();
                var response = new FlightSearchResult
                {
                    TotalItems = result.Count,
                    Items = matchedItems,
                    Page = matchedItems.Any() ? 1 : 0
                };
                return Ok(response);
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
            var flight = _flightService.GetFlightById(id);
            //if (flight == null)
            //{
            //    return request.CreateResponse(HttpStatusCode.NotFound);
            //}
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
