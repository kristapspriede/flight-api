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
