using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using flight_planner.core.Models;
using flight_planner.core.services;
using flight_planner.data;
using flight_planner.core.Services;
namespace flight_planner.services
{
    public class AirportService : EntityService<Airport>, IAirportService
    {
        public AirportService(IFlightPlannerDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Airport>> SearchAirports(string search)
        {
            search = search.ToLower().Trim();
            var airports = Query().Where(a =>
                a.AirportCode.ToLower().Contains(search) ||
                a.City.ToLower().Contains(search) ||
                a.Country.ToLower().Contains(search));
            return await airports.ToListAsync();
        }
        public async Task<ICollection<Airport>> GetAirports()
        {
            using (var context = new FlightPlannerDbContext())
            {
                return await context.Airports.ToListAsync();
            }
        }
    }
}
