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
    public class FlightService : EntityService<Flight>, IFlightService
    {
        public FlightService(IFlightPlannerDbContext context) : base(context)
        {

        }
        public async Task<IEnumerable<Flight>> GetFlights()
        {
            return await Task.FromResult(Get());
        }

        public async Task<ICollection<Airport>> GetAirports()
        {
            using (var context = new FlightPlannerDbContext())
            {
                return await context.Airports.ToListAsync();
            }
        }


        public async Task<ServicesResult> AddFlight(Flight flight)
        {
            if (await FlightExists(flight))
            {
                return new ServicesResult(false);
            }
            return Create(flight);
        }

        public async Task<Flight> GetFlightById(int id)
        {
            return await GetById(id);
        }
        public async Task DeleteFlights()
        {
            _ctx.Flights.RemoveRange(_ctx.Flights);
            _ctx.Airports.RemoveRange(_ctx.Airports);
            await _ctx.SaveChangesAsync();
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
        public async Task<ServicesResult> DeleteFlightById(int id)
        {
            var flight = await GetById(id);
            if (flight != null)
            {
                Delete(flight);
            }
            
            return new ServicesResult(true);
        }
        public async Task<bool> FlightExists(Flight flight)
        {

            return await Query().AnyAsync(f =>
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
}
