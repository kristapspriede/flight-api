using flight_planner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;

namespace flight_planner.Controllers
{
    public class TestingApiController : ApiController
    {
        [HttpPost]
        [Route("testing-api/clear")]
        public async Task<HttpResponseMessage> Clear(HttpRequestMessage request)
        {
            FlightStorage.ClearList();
            return request.CreateResponse(HttpStatusCode.OK, true);
        }
        [HttpPost]
        [Route("testing-api/")]
        public string Get()
        {
            return "Testing";
        }
    }
}
