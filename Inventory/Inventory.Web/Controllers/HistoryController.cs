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

        public ActionResult AjaxHistoryList(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            IEnumerable<HistoryDTO> historyDTOs = HistoryService.GetAll().ToList();

            IEnumerable<HistoryVM> historyVMs = WebHistoryMapper.DtoToVm(historyDTOs);

            return PartialView(historyVMs.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Index(int? page, string repairPlaceId, string statusId)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);

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

            List<StatusTypeVM> statusTypeVMs = WebStatusTypeMapper.DtoToVm(StatusTypeService.GetAll()).ToList();
            ViewBag.StatusTypeId = statusTypeVMs;

            List<RepairPlaceVM> repairPlaceVMs = WebRepairPlaceMapper.DtoToVm(RepairPlaceService.GetAll()).ToList();
            ViewBag.RepairPlaceId = repairPlaceVMs;

            ViewBag.EquipmentId = equipmentSelectModel;

            ViewBag.EmployeeId = EmployeeService.GetAll().ToList(); 

            IEnumerable<HistoryDTO> historyDTOs = HistoryService.GetAll().ToList();

            IEnumerable<HistoryVM> historyVMs = WebHistoryMapper.DtoToVm(historyDTOs);

            return View(historyVMs.ToPagedList(pageNumber, pageSize));
        }

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