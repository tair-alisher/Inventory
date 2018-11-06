using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Inventory.Web.Models;
using PagedList.Mvc;
using PagedList;
using System.Net;
using Inventory.BLL.Interfaces;
using Inventory.BLL.Services;
using Inventory.BLL.DTO;
using Inventory.Web.Util;

namespace Inventory.Web.Controllers
{
    public class SearchController : Controller
    {
        private IComponentTypeService ComponentTypeService;
        private IEquipmentTypeService EquipmentTypeService;
        private IEquipmentService EquipmentService;
        private IStatusTypeService StatusTypeService;
        private IRepairPlaceService RepairPlaceService;
        private IHistoryService HistoryService;

        public SearchController(IComponentTypeService componentTypeService, IEquipmentTypeService equipmentTypeService, IEquipmentService equipmentService, IStatusTypeService statusTypeService, IRepairPlaceService repairPlaceService, IHistoryService historyService)
        {
            ComponentTypeService = componentTypeService;
            EquipmentTypeService = equipmentTypeService;
            EquipmentService = equipmentService;
            StatusTypeService = statusTypeService;
            RepairPlaceService = repairPlaceService;
            HistoryService = historyService;
        }

        // Builds the ajax employee search query with pagination
        [HttpPost]
        public ActionResult HistoryFilter(/*string name,*/ int? page, Guid? equipmentId, int? employeeId, Guid? repairPlaceId, Guid? statusTypeId)
        {
            IQueryable<HistoryDTO> historyDTOs = Enumerable.Empty<HistoryDTO>().AsQueryable();

            IQueryable<HistoryVM> historyVMs = WebHistoryMapper.DtoToVm(historyDTOs).AsQueryable();

            //return PartialView(historyVMs.ToPagedList(pageNumber, pageSize));

            //IQueryable<Employee> employees = Enumerable.Empty<Employee>().AsQueryable();

            //name = name.Trim();
            //if (name.Length <= 0)
            //employees = db.Employees.OrderBy(c => c.EmployeeFullName);
            //else
            //    employees = BuildEmployeeSearchQueryByName(name);
            historyVMs = FilterAdditions(historyVMs, equipmentId, employeeId, repairPlaceId, statusTypeId);
            //employees = FilterAdditions(employees, positionId, departmentId, administrationId, divisionId);

            historyVMs = AddIncludes(historyVMs);
            //employees = AddIncludes(employees);

            string view = "";
            //if (User.IsInRole("admin"))
                view = "~/Views/Search/Histories.cshtml";
            //else if (User.IsInRole("manager"))
            //    view = "~/Views/Search/ManagerEmployeeFilter.cshtml";
            //else
            //    view = "~/Views/Search/EmployeeFilter.cshtml";

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return PartialView(view, historyVMs.ToPagedList(pageNumber, pageSize));
        }

        // Adds filters to the search query
        private IQueryable<HistoryVM> FilterAdditions(IQueryable<HistoryVM> query, Guid? equipmentId, int? employeeId, Guid? repairPlaceId, Guid? statusTypeId)
        {
            if (equipmentId != null)
                query = query.Where(e => e.EquipmentId == equipmentId);

            if (employeeId != null)
                query = query.Where(e => e.EmployeeId == employeeId);

            if (repairPlaceId != null)
                query = query.Where(e => e.RepairPlaceId == repairPlaceId);

            if (statusTypeId != null)
                query = query.Where(e => e.StatusTypeId == statusTypeId);

            //if (administrationId != null)
            //{
            //    List<int> departmentIds = GetDepartmentIds("administration", administrationId);
            //    query = query.Where(e => departmentIds.Contains(e.DepartmentId));
            //}

            //if (divisionId != null)
            //{
            //    List<int> departmentIds = GetDepartmentIds("division", divisionId);
            //    query = query.Where(e => departmentIds.Contains(e.DepartmentId));
            //}

            return query;
        }

        // Adds relationships Position and Department to the (Employee) query
        private IQueryable<HistoryVM> AddIncludes(IQueryable<HistoryVM> query)
        {
            IQueryable<HistoryVM> employeeMatches = query
                .OrderBy(c => c.Id)
                .Include(d => d.Equipment)
                .Include(e => e.Employee)
                .Include(r => r.RepairPlace)
                .Include(s => s.StatusType);

            return employeeMatches;
        }


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
            IEnumerable<EquipmentTypeDTO> equipmentTypeDTOs = EquipmentTypeService.GetAll()
                .ToList()
                .Where(d => words.All(d.Name.ToLower().Contains));

            IEnumerable<EquipmentTypeVM> equipmentTypeVMs = WebEquipmentTypeMapper
               .DtoToVm(equipmentTypeDTOs);

            return equipmentTypeVMs;
        }

        private IEnumerable<EquipmentVM> BuildEquipmentSearchQuery(params string[] words)
        {
            IEnumerable<EquipmentDTO> equipmentDTOs = EquipmentService.GetAll()
                .ToList()
                .Where(d => words.All(d.InventNumber.ToLower().Contains));

            IEnumerable<EquipmentVM> equipmentVMs = WebEquipmentMapper
               .DtoToVm(equipmentDTOs);

            return equipmentVMs;
        }

        private IEnumerable<ComponentTypeVM> BuildComponentTypeSearchQuery(params string[] words)
        {
            IEnumerable<ComponentTypeDTO> componentTypeDTOs = ComponentTypeService.GetAll()
                .ToList()
                .Where(d => words.All(d.Name.ToLower().Contains));

            IEnumerable<ComponentTypeVM> componentTypeVMs = WebComponentTypeMapper
               .DtoToVm(componentTypeDTOs);

            return componentTypeVMs;
        }

        private IEnumerable<StatusTypeVM> BuildStatusTypeSearchQuery(params string[] words)
        {
            IEnumerable<StatusTypeDTO> statusTypeDTOs = StatusTypeService.GetAll()
                .ToList()
                .Where(d => words.All(d.Name.ToLower().Contains));

            IEnumerable<StatusTypeVM> statusTypeVMs = WebStatusTypeMapper
               .DtoToVm(statusTypeDTOs);

            return statusTypeVMs;
        }

        private IEnumerable<RepairPlaceVM> BuildRepairPlaceSearchQuery(params string[] words)
        {
            IEnumerable<RepairPlaceDTO> repairPlaceDTOs = RepairPlaceService.GetAll()
                .ToList()
                .Where(d => words.All(d.Name.ToLower().Contains));

            IEnumerable<RepairPlaceVM> repairPlaceVMs = WebRepairPlaceMapper
               .DtoToVm(repairPlaceDTOs);

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