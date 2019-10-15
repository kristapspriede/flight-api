﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace flight_planner.Models
{
    public class FlightSearchRequest
    {
        public string From { get; set; }
        public string To { get; set; }
        public string DepartureTime { get; set; }
        public string DepartureDate{ get; set; }
        public FlightSearchRequest(string from, string to, string departureDate)
        {
            this.From = from;
            this.To = to;
            this.DepartureDate = departureDate;
        }
    }
}