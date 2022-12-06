using Licenta.DataAccess.Abstractions;
using Licenta.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Licenta.DataAccess.Repositories
{
    public class EfBaseRepository<T> : IBaseRepository<T> where T : DataEntity
    {
        protected readonly ApplicationDbContext DbContext;

        protected EfBaseRepository(ApplicationDbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        public T Add(T itemToAdd)
        {
            var entity = DbContext.Add(itemToAdd);
            DbContext.SaveChanges();
            return entity.Entity;
        }

        public virtual IEnumerable<T> GetAll()
        {
            return DbContext.Set<T>()
                            .AsEnumerable();
        }

        public virtual T GetById(Guid id)
        {
            return DbContext
                .Set<T>()
                .SingleOrDefault(entity => entity.Id.Equals(id));
        }

        public bool Remove(Guid id)
        {
            var entityToRemove = GetById(id);
            if (entityToRemove == null) return false;
            DbContext.Remove(entityToRemove);
            DbContext.SaveChanges();
            return true;
        }

        public T Update(T itemToUpdate)
        {
            var entity = DbContext.Update(itemToUpdate);
            DbContext.SaveChanges();
            return entity.Entity;
        }
    }
}
