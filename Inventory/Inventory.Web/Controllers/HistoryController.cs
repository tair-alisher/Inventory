using Inventory.BLL.DTO;
using Inventory.BLL.Infrastructure;
using Inventory.BLL.Interfaces;
using Inventory.Web.Models;
using Inventory.Web.Util;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Inventory.Web.Controllers
{
    public class HistoryController : Controller
    {
        private IHistoryService HistoryService;
        private IStatusTypeService StatusTypeService;
        private IRepairPlaceService RepairPlaceService;
        private IEquipmentService EquipmentService;
        private IEmployeeService EmployeeService;

        public HistoryController(IHistoryService historyService, IStatusTypeService statusTypeService, IRepairPlaceService repairPlaceService, IEquipmentService equipmentService, IEmployeeService employeeService)
        {
            HistoryService = historyService;
            StatusTypeService = statusTypeService;
            RepairPlaceService = repairPlaceService;
            EquipmentService = equipmentService;
            EmployeeService = employeeService;
        }

        [Authorize(Roles = "admin, manager, user")]
        [OutputCache(Duration = 30, Location = OutputCacheLocation.Downstream)]
        public ActionResult Index(int? page, string equipmentId, string employeeId, string repairPlaceId, string statusTypeId)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            List<EquipmentSelectModel> equipmentSelectModel = new List<EquipmentSelectModel>();
            var equipmentWithInventNumber = EquipmentService.GetAll();

            foreach (var item in equipmentWithInventNumber)
            {
                equipmentSelectModel.Add(
                    new EquipmentSelectModel
                    {
                        Id = item.Id,
                        TypeAndInventNumber = item.EquipmentType.Name + " (Номер: " + item.InventNumber + ")"
                    }
                );
            }

            List<StatusTypeVM> statusTypeVMs = WebStatusTypeMapper.DtoToVm(StatusTypeService.GetAll()).ToList();
            ViewBag.StatusTypeId = new SelectList(statusTypeVMs,"Id","Name");

            List<RepairPlaceVM> repairPlaceVMs = WebRepairPlaceMapper.DtoToVm(RepairPlaceService.GetAll()).ToList();
            ViewBag.RepairPlaceId = new SelectList(repairPlaceVMs, "Id", "Name");

            ViewBag.EquipmentId = new SelectList(equipmentSelectModel,"Id","TypeAndInventNumber");

            ViewBag.EmployeeId = new SelectList(EmployeeService.GetAll(),"EmployeeId","EmployeeFullName");

            IEnumerable<HistoryDTO> historyDTOs = HistoryService.GetAll().ToList();

            IEnumerable<HistoryVM> historyVMs = WebHistoryMapper.DtoToVm(historyDTOs);

            var filteredHistories = (!String.IsNullOrEmpty(equipmentId)) || (!String.IsNullOrEmpty(employeeId)) || (!String.IsNullOrEmpty(repairPlaceId)) || (!String.IsNullOrEmpty(statusTypeId))
              ? HistoryService.Filter(pageNumber, pageSize, historyDTOs, equipmentId, employeeId, repairPlaceId, statusTypeId).OrderBy(x => x.Employee.EmployeeFullName)
              : null;

            return filteredHistories == null ? View(historyVMs.ToPagedList(pageNumber, pageSize)) : View(WebHistoryMapper.DtoToVm(filteredHistories).ToPagedList(pageNumber, pageSize));
        }

        [Authorize(Roles = "admin, manager")]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            HistoryDTO historyDTO = HistoryService.Get((Guid)id);
            if (historyDTO == null)
                return HttpNotFound();

            HistoryVM historyVM = WebHistoryMapper.DtoToVm(historyDTO);

            return View(historyVM);
        }

        [Authorize(Roles = "admin, manager, user")]
        public ActionResult Create()
        {
            List<EquipmentSelectModel> equipmentSelectModel = new List<EquipmentSelectModel>();
            var eqipmentWithInventNumber = EquipmentService.GetAll();

            foreach (var item in eqipmentWithInventNumber)
            {
                equipmentSelectModel.Add(
                    new EquipmentSelectModel
                    {
                        Id = item.Id,
                        TypeAndInventNumber = item.EquipmentType.Name + " (Номер: " + item.InventNumber + ")"
                    }
                );
            }
            ViewBag.ChangeDateNow = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");

            ViewBag.StatusTypeId = new SelectList(
                StatusTypeService.GetAll(),
                "Id",
                "Name");

            ViewBag.RepairPlaceId = new SelectList(
                RepairPlaceService.GetAll(),
                "Id",
                "Name");

            ViewBag.EquipmentId = new SelectList(
                equipmentSelectModel,
                "Id",
                "TypeAndInventNumber");

            ViewBag.EmployeeId = new SelectList(
                EmployeeService.GetAll(),
                "EmployeeId",
                "EmployeeFullName");

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin, manager, user")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EquipmentId,ChangeDate,EmployeeId,RepairPlaceId,StatusTypeId,Comments")] HistoryVM historyVM)
        {
            List<EquipmentSelectModel> equipmentSelectModel = new List<EquipmentSelectModel>();
            var eqipmentWithInventNumber = EquipmentService.GetAll();

            foreach (var item in eqipmentWithInventNumber)
            {
                equipmentSelectModel.Add(
                    new EquipmentSelectModel
                    {
                        Id = item.Id,
                        TypeAndInventNumber = item.EquipmentType.Name + " (Номер: " + item.InventNumber + ")"
                    }
                );
            }

            if (ModelState.IsValid)
            {
                HistoryDTO historyDTO = WebHistoryMapper.VmToDto(historyVM);
                HistoryService.Add(historyDTO);

                return RedirectToAction("Index");
            }

            ViewBag.ChangeDateNow = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");

            ViewBag.StatusTypeId = new SelectList(
                StatusTypeService.GetAll(),
                "Id",
                "Name");

            ViewBag.RepairPlaceId = new SelectList(
                RepairPlaceService.GetAll(),
                "Id",
                "Name");

            ViewBag.EquipmentId = new SelectList(
               equipmentSelectModel,
                "Id",
                "TypeAndInventNumber");

            ViewBag.EmployeeId = new SelectList(
                EmployeeService.GetAll(),
                "EmployeeId",
                "EmployeeFullName");

            return View(historyVM);
        }

        [Authorize(Roles = "admin, manager")]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            HistoryDTO historyDTO = HistoryService.Get((Guid)id);
            if (historyDTO == null)
                return HttpNotFound();

            HistoryVM historyVM = WebHistoryMapper.DtoToVm(historyDTO);

            List<EquipmentSelectModel> equipmentSelectModel = new List<EquipmentSelectModel>();
            var eqipmentWithInventNumber = EquipmentService.GetAll();

            foreach (var item in eqipmentWithInventNumber)
            {
                equipmentSelectModel.Add(
                    new EquipmentSelectModel
                    {
                        Id = item.Id,
                        TypeAndInventNumber = item.EquipmentType.Name + " (Номер: " + item.InventNumber + ")"
                    }
                );
            }
            ViewBag.ChangeDateNow = ((DateTime)historyVM.ChangeDate).ToString("dd.MM.yyyy HH:mm:ss");

            ViewBag.StatusTypeId = new SelectList(
                StatusTypeService.GetAll(),
                "Id",
                "Name",
                historyVM.StatusTypeId);

            ViewBag.RepairPlaceId = new SelectList(
                RepairPlaceService.GetAll(),
                "Id",
                "Name",
                historyVM.RepairPlaceId);

            ViewBag.EquipmentId = new SelectList(
                equipmentSelectModel,
                "Id",
                "TypeAndInventNumber",
                historyVM.EquipmentId);

            ViewBag.EmployeeId = new SelectList(
                EmployeeService.GetAll(),
                "EmployeeId",
                "EmployeeFullName",
                historyVM.EmployeeId);

            return View(historyVM);
        }

        [HttpPost]
        [Authorize(Roles = "admin, manager")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,EquipmentId,ChangeDate,EmployeeId,RepairPlaceId,StatusTypeId,Comments")] HistoryVM historyVM)
        {
            if (ModelState.IsValid)
            {
                HistoryDTO historyDTO = WebHistoryMapper.VmToDto(historyVM);
                HistoryService.Update(historyDTO);

                return RedirectToAction("Index");
            }

            List<EquipmentSelectModel> equipmentSelectModel = new List<EquipmentSelectModel>();
            var eqipmentWithInventNumber = EquipmentService.GetAll();

            foreach (var item in eqipmentWithInventNumber)
            {
                equipmentSelectModel.Add(
                    new EquipmentSelectModel
                    {
                        Id = item.Id,
                        TypeAndInventNumber = item.EquipmentType.Name + " (Номер: " + item.InventNumber + ")"
                    }
                );
            }

            ViewBag.ChangeDateNow = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");

            ViewBag.StatusTypeId = new SelectList(
                StatusTypeService.GetAll(),
                "Id",
                "Name",
                historyVM.StatusTypeId);

            ViewBag.RepairPlaceId = new SelectList(
                RepairPlaceService.GetAll(),
                "Id",
                "Name",
                historyVM.RepairPlaceId);

            ViewBag.EquipmentId = new SelectList(
                equipmentSelectModel,
                "Id",
                "TypeAndInventNumber",
                historyVM.EquipmentId);

            ViewBag.EmployeeId = new SelectList(
                EmployeeService.GetAll(),
                "EmployeeId",
                "EmployeeFullName",
                historyVM.EmployeeId);

            return View(historyVM);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id)
        {
            try
            {
                HistoryService.Delete(id);
            }
            catch (NotFoundException) { return HttpNotFound(); }
            catch (HasRelationsException) { return Content("Удаление невозможно."); }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}