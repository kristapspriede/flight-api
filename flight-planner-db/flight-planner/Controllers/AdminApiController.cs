using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using flight_planne.core.Models;
using flight_planner.Attributes;
using flight_planner.core.Models;
using flight_planner.Models;
using flight_planner.services;
using Nest;

namespace flight_planner.Controllers
{

    [BasicAuthentication]
    public class AdminApiController : ApiController
    {

        private readonly FlightService _flightService;
        public AdminApiController()
        {
            _flightService = new FlightService();

        }
        // GET: api/AdminApi
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/AdminApi/5

        [HttpGet]
        [Route("admin-api/flights/{id}")]
        public async Task<HttpResponseMessage> Get(HttpRequestMessage request, int id)
        {
            var flight = await _flightService.GetFlightById(id);
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
        public async Task<HttpResponseMessage> AddFlight(HttpRequestMessage request, FlightRequest flight)
        {
            
            if (IsValid(ConvertFlightRequestToFlight(flight)) )
            {
                var domainFlight = await _flightService.AddFlight(ConvertFlightRequestToFlight(flight));
                return request.CreateResponse(HttpStatusCode.Created, domainFlight);
            }
            return request.CreateResponse(HttpStatusCode.BadRequest, flight);
            //flight.Id = FlightStorage.GetId();
            //if (!FlightStorage.AddFlight(flight))
            //{
            //    return request.CreateResponse(HttpStatusCode.Conflict, flight);
            //}
            //FlightStorage.AddFlight(flight);

        }

        // DELETE: api/AdminApi/5
        [HttpDelete]
        [Route("admin-api/flights/{id}")]
        public async Task<HttpResponseMessage> Delete(HttpRequestMessage request, int id)
        {
            _flightService.DeleteFlightById(id);
            return request.CreateResponse(HttpStatusCode.OK);

        }
        private bool IsValid(Flight flight)
        {
            var domainFlight = convertFlightToFlightRequest(flight);
            return (!string.IsNullOrEmpty(domainFlight.ArrivalTime) &&
                    !string.IsNullOrEmpty(domainFlight.DepartureTime) &&
                    !string.IsNullOrEmpty(domainFlight.Carrier) &&
                    IsValidAirport(domainFlight.From) && IsValidAirport(domainFlight.To) &&
                    ValidateDates(domainFlight.DepartureTime, domainFlight.ArrivalTime) &&
                    isDifferentAirport(domainFlight.From, domainFlight.To));


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
            
        }
        private bool ValidateDates(string departure, string arrival)
        {
            if (!string.IsNullOrEmpty(departure) && !string.IsNullOrEmpty(arrival))
            {
                var arrivalDate = DateTime.Parse(arrival);
                var departureDate = DateTime.Parse(departure);
                var compare = DateTime.Compare(arrivalDate, departureDate);

                return compare > 0;
            }
            return false;
        }
        protected Flight ConvertFlightRequestToFlight(FlightRequest flightRequest)
        {
            return new Flight
            {
                Id = flightRequest.Id,
                To = ConvertAirportRequestToAirport(flightRequest.To),
                From = ConvertAirportRequestToAirport(flightRequest.From),
                Carrier = flightRequest.Carrier,
                ArrivalTime = flightRequest.ArrivalTime,
                DepartureTime = flightRequest.DepartureTime
            };
        }

        private Airport ConvertAirportRequestToAirport(AirportRequest airportRequest)
        {
            return new Airport
            {
                City = airportRequest.City,
                Country = airportRequest.Country,
                AirportCode = airportRequest.Airport
            };
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
