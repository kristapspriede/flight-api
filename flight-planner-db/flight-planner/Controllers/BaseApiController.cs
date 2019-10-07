using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using flight_planne.core.Models;
using flight_planner.core.Models;
using flight_planner.Models;

namespace flight_planner.Controllers
{
    public class BaseApiController : ApiController
    {
        // GET: api/BaseApi
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
        protected Flight ConvertFlightFromDomain(Flight flight)
        {
            return new Flight
            {
                Id = flight.Id,
                To = ConvertAirportFromoDomain(flight.To),
                From = ConvertAirportFromoDomain(flight.From),
                Carrier = flight.Carrier,
                ArrivalTime = flight.ArrivalTime,
                DepartureTime = flight.DepartureTime
            };
        }

        private Airport ConvertAirportFromoDomain(Airport airport)
        {
            return new Airport
            {
                City = airport.City,
                Country = airport.Country,
                AirportCode = airport.AirportCode
            };
        }

        // GET: api/BaseApi/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/BaseApi
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/BaseApi/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/BaseApi/5
        public void Delete(int id)
        {
        }
    }
}
