using Inventory.BLL.Interfaces;
using System;
using System.Web.Mvc;

namespace Inventory.Web.Controllers
{
    public class BaseController : Controller
    {
        public IComponentService ComponentService;
        public IComponentTypeService ComponentTypeService;
        public IEquipmentService EquipmentService;
        public const int PageSize = 10;

        public BaseController(
            IComponentService componentService,
            IComponentTypeService componentTypeService,
            IEquipmentService equipmentService)
        {
            ComponentService = componentService;
            ComponentTypeService = componentTypeService;
            EquipmentService = equipmentService;
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