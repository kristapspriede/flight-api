using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using flight_planner.Attributes;
using flight_planner.Models;

namespace flight_planner.Controllers
{

    [BasicAuthentication]
    public class AdminApiController : ApiController
    {
        
        private Random _random;
        public AdminApiController()
        {
            _random = new Random();
            
        }
        // GET: api/AdminApi
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/AdminApi/5

        [HttpGet]
        [Route("admin-api/flights/{id}")]
        public async Task <HttpResponseMessage> Get(HttpRequestMessage request, int id)
        {
            var flight = FlightStorage.GetFlightById(id);
            if (flight == null)
            {
                return request.CreateResponse(HttpStatusCode.NotFound, flight);
            }
            return request.CreateResponse(HttpStatusCode.NotFound, flight);
        }

        // POST: api/AdminApi
        public void Post([FromBody]string value)
        {
        }


        // PUT: api/AdminApi/5
        [HttpPut]
        [Route("admin-api/flights")]
        public async Task <HttpResponseMessage> AddFlight(HttpRequestMessage request, Flight flight)
        {
            if (!IsValid(flight))
            {
                return request.CreateResponse(HttpStatusCode.BadRequest, flight);
            }
            flight.Id = FlightStorage.GetId();
            if (!FlightStorage.AddFlight(flight))
            {
                return request.CreateResponse(HttpStatusCode.Conflict, flight);
            }
            FlightStorage.AddFlight(flight);
            return request.CreateResponse(HttpStatusCode.Created, flight);
        }

        // DELETE: api/AdminApi/5
        [HttpDelete]
        [Route("admin-api/flights/{id}")]
        public async Task <HttpResponseMessage> Delete(HttpRequestMessage request, int id)
        {
            FlightStorage.RemoveFlightById(id);
            return request.CreateResponse(HttpStatusCode.OK);
            
        }
        private bool IsValid(Flight flight)
        {
            //PROBLEM
            return (!string.IsNullOrEmpty(flight.ArrivalTime) &&
                    !string.IsNullOrEmpty(flight.DepartureTime) &&
                    !string.IsNullOrEmpty(flight.Carrier) &&
                    IsValidAirport(flight.From) && IsValidAirport(flight.To) && 
                    ValidateDates(flight.DepartureTime, flight.ArrivalTime) &&
                    isDifferentAirport(flight.From, flight.To)); 
        }
        private bool IsValidAirport(AirportRequest airport)
        {
            return airport != null &&
                   !string.IsNullOrEmpty(airport.Airport) &&
                   !string.IsNullOrEmpty(airport.City) &&
                   !string.IsNullOrEmpty(airport.Country);
        }
        private bool isDifferentAirport(AirportRequest airportFrom, AirportRequest airportTo)
        {
            return airportFrom.Airport.ToLowerInvariant().Trim() != (airportTo.Airport.ToLowerInvariant()) &&
                   airportFrom.City.ToLowerInvariant().Trim() != (airportTo.City.ToLowerInvariant());
                   //airportFrom.Country.ToLowerInvariant().Trim() != (airportTo.Country.ToLowerInvariant());
        }
        private bool ValidateDates(string departure, string arrival)
        {
            if(!string.IsNullOrEmpty(departure)&& !string.IsNullOrEmpty(arrival))
            {
                var arrivalDate = DateTime.Parse(arrival);
                var departureDate = DateTime.Parse(departure);
                var compare = DateTime.Compare(arrivalDate, departureDate);

                return compare > 0;
            }
            return false;
        }
    }
}
