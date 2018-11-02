using Inventory.DAL.Identity;
using System;
using System.Threading.Tasks;

namespace Inventory.DAL.Interfaces
{
    public interface IAccountWorker : IDisposable
    {
        ApplicationUserManager UserManager { get; }
        ApplicationRoleManager RoleManager { get; }
        Task SaveAsync();
        void Save();
    }
}
