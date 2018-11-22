using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Inventory.Web.Models;
using Inventory.BLL.Interfaces;
using Inventory.BLL.DTO;
using AutoMapper;

namespace Inventory.Web.Controllers
{
    public class SearchController : Controller
    {
        private IComponentTypeService ComponentTypeService;
        private IComponentService ComponentService;
        private IEquipmentTypeService EquipmentTypeService;
        private IEquipmentService EquipmentService;
        private IStatusTypeService StatusTypeService;
        private IRepairPlaceService RepairPlaceService;
        private IHistoryService HistoryService;

        public SearchController(IComponentTypeService componentTypeService, IComponentService componentService, IEquipmentTypeService equipmentTypeService, IEquipmentService equipmentService, IStatusTypeService statusTypeService, IRepairPlaceService repairPlaceService, IHistoryService historyService)
        {
            ComponentTypeService = componentTypeService;
            ComponentService = componentService;
            EquipmentTypeService = equipmentTypeService;
            EquipmentService = equipmentService;
            StatusTypeService = statusTypeService;
            RepairPlaceService = repairPlaceService;
            HistoryService = historyService;
        }

        [Authorize(Roles = "admin, manager, user")]
        public ActionResult AdminSearch(string title, string type)
        {
            string view = "~/Views/Search/";
            string[] words = title.ToLower().Split(' ');

            // returns not found if input string is empty
            if (title.Trim().Length <= 0)
                return RedirectToAction("NotFoundResult");

            if (type == "equipmentType")
            {
                List<EquipmentTypeVM> equipmentTypeVMs = BuildEquipmentTypeSearchQuery(words).ToList();
                BindSearchResults(equipmentTypeVMs, ref view, "EquipmentTypes.cshtml");
            }
            else if (type == "equipment")
            {
                List<EquipmentVM> equipmentVMs = BuildEquipmentSearchQuery(words).ToList();
                BindSearchResults(equipmentVMs, ref view, "Equipments.cshtml");
            }
            else if (type == "componentType")
            {
                List<ComponentTypeVM> componentTypeVMs = BuildComponentTypeSearchQuery(words).ToList();
                BindSearchResults(componentTypeVMs, ref view, "ComponentTypes.cshtml");
            }
            else if (type == "component")
            {
                List<ComponentVM> componentVMs = BuildComponentSearchQuery(words).ToList();
                BindSearchResults(componentVMs, ref view, "Components.cshtml");
            }
            else if (type == "statusType")
            {
                List<StatusTypeVM> statusTypeVMs = BuildStatusTypeSearchQuery(words).ToList();
                BindSearchResults(statusTypeVMs, ref view, "StatusTypes.cshtml");
            }
            else if (type == "repairPlace")
            {
                List<RepairPlaceVM> repairPlaceVMs = BuildRepairPlaceSearchQuery(words).ToList();
                BindSearchResults(repairPlaceVMs, ref view, "RepairPlaces.cshtml");
            }
            return PartialView(view);
        }

        private IEnumerable<EquipmentTypeVM> BuildEquipmentTypeSearchQuery(params string[] words)
        {
            IEnumerable<EquipmentTypeDTO> equipmentTypeDTOs = EquipmentTypeService
                .GetAll()
                .ToList()
                .Where(d => words.All(d.Name.ToLower().Contains));

            IEnumerable<EquipmentTypeVM> equipmentTypeVMs = Mapper.Map<IEnumerable<EquipmentTypeVM>>(equipmentTypeDTOs);

            return equipmentTypeVMs;
        }

        private IEnumerable<EquipmentVM> BuildEquipmentSearchQuery(params string[] words)
        {
            IEnumerable<EquipmentDTO> equipmentDTOs = EquipmentService.GetAll()
                .ToList()
                .Where(d => words.All(d.InventNumber.ToLower().Contains));

            IEnumerable<EquipmentVM> equipmentVMs = Mapper.Map<IEnumerable<EquipmentVM>>(equipmentDTOs);

            return equipmentVMs;
        }

        private IEnumerable<ComponentTypeVM> BuildComponentTypeSearchQuery(params string[] words)
        {
            IEnumerable<ComponentTypeDTO> componentTypeDTOs = ComponentTypeService.GetAll()
                .ToList()
                .Where(d => words.All(d.Name.ToLower().Contains));

            IEnumerable<ComponentTypeVM> componentTypeVMs = Mapper.Map<IEnumerable<ComponentTypeVM>>(componentTypeDTOs);

            return componentTypeVMs;
        }

        private IEnumerable<ComponentVM> BuildComponentSearchQuery(params string[] words)
        {
            IEnumerable<ComponentDTO> componentDTOs = ComponentService.GetAll()
                .ToList()
                .Where(d => d.InventNumber != null && words.All(d.InventNumber.ToLower().Contains));

            IEnumerable<ComponentVM> componentVMs = Mapper.Map<IEnumerable<ComponentVM>>(componentDTOs);

            return componentVMs;
        }

        private IEnumerable<StatusTypeVM> BuildStatusTypeSearchQuery(params string[] words)
        {
            IEnumerable<StatusTypeDTO> statusTypeDTOs = StatusTypeService.GetAll()
                .ToList()
                .Where(d => words.All(d.Name.ToLower().Contains));

            IEnumerable<StatusTypeVM> statusTypeVMs = Mapper.Map<IEnumerable<StatusTypeVM>>(statusTypeDTOs);

            return statusTypeVMs;
        }

        private IEnumerable<RepairPlaceVM> BuildRepairPlaceSearchQuery(params string[] words)
        {
            IEnumerable<RepairPlaceDTO> repairPlaceDTOs = RepairPlaceService.GetAll()
                .ToList()
                .Where(d => words.All(d.Name.ToLower().Contains));

            IEnumerable<RepairPlaceVM> repairPlaceVMs = Mapper.Map<IEnumerable<RepairPlaceVM>>(repairPlaceDTOs);

            return repairPlaceVMs;
        }

        // Binds entity search results and entity view
        private void BindSearchResults<T>(List<T> items, ref string view, string entityView)
        {
            if (items.Count <= 0)
            {
                view += "NotFound.cshtml";
            }
            else
            {
                ViewBag.Items = items;
                view += entityView;
            }
        }

        // Forms not found partial view
        public ActionResult NotFoundResult()
        {
            return PartialView("~/Views/Error/NotFoundError.cshtml");
        }
    }
}