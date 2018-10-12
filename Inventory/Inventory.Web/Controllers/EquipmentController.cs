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
            ViewBag.EmployeeId = new SelectList(
                EmployeeService.GetAll(),
                "EmployeeId",
                "EmployeeFullName");
            ViewBag.EquipmentTypeId = new SelectList(
                EquipmentTypeService.GetAll(),
                "Id",
                "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="EquipmentTypeId,InventNumber,QRCode,Price,Supplier")] EquipmentVM equipmentVM, string EmployeeId)
        {
            if (ModelState.IsValid)
            {
                EquipmentDTO equipmentDTO = WebEquipmentMapper.VmToDto(equipmentVM);
                Guid equipmentId = EquipmentService.AddAndGetId(equipmentDTO);

                if (!string.IsNullOrEmpty(EmployeeId))
                    EquipmentEmployeeRelationService.Create(equipmentId, int.Parse(EmployeeId));

                return RedirectToAction("Index");
            }

            ViewBag.EmployeeId = new SelectList(
                    EmployeeService.GetAll(),
                    "EmployeeId",
                    "EmployeeFullName",
                    equipmentVM.EquipmentTypeId);
            ViewBag.EquipmentTypeId = new SelectList(
                    EquipmentTypeService.GetAll(),
                    "Id",
                    "Name",
                    EmployeeId);

            return View(equipmentVM);
        }

        public ActionResult Edit(Guid id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit()
        {
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

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}