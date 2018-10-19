using Inventory.BLL.DTO;
using Inventory.BLL.Infrastructure;
using Inventory.BLL.Interfaces;
using Inventory.Web.Models;
using Inventory.Web.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Inventory.Web.Controllers
{
    public class EquipmentController : Controller
    {
        private IEquipmentService EquipmentService;
        private IEquipmentTypeService EquipmentTypeService;
        private IEmployeeService EmployeeService;
        private IEquipmentEmployeeRelationService EquipmentEmployeeRelationService;

        public EquipmentController
            (IEquipmentService equipmentService,
            IEmployeeService employeeService,
            IEquipmentTypeService equipmentTypeService,
            IEquipmentEmployeeRelationService equipmentEmployeeRelationService)
        {
            EquipmentService = equipmentService;
            EmployeeService = employeeService;
            EquipmentEmployeeRelationService = equipmentEmployeeRelationService;
            EquipmentTypeService = equipmentTypeService;
        }

        public ActionResult Index()
        {
            IEnumerable<EquipmentDTO> equipmentDTOs = EquipmentService
                .GetAll()
                .ToList();
            IEnumerable<EquipmentVM> equipmentVMs = WebEquipmentMapper.DtoToVm(equipmentDTOs);

            return View(equipmentVMs.ToList());
        }

        public ActionResult Details(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            EquipmentDTO equipmentDTO = EquipmentService.Get((Guid)id);
            if (equipmentDTO == null)
                return HttpNotFound();

            ViewBag.OwnerHistory = EquipmentService.GetOwnerHistory((Guid)id);
            EquipmentVM equipmentVM = WebEquipmentMapper.DtoToVm(equipmentDTO);

            return View(equipmentVM);
        }

        public ActionResult Create()
        {
            ViewBag.EquipmentTypeId = new SelectList(
                EquipmentTypeService.GetAll(),
                "Id",
                "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="EquipmentTypeId,InventNumber,QRCode,Price,Supplier")] EquipmentVM equipmentVM)
        {
            if (ModelState.IsValid) {
                EquipmentDTO equipmentDTO = WebEquipmentMapper.VmToDto(equipmentVM);
                Guid equipmentId = EquipmentService.AddAndGetId(equipmentDTO);

                string[] employeeIds = Request.Form.GetValues("employeeId[]");

                if (!(employeeIds.Length <= 0)) {
                    try {
                        EquipmentEmployeeRelationService.Create(equipmentId, employeeIds);
                        if (Request.Form["OwnerId"] != null)
                            EquipmentEmployeeRelationService
                                .SetOwner(equipmentId, int.Parse(Request.Form["ownerId"]));
                    }
                    catch { EquipmentEmployeeRelationService.DeleteEquipmentRelations(equipmentId); }
                }

                return RedirectToAction("Edit", new { id = equipmentId });
            }

            ViewBag.EquipmentTypeId = new SelectList(
                    EquipmentTypeService.GetAll(),
                    "Id",
                    "Name",
                    equipmentVM.EquipmentTypeId);

            return View(equipmentVM);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            EquipmentDTO equipmentDTO = EquipmentService.Get((Guid)id);
            if (equipmentDTO == null)
                return HttpNotFound();

            EquipmentVM equipmentVM = WebEquipmentMapper.DtoToVm(equipmentDTO);

            ViewBag.EquipmentTypeId = new SelectList(
                EquipmentTypeService.GetAll(),
                "Id",
                "Name",
                equipmentVM.EquipmentTypeId);
            ViewBag.OwnerHistory = EquipmentService.GetOwnerHistory((Guid)id);
            

            return View(equipmentVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,EquipmentTypeId,InventNumber,QRCode,Price,Supplier")] EquipmentVM equipmentVM)
        {
            if (ModelState.IsValid)
            {
                EquipmentDTO equipmentDTO = WebEquipmentMapper
                    .VmToDto(equipmentVM);
                EquipmentService.Update(equipmentDTO);

                string[] employeeIds = Request.Form.GetValues("employeeId[]");

                if (employeeIds.Length <= 0) {
                    EquipmentEmployeeRelationService
                        .DeleteEquipmentRelations(equipmentVM.Id);
                }
                else {
                    try {
                        EquipmentEmployeeRelationService
                            .UpdateEquipmentRelations(equipmentVM.Id, employeeIds);
                        if (Request.Form["ownerId"] != null)
                            EquipmentEmployeeRelationService
                                .ResetOwner(equipmentVM.Id, int.Parse(Request.Form["ownerId"]));
                        else
                            EquipmentEmployeeRelationService
                                .UnsetOwner(equipmentVM.Id);
                    }
                    catch {
                        EquipmentEmployeeRelationService
                            .DeleteEquipmentRelations(equipmentVM.Id);
                    }
                }
            }

            EquipmentDTO dto = EquipmentService.Get(equipmentVM.Id);
            equipmentVM = WebEquipmentMapper.DtoToVm(dto);

            ViewBag.EquipmentTypeId = new SelectList(
                EquipmentTypeService.GetAll(),
                "Id",
                "Name",
                equipmentVM.EquipmentTypeId);
            ViewBag.OwnerHistory = EquipmentService.GetOwnerHistory((Guid)equipmentVM.Id);

            return View(equipmentVM);
        }

        public ActionResult OwnerHistory(Guid? equipmentId)
        {
            if (equipmentId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ViewBag.OwnerHistory = EquipmentService.GetOwnerHistory((Guid)equipmentId);
            ViewBag.EquipmentId = equipmentId;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OwnerHistory()
        {
            try
            {
                Guid equipmentId = Guid.Parse(Request.Form["equipmentId"]);
                string[] employeeIds = Request.Form.GetValues("employeeIds[]");
                int ownerId = int.Parse(Request.Form["ownerId"]);
            }
            catch (ArgumentNullException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View();
        }

        public ActionResult EditRelation()
        {
            Guid equipmentId;
            int employeeId;
            try {
                equipmentId = Guid.Parse(Request.QueryString["equipmentId"]);
                employeeId = int.Parse(Request.QueryString["employeeId"]);
            }
            catch (ArgumentNullException) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            EquipmentEmployeeRelationDTO relationDTO;
            try {
                relationDTO = EquipmentEmployeeRelationService
                    .GetByEquipmentAndEmployee(equipmentId, employeeId);
            }
            catch (NotFoundException) { return HttpNotFound(); }

            EquipmentEmployeeRelationVM relationVM = WebEquipmentEmployeeMapper
                    .DtoToVm(relationDTO);

            return View(relationVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRelation([Bind(Include = "Id,CreatedAt,UpdatedAt")] EquipmentEmployeeRelationVM relationVM)
        {
            if (ModelState.IsValid)
            {
                EquipmentEmployeeRelationDTO relationDTO = WebEquipmentEmployeeMapper
                    .VmToDto(relationVM);
                EquipmentEmployeeRelationService.UpdateDates(relationDTO);

                return RedirectToAction("Index");
            }

            return View();
        }

        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            try { EquipmentService.Delete(id); }
            catch (NotFoundException) { return HttpNotFound(); }
            catch (HasRelationsException) { return Content("Удаление невозможно."); }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FindEmployees(string name)
        {
            const int MaxNumberOfWordsInFullName = 3;

            name = name.Trim();
            if (name.Length <= 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            string[] nameParts = name.Split(' ');
            int wordsAmount;
            if (nameParts.Length < MaxNumberOfWordsInFullName)
                wordsAmount = nameParts.Length;
            else
                wordsAmount = MaxNumberOfWordsInFullName;

            IEnumerable<OwnerInfoDTO> employees = Enumerable.Empty<OwnerInfoDTO>();
            if (wordsAmount == 1)
                employees = EmployeeService.GetEmployeesByName(nameParts.First());
            else if (wordsAmount == 2)
                employees = EmployeeService.GetEmployeesByName(nameParts[0], nameParts[1]);
            else if (wordsAmount == 3)
                employees = EmployeeService.GetEmployeesByName(nameParts[0], nameParts[1], nameParts[2]);

            return PartialView(employees);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}