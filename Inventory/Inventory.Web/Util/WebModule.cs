using Inventory.BLL.Interfaces;
using Inventory.BLL.Services;
using Ninject.Modules;

namespace Inventory.Web.Util
{
    public class WebModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IEquipmentService>().To<EquipmentService>();
            Bind<IEquipmentTypeService>().To<EquipmentTypeService>();
            Bind<IEquipmentEmployeeRelationService>().To<EquipmentEmployeeRelationService>();

            Bind<IComponentService>().To<ComponentService>();
            Bind<IComponentTypeService>().To<ComponentTypeService>();
            Bind<IEquipmentComponentRelationService>().To<EquipmentComponentRelationService>();

            Bind<IEmployeeService>().To<EmployeeService>();

            Bind<IStatusTypeService>().To<StatusTypeService>();
            Bind<IRepairPlaceService>().To<RepairPlaceService>();
            Bind<IHistoryService>().To<HistoryService>();


            Bind<IAccountService>().To<AccountService>();
        }
    }
}