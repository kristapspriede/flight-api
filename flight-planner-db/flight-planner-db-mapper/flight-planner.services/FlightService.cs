using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using flight_planner.core.Models;
using flight_planner.core.Services;
using flight_planner.data;


namespace flight_planner.services
{
    public class FlightService
    {
        private static readonly Object obj = new Object(); 
        public async Task<ICollection<Flight>> GetFlights()
        {
            using (var context = new FlightPlannerDbContext())
            {
                return await context.Flights.Include(f => f.To).Include(f => f.From).ToListAsync();
            }
        }

        public async Task<ICollection<Airport>> GetAirports()
        {
            using (var context = new FlightPlannerDbContext())
            {
                return await context.Airports.ToListAsync();
            }
        }


        public async Task<Flight> AddFlight(Flight flight)
        {
            using (var context = new FlightPlannerDbContext())
            {
                
                    if (!Exists(flight))
                    {
                        flight = context.Flights.Add(flight);
                        await context.SaveChangesAsync();
                    }

                    return flight;
            }
        }

        public async Task<Flight> GetFlightById(int id)
        {
            using (var context = new FlightPlannerDbContext())
            {
                var flight = await context.Flights.Include(f => f.To).Include(f => f.From).SingleOrDefaultAsync(f => f.Id == id);
                    return flight;
            }
        }
        public async Task DeleteFlight(Flight flight)
        {
            using (var context = new FlightPlannerDbContext())
            {
                context.Flights.Attach(flight);
                    context.Flights.Remove(flight);
                    await context.SaveChangesAsync();
            }
        }

        public async Task<ICollection<Airport>> SearchAirports(string search)
        {
            search = search.ToLowerInvariant().Trim();
            using (var context = new FlightPlannerDbContext())
            {
                var query = context.Airports.Where(a => a.City.ToLower().Contains(search.ToLower())
                                                        && a.Country.ToLower().Contains(search.ToLower())
                                                        && a.AirportCode.ToLower().Contains(search.ToLower()));
                return await query.ToListAsync();
            }
        }
        public async Task<bool> DeleteFlightById(int id)
        {
           
                using (var context = new FlightPlannerDbContext())
                {
                    var flight = await context.Flights.SingleOrDefaultAsync(f => f.Id == id);
                if (flight != null)
                {
                    context.Flights.Remove(flight);
                    await context.SaveChangesAsync();
                }

                return true;
                }
            
        }
        public static bool Exists(Flight flight)
        {
            using (var context = new FlightPlannerDbContext())
            {
                return context.Flights.Any(f =>
                        //f.Id == flight.Id &&
                        f.From.AirportCode == flight.From.AirportCode &&
                        f.From.City == flight.From.City &&
                        f.From.Country == flight.From.Country &&
                        f.To.AirportCode == flight.To.AirportCode &&
                        f.To.City == flight.To.City &&
                        f.To.Country == flight.To.Country &&
                        f.Carrier == flight.Carrier &&
                        f.ArrivalTime == flight.ArrivalTime &&
                        f.DepartureTime == flight.DepartureTime);

            }
        }
        public static async Task<bool> DeleteFlights()
        {
            using (var context = new FlightPlannerDbContext())
            {
                context.Flights.RemoveRange(context.Flights);
                context.Airports.RemoveRange(context.Airports);
                await context.SaveChangesAsync();
                return true;
            }
        }
    }
}
