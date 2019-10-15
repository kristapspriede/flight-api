using flight_planner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using flight_planner.core.Services;
using flight_planner.services;

namespace flight_planner.Controllers
{
    public class TestingApiController : ApiController
    {
        private readonly IFlightService _flightService;
        public TestingApiController(IFlightService flightService)
        {
            _flightService = flightService;
        }
        [HttpPost]
        [Route("testing-api/clear")]
        public async Task<bool> Clear()
        {
            await _flightService.DeleteFlights();
            return true;
        }
        [HttpPost]
        [Route("testing-api/")]
        public string Get()
        {
            return "Testing";
        }
    }
}
