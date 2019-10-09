using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using flight_planne.core.Models;

namespace flight_planner.core.Models
{
    public class Flight
    {
        public int Id { get; set; }
        public Airport To { get; set; }
        public Airport From { get; set; }
        public string Carrier { get; set; }
        public string DepartureTime { get; set; }
        public string ArrivalTime { get; set; }

        //public override bool Equals(object obj)
        //{

        //    var flight = obj as Flight;
        //    if (flight == null)

        //        return false;

        //    return flight.Carrier == Carrier &&
        //           flight.ArrivalTime == ArrivalTime &&
        //           flight.DepartureTime == DepartureTime &&
        //           flight.From.Equals(From) &&
        //           flight.To.Equals(To);



        //}
    }
}