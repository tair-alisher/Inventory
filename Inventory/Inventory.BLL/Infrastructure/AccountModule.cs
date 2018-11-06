using Inventory.DAL.Interfaces;
using Inventory.DAL.Repositories;
using Ninject.Modules;

namespace Inventory.BLL.Infrastructure
{
    public class AccountModule : NinjectModule
    {
        private string connectionString;

        public AccountModule(string connection)
        {
            connectionString = connection;
        }

        public override void Load()
        {
            Bind<IAccountWorker>().To<AccountWorker>().WithConstructorArgument(connectionString);
        }
    }
}
