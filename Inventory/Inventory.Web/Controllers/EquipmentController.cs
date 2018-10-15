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
        public ActionResult Create([Bind(Include="EquipmentTypeId,InventNumber,QRCode,Price,Supplier")] EquipmentVM equipmentVM, string[] employeeId)
        {
            if (ModelState.IsValid)
            {
                EquipmentDTO equipmentDTO = WebEquipmentMapper.VmToDto(equipmentVM);
                Guid equipmentId = EquipmentService.AddAndGetId(equipmentDTO);

                if (!(employeeId.Length <= 0))
                {
                    try { EquipmentEmployeeRelationService.Create(equipmentId, employeeId); }
                    catch { EquipmentEmployeeRelationService.DeleteEquipmentRelations(equipmentId); }
                }

                return RedirectToAction("Index");
            }

            ViewBag.EquipmentTypeId = new SelectList(
                    EquipmentTypeService.GetAll(),
                    "Id",
                    "Name",
                    equipmentVM.EquipmentTypeId);

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