using Licenta.DataAccess.Abstractions;
using Licenta.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Licenta.DataAccess.Repositories
{
    public class EFBaseRepository<T> : IBaseRepository<T> where T : DataEntity
    {
        protected readonly ApplicationDbContext dbContext;

        protected EFBaseRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public T Add(T itemToAdd)
        {
            var entity = dbContext.Add<T>(itemToAdd);
            dbContext.SaveChanges();
            return entity.Entity;
        }

        public virtual IEnumerable<T> GetAll()
        {
            return dbContext.Set<T>()
                            .AsEnumerable();
        }

        public virtual T GetById(Guid id)
        {
            return dbContext
                .Set<T>()
                .SingleOrDefault(entity => entity.Id.Equals(id));
        }

        public bool Remove(Guid id)
        {
            var entityToRemove = GetById(id);
            if (entityToRemove == null) return false;
            dbContext.Remove<T>(entityToRemove);
            dbContext.SaveChanges();
            return true;
        }

        public T Update(T itemToUpdate)
        {
            var entity = dbContext.Update<T>(itemToUpdate);
            dbContext.SaveChanges();
            return entity.Entity;
        }
    }
}
