using System.Collections.Generic;

namespace Inventory.BLL.Interfaces
{
    public interface IPartialService<T> where T : class
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        void Dispose();
    }
}
