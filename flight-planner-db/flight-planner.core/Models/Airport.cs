using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace flight_planner.core.Models
{
    public class Airport
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string AirportCode { get; set; }
    }
}