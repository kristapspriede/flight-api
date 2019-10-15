using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using flight_planner.core.Interfaces;

namespace flight_planner.core.Models
{
    public abstract class Entity : IEntity
    {
        public int Id { get; set; }
    }
}
