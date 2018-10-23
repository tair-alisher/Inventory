using System;
using System.Collections.Generic;

namespace Inventory.DAL.Interfaces
{
    public interface IPartialRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T Get(int? id);
        IEnumerable<T> Find(Func<T, Boolean> predicate);
    }
}
