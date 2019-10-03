using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace flight_planner.Models
{
    public class FlightStorage
    {
        private static readonly Object obj = new Object();
        private static int _id;
        private static SynchronizedCollection<Flight> _flights { get; set; }
        static FlightStorage()
        {
            _flights = new SynchronizedCollection<Flight>();
            _id = 1;

        }
        


        public static bool AddFlight(Flight flight)
        {
            lock (obj) {
                if (!_flights.Any(f => f.Equals(flight)))
                {
                    _flights.Add(flight);
                    return true;
                }
                return false;
            }
               
        }
        public static void RemoveFlight(Flight flight)
        {
            _flights.Remove(flight);
        }
        public static void ClearList()
        {
            _flights.Clear();
        }
        public static void RemoveFlightById(int id)
        {
            var flight = GetFlightById(id);
            if(flight != null)
            {
                _flights.Remove(flight);
            }
        }
        public static Flight GetFlightById(int id)
        {
            lock (obj)
            {
                return _flights.FirstOrDefault(f => f.Id == id);
            }
        }
        public static int GetId()
        {
            return _id++;
        }
        public static Flight[] GetFlights()
        {          
            return _flights.ToArray();
        }
    }
}