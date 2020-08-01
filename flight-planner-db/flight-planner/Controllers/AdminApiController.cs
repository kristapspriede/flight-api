using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using flight_planner.core.Models;
using flight_planner.Attributes;
using flight_planner.data;
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
        [Route("admin-api/get/flights")]
        public async Task<IHttpActionResult> GetFlights()
        {
            var flights = await _flightService.GetFlights();
            return Ok(flights.Select(convertFlightToFlightRequest));
        }
        [HttpGet]
        [Route("admin-api/flights/{id}")]
        public async Task<HttpResponseMessage> Get(HttpRequestMessage request, int id)
        {
            var flight = await _flightService.GetFlightById(id);
            if (flight == null)
            {
                return request.CreateResponse(HttpStatusCode.NotFound, flight);
            }
            return request.CreateResponse(HttpStatusCode.OK, flight);

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
            if (!IsValid(flight))
            {
                return request.CreateResponse(HttpStatusCode.BadRequest, flight);
            }
            var doExists = ConvertFlightRequestToFlight(flight);
            if (FlightService.Exists(doExists))
            {
                return request.CreateResponse(HttpStatusCode.Conflict);
            }
            else
            {
                var addFlight = ConvertFlightRequestToFlight(flight);
                addFlight = await _flightService.AddFlight(addFlight);
                //flight.Id = addFlight.Id;
                flight = convertFlightToFlightRequest(addFlight);
                return request.CreateResponse(HttpStatusCode.Created, flight);
            }
        }
        // DELETE: api/AdminApi/5
        [HttpDelete]
        [Route("admin-api/flights/{id}")]
        public async Task<HttpResponseMessage> Delete(HttpRequestMessage request, int id)
        {
            await _flightService.DeleteFlightById(id);
            return request.CreateResponse(HttpStatusCode.OK);
        }
        private bool IsValid(FlightRequest flight)
        {
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
                   !string.IsNullOrEmpty(airport.City);
        }
        private bool isDifferentAirport(AirportRequest airportFrom, AirportRequest airportTo)
        {
            return !airportFrom.Airport.ToLower().Equals(airportTo.Airport.ToLower()) &&
                           !airportFrom.City.ToLower().Equals(airportTo.City.ToLower());
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
