using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using flight_planne.core.Models;
using flight_planner.core.Models;
using flight_planner.data;


namespace flight_planner.services
{
    public class FlightService
    {

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
                return await context.Airports.Include(a => a.AirportCode).Include(a => a.City).Include(a => a.Country)
                    .ToListAsync();
            }
        }


        public async Task<Flight> AddFlight(Flight flight)
        {
            using (var context = new FlightPlannerDbContext())
            {
                flight = context.Flights.Add(flight);
                await context.SaveChangesAsync();
                return flight;
            }
        }

        public async Task<Flight> GetFlightById(int id)
        {
            using (var context = new FlightPlannerDbContext())
            {
                var flight = await context.Flights.Include(f => f.To).Include(f => f.From)
                    .SingleOrDefaultAsync(f => f.Id == id);
                return flight;
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
        public async Task<Flight> DeleteFlightById(int id)
        {

            using (var context = new FlightPlannerDbContext())
            {
                var flight = await context.Flights.Include(f => f.To).Include(f => f.From).SingleOrDefaultAsync(f => f.Id == id);
                var to = flight.To;
                var from = flight.From;

                context.Flights.Remove(flight);
                context.Airports.Remove(to); //???
                context.Airports.Remove(from);
                //???
                await context.SaveChangesAsync();
                return flight;
            }
        }
        public async Task<bool> Exists(Flight flight)
        {
            using (var context = new FlightPlannerDbContext())
            {
                var domainflight = context.Flights.Any(f => f.Carrier == flight.Carrier && f.ArrivalTime == flight.ArrivalTime && f.DepartureTime == flight.DepartureTime);
                if (domainflight)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
        }





    }
}
