using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using flight_planner.core.Models;

namespace flight_planner.core.Services
{
    public interface IFlightService : IEntityService<Flight>
    {
        Task DeleteFlights();
        Task<bool> FlightExists(Flight flight);
        Task<ServicesResult> DeleteFlightById(int id);
        Task<Flight> GetFlightById(int id);
        Task<ServicesResult> AddFlight(Flight flight);
        Task<IEnumerable<Flight>> GetFlights();
    }
}
