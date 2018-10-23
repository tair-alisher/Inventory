using Inventory.DAL.EF;
using Inventory.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Inventory.DAL.Repositories
{
    class BaseRepository<T> : IRepository<T> where T : class
    {
        private InventoryContext Context;
        private DbSet<T> DbSet;

        public BaseRepository(InventoryContext context)
        {
            Context = context;
            DbSet = context.Set<T>();
        }

        public virtual IEnumerable<T> GetAll()
        {
            return DbSet;
        }

        public T Get(Guid? id)
        {
            return DbSet.Find(id);
        }

        public IEnumerable<T> Find(Func<T, Boolean> predicate)
        {
            return GetAll().Where(predicate);
        }

        public void Create(T entity)
        {
            DbSet.Add(entity);
        }

        public void Update(T entity)
        {
            var entityEntry = Context.Entry(entity);
            if (entityEntry.State == EntityState.Detached)
                DbSet.Attach(entity);
            entityEntry.State = EntityState.Modified;
        }

        public void Delete(Guid id)
        {
            T entity = DbSet.Find(id);
            if (entity != null)
                DbSet.Remove(entity);
        }
    }
}
