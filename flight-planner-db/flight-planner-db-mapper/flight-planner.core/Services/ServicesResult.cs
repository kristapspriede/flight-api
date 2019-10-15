using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Design;

namespace flight_planner.core.Services
{
    public class ServicesResult
    {
        public ServicesResult(int id, bool succeeded)
        {
            Id = id;
            Succeeded = succeeded;
        }
        public ServicesResult(bool succeeded)
        {
            Succeeded = succeeded;
        }

       
        public int Id { get; }
        
        public bool Succeeded { get;}
    }
}
