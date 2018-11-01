using Inventory.DAL.Repositories;
using System;
using System.Threading.Tasks;

namespace Inventory.DAL.Interfaces
{
    public interface IAccountWorker : IDisposable
    {
        ApplicationUserManager ApplicationUserManager { get; }
        ApplicationRoleManager ApplicationRoleManager { get; }
        Task SaveAsync();
        void Save();
    }
}
