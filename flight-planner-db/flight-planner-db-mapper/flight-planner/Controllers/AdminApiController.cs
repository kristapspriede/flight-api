using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using flight_planner.core.Models;
using flight_planner.Attributes;
using flight_planner.core.Services;
using flight_planner.data;
using flight_planner.Models;
using flight_planner.services;
using Nest;

namespace flight_planner.Controllers
{

    [BasicAuthentication]
    public class AdminApiController : ApiController
    {

        private readonly IMapper _mapper;
        private readonly IFlightService _flightService;
        public AdminApiController(IFlightService flightService, IMapper mapper)
        {
            _flightService = flightService;
            _mapper = mapper;
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
            return Ok(flights.Select(f => _mapper.Map<FlightRequest>(f)).ToList());
        }
        [HttpGet]
        [Route("admin-api/flights/{id}")]
        public async Task<IHttpActionResult> Get(int id)
        {
            var flight = await _flightService.GetFlightById(id);
            if (flight == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<FlightRequest>(flight));

        }
        // POST: api/AdminApi
        public void Post([FromBody]string value)
        {
        }
        // PUT: api/AdminApi/5
        [HttpPut]
        [Route("admin-api/flights")]
        public async Task<IHttpActionResult> AddFlight(FlightRequest flight)
        {
            if (!IsValid(flight))
            {
                return BadRequest();
            }
            var result = await _flightService.AddFlight(_mapper.Map<Flight>(flight));

            if (!result.Succeeded)
            {
                return Conflict();
            }
            flight.Id = result.Entity.Id;
            return Created(string.Empty, flight);
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
        
    }
}
