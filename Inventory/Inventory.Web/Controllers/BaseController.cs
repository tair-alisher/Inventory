using Inventory.BLL.Interfaces;
using System;
using System.Web.Mvc;

namespace Inventory.Web.Controllers
{
    public class BaseController : Controller
    {
        public IEquipmentService EquipmentService;
        public IEquipmentTypeService EquipmentTypeService;
        public IEmployeeService EmployeeService;
        public IComponentService ComponentService;
        public IComponentTypeService ComponentTypeService;
        public const int PageSize = 10;

        public BaseController(
            IEquipmentService equipmentService,
            IEquipmentTypeService equipmentTypeService,
            IEmployeeService employeeService)
        {
            EquipmentService = equipmentService;
            EquipmentTypeService = equipmentTypeService;
            EmployeeService = employeeService;
        }

        public BaseController(
            IComponentService componentService,
            IComponentTypeService componentTypeService,
            IEquipmentService equipmentService)
        {
            ComponentService = componentService;
            ComponentTypeService = componentTypeService;
            EquipmentService = equipmentService;
        }

        public BaseController(IEquipmentTypeService equipmentTypeService)
        {
            EquipmentTypeService = equipmentTypeService;
        }

        public BaseController(IComponentTypeService compTypeService)
        {
            ComponentTypeService = compTypeService;
        }

        public SelectList GetEquipmentTypeIdSelectList(Guid? selectedValue = null)
        {
            return new SelectList(EquipmentTypeService.GetAll(), "Id", "Name", selectedValue);
        }

        public SelectList GetComponentTypeIdSelectList(Guid? selectedValue = null)
        {
            return new SelectList(ComponentTypeService.GetAll(), "Id", "Name", selectedValue);
        }

        public SelectList GetModelNameSelectList(string selectedValue = null)
        {
            return new SelectList(ComponentService.GetAll(), "ModelName", "ModelName", selectedValue);
        }

        public SelectList GetComponentNameSelectList(string selectedValue = null)
        {
            return new SelectList(ComponentService.GetAll(), "Name", "Name");
        }
    }
}