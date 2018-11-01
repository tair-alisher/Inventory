using Inventory.DAL.EF;
using Inventory.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Inventory.DAL.Repositories
{
    class PartialRepository<T> : IPartialRepository<T> where T : class
    {
        private InventoryContext InventContext;
        private AccountContext AccountContext;
        private DbSet<T> DbSet;

        public PartialRepository(InventoryContext context)
        {
            InventContext = context;
            DbSet = context.Set<T>();
        }

        public PartialRepository(AccountContext context)
        {
            AccountContext = context;
            DbSet = context.Set<T>();
        }

        public IEnumerable<T> Find(Func<T, bool> predicate)
        {
            return GetAll().Where(predicate);
        }

        public T Get(int? id)
        {
            return DbSet.Find(id);
        }

        public IEnumerable<T> GetAll()
        {
            return DbSet;
        }
    }
}
