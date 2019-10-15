// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultRegistry.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using AutoMapper;
using flight_planner.App_Start;
using flight_planner.core.services;
using flight_planner.core.Services;
using flight_planner.data;
using flight_planner.services;
using StructureMap;

namespace flight_planner.DependencyResolution {
    using StructureMap.Configuration.DSL;
    using StructureMap.Graph;
	
    public class DefaultRegistry : Registry {
        #region Constructors and Destructors

        public DefaultRegistry() {
            Scan(
                scan => {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
					scan.With(new ControllerConvention());
                });
            //For<IExample>().Use<Example>();
            For<IFlightPlannerDbContext>().Use<FlightPlannerDbContext>().Transient();
            For<IDbService>().Use<DbService>();
            For(typeof(IEntityService<>)).Use(typeof(EntityService<>));
            For<IFlightService>().Use<FlightService>();
            For<IAirportService>().Use<AirportService>();
            var mapper = AutoMapperConfig.GetMapper();
            For<IMapper>().Use(mapper).Singleton();
        }

        #endregion
    }
}