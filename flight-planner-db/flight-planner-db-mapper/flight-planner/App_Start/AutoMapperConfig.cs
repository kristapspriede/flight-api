using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using flight_planner.core.Models;
using flight_planner.Models;

namespace flight_planner.App_Start
{
    public class AutoMapperConfig
    {
        public static IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AirportRequest, Airport>()
                    .ForMember(d => d.AirportCode, s => s.MapFrom(p => p.Airport))
                    .ForMember(d => d.Id, s => s.Ignore());
                cfg.CreateMap<Airport, AirportRequest>()
                    .ForMember(d => d.Airport, s => s.MapFrom(p => p.AirportCode));
                cfg.CreateMap<FlightRequest, Flight>();
                cfg.CreateMap<Flight, FlightRequest>();

            });
            config.AssertConfigurationIsValid();
            var mapper = config.CreateMapper();
            return mapper;
        }
    }
}