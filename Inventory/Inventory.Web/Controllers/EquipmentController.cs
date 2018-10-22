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
        private IComponentService ComponentService;
        private IEquipmentTypeService EquipmentTypeService;
        private IEmployeeService EmployeeService;
        private IEquipmentEmployeeRelationService EquipmentEmployeeRelationService;

        public EquipmentController
            (IEquipmentService equipmentService,
            IComponentService componentService,
            IEmployeeService employeeService,
            IEquipmentTypeService equipmentTypeService,
            IEquipmentEmployeeRelationService equipmentEmployeeRelationService)
        {
            EquipmentService = equipmentService;
            ComponentService = componentService;
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

            IEnumerable<OwnerInfoDTO> ownerHistory = EquipmentService.GetOwnerHistory((Guid)id);
            ViewBag.OwnerHistory = ownerHistory.ToList();
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
            }

            EquipmentDTO dto = EquipmentService.Get(equipmentVM.Id);
            equipmentVM = WebEquipmentMapper.DtoToVm(dto);

            ViewBag.EquipmentTypeId = new SelectList(
                EquipmentTypeService.GetAll(),
                "Id",
                "Name",
                equipmentVM.EquipmentTypeId);

            return View(equipmentVM);
        }

        public ActionResult OwnerHistory(Guid? equipmentId)
        {
            if (equipmentId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            IEnumerable<OwnerInfoDTO> ownerHistory = EquipmentService.GetOwnerHistory((Guid)equipmentId);
            ViewBag.OwnerHistory = ownerHistory.ToList();
            ViewBag.EquipmentId = equipmentId;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateOwnerHistory(Guid? equipmentId)
        {
            if (equipmentId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            string[] employeeIds = Request.Form.GetValues("employeeId[]") ?? new string[0];
            if (employeeIds.Length <= 0)
                EquipmentEmployeeRelationService.DeleteEquipmentRelations((Guid)equipmentId);
            else
            {
                try
                {
                    EquipmentEmployeeRelationService.UpdateEquipmentRelations((Guid)equipmentId, employeeIds);
                    if (Request.Form["ownerId"] != null)
                        EquipmentEmployeeRelationService.ResetOwner((Guid)equipmentId, int.Parse(Request.Form["ownerId"]));
                    else
                        EquipmentEmployeeRelationService.UnsetOwner((Guid)equipmentId);
                }
                catch
                {
                    EquipmentEmployeeRelationService.DeleteEquipmentRelations((Guid)equipmentId);
                }
            }

            IEnumerable<OwnerInfoDTO> ownerHistory = EquipmentService.GetOwnerHistory((Guid)equipmentId);
            ViewBag.OwnerHistory = ownerHistory.ToList();
            ViewBag.EquipmentId = equipmentId;

            return View("OwnerHistory");
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

        public ActionResult Components(Guid? equipmentId)
        {
            if (equipmentId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ViewBag.EquipmentId = equipmentId;

            IEnumerable<ComponentDTO> components = EquipmentService
                .GetComponents((Guid)equipmentId)
                .ToList();
            IEnumerable<ComponentVM> componentVMs = WebComponentMapper
                .DtoToVm(components);

            return View(componentVMs);
        }

        public ActionResult UpdateComponents(Guid? equipmentId)
        {
            if (equipmentId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            return View("Components");
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

            return PartialView(employees.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FindComponents(string value, string type)
        {
            value = value.Trim();

            List<ComponentDTO> componentDTOs = ComponentService
                .GetComponentsBy(type, value)
                .ToList();

            List<ComponentVM> componentVMs = WebComponentMapper
                .DtoToVm(componentDTOs)
                .ToList();

            return PartialView(componentVMs);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}