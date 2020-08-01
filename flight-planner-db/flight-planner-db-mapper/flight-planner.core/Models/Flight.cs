using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using flight_planner.core.Models;

namespace flight_planner.core.Models
{
    public class Flight : Entity
    {
        //public int Id { get; set; }
        public virtual Airport To { get; set; }
        public virtual Airport From { get; set; }
        public string Carrier { get; set; }
        public string DepartureTime { get; set; }
        public string ArrivalTime { get; set; }
    }
}