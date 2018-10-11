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

            Bind<IComponentTypeService>().To<ComponentTypeService>();

            Bind<IEmployeeService>().To<EmployeeService>();
        }
    }
}