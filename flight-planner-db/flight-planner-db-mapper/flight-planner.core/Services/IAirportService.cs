using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using flight_planner.core.Models;
using flight_planner.core.Services;

namespace flight_planner.core.services
{
    public interface IAirportService : IEntityService<Airport>
    {
        Task<IEnumerable<Airport>> SearchAirports(string search);
        Task<ICollection<Airport>> GetAirports();
    }
}
