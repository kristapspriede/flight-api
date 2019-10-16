using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using flight_planner.core.Models;
using flight_planner.core.Services;
using System.Data.Entity;
using flight_planner.data;

namespace flight_planner.services
{
    public class DbService : IDbService
    {
        protected readonly IFlightPlannerDbContext _ctx;

        public DbService(IFlightPlannerDbContext context)
        {
            _ctx = context;
        }

        public ServicesResult Create<T>(T entity) where T : Entity
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _ctx.Set<T>().Add(entity);
            _ctx.SaveChanges();

            return new ServicesResult(true).Set(entity);
        }

        public ServicesResult Delete<T>(T entity) where T : Entity
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            _ctx.Set<T>().Remove(entity);
            _ctx.SaveChanges();

            return new ServicesResult(true).Set(entity);
        }

        public bool Exists<T>(int id) where T : Entity
        {
            return QueryById<T>(id).Any();
        }

        public IEnumerable<T> Get<T>() where T : Entity
        {
            return Query<T>().ToList();
        }

        public virtual Task<T> GetById<T>(int id) where T : Entity//async/await
        {
            return _ctx.Set<T>().SingleOrDefaultAsync(e => e.Id == id);
        }
        public virtual IQueryable<T> Query<T>() where T : Entity
        {
            return _ctx.Set<T>().AsQueryable();
        }
        public virtual IQueryable<T> QueryById<T>(int id) where T : Entity
        {
            return Query<T>().Where(t => t.Id == id);
        }
        public ServicesResult Update<T>(T entity) where T : Entity
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            _ctx.Entry(entity).State = EntityState.Modified;
            _ctx.SaveChanges();

            return new ServicesResult(true).Set(entity);
        }
    }
}
