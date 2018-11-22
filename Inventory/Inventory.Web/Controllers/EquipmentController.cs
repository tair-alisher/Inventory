using AutoMapper;
using Inventory.BLL.DTO;
using Inventory.BLL.Infrastructure;
using Inventory.BLL.Interfaces;
using Inventory.Web.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.UI;

namespace Inventory.Web.Controllers
{
    public class EquipmentController : BaseController
    {
        public EquipmentController
            (IEquipmentService equipService,
            IEmployeeService empService,
            IEquipmentTypeService equipTypeService) : base(equipService, equipTypeService, empService) { }

        [Authorize(Roles = "admin")]
        [OutputCache(Duration = 30, Location = OutputCacheLocation.Downstream)]
        public ActionResult AjaxEquipmentList(int? page)
        {
            IEnumerable<EquipmentDTO> equipmentDTOs = EquipmentService.GetAll().ToList();
            IEnumerable<EquipmentVM> equipmentVMs = Mapper.Map<IEnumerable<EquipmentVM>>(equipmentDTOs);

            return PartialView(equipmentVMs.ToPagedList(page ?? 1, PageSize));
        }

        [Authorize(Roles = "admin")]
        [OutputCache(Duration = 30, Location = OutputCacheLocation.Downstream)]
        public ActionResult Index(int? page)
        {
            IEnumerable<EquipmentDTO> equipmentDTOs = EquipmentService.GetAll().ToList();
            IEnumerable<EquipmentVM> equipmentVMs = Mapper.Map<IEnumerable<EquipmentVM>>(equipmentDTOs);

            return View(equipmentVMs.ToPagedList(page ?? 1, PageSize));
        }

        [Authorize(Roles = "admin")]
        public ActionResult Details(Guid? id)
        {
            try
            {
                EquipmentDTO equipmentDTO = EquipmentService.Get(id);
                EquipmentVM equipmentVM = Mapper.Map<EquipmentVM>(equipmentDTO);

                IEnumerable<OwnerInfoDTO> ownerHistory = EquipmentService.GetOwnerHistory((Guid)id);
                ViewBag.OwnerHistory = ownerHistory.ToList();

                return View(equipmentVM);
            }
            catch (ArgumentNullException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            catch (NotFoundException)
            {
                return HttpNotFound();
            }
        }

        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            ViewBag.EquipmentTypeId = GetEquipmentTypeIdSelectList();

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EquipmentTypeId,InventNumber,QRCode,Price,Supplier")] EquipmentVM equipmentVM)
        {
            if (ModelState.IsValid)
            {
                EquipmentDTO equipmentDTO = Mapper.Map<EquipmentDTO>(equipmentVM);
                Guid equipmentId = EquipmentService.AddAndGetId(equipmentDTO);

                return RedirectToAction("Index");
            }

            ViewBag.EquipmentTypeId = GetEquipmentTypeIdSelectList(equipmentVM.EquipmentTypeId);

            return View(equipmentVM);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Edit(Guid? id)
        {
            try
            {
                EquipmentDTO equipmentDTO = EquipmentService.Get(id);
                EquipmentVM equipmentVM = Mapper.Map<EquipmentVM>(equipmentDTO);

                ViewBag.EquipmentTypeId = GetEquipmentTypeIdSelectList(equipmentVM.EquipmentTypeId);

                return View(equipmentVM);
            }
            catch (ArgumentNullException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            catch (NotFoundException)
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,EquipmentTypeId,InventNumber,QRCode,Price,Supplier")] EquipmentVM equipmentVM)
        {
            if (ModelState.IsValid)
            {
                EquipmentDTO equipmentDTO = Mapper.Map<EquipmentDTO>(equipmentVM);
                EquipmentService.Update(equipmentDTO);
            }
            else
                ModelState.AddModelError(null, "Что-то пошло не так. Не удалось сохранить изменения.");

            EquipmentDTO dto = EquipmentService.Get(equipmentVM.Id);
            equipmentVM = Mapper.Map<EquipmentVM>(dto);

            ViewBag.EquipmentTypeId = new SelectList(
                EquipmentTypeService.GetAll(),
                "Id",
                "Name",
                equipmentVM.EquipmentTypeId);

            return View(equipmentVM);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Copy(Guid? id)
        {
            try
            {
                EquipmentDTO equipmentDTO = EquipmentService.Get(id);
                EquipmentVM equipmentVM = Mapper.Map<EquipmentVM>(equipmentDTO);
                equipmentVM.InventNumber = null;

                ViewBag.EquipmentTypeId = GetEquipmentTypeIdSelectList(equipmentVM.EquipmentTypeId); ;

                return View("Create", equipmentVM);
            }
            catch (ArgumentNullException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            catch (NotFoundException)
            {
                return HttpNotFound();
            }
        }

        public ActionResult OwnerHistory(Guid? equipmentId)
        {
            if (equipmentId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ViewBag.EquipmentId = equipmentId;
            IEnumerable<OwnerInfoDTO> ownerHistoryDTO = EquipmentService.GetOwnerHistory((Guid)equipmentId).ToList();
            List<OwnerInfoVM> ownerHistoryVM = Mapper.Map<IEnumerable<OwnerInfoVM>>(ownerHistoryDTO).ToList();

            return View(ownerHistoryVM);
        }

        [Authorize(Roles = "admin")]
        public ActionResult OwnerInfo(Guid? equipmentId, int employeeId)
        {
            if (equipmentId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ViewBag.EquipmentId = equipmentId;

            OwnerInfoDTO ownerInfoDTO = EquipmentService.GetOwnerInfo((Guid)equipmentId, employeeId);
            OwnerInfoVM ownerInfoVM = Mapper.Map<OwnerInfoVM>(ownerInfoDTO);

            return View(ownerInfoVM);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Components(Guid? equipmentId)
        {
            if (equipmentId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ViewBag.EquipmentId = equipmentId;

            IEnumerable<ComponentDTO> components = EquipmentService
                .GetComponents((Guid)equipmentId)
                .ToList();
            IEnumerable<ComponentVM> componentVMs = Mapper.Map<IEnumerable<ComponentVM>>(components);

            return View(componentVMs);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id)
        {
            try
            {
                string imagePath = Request.MapPath($"/Content/Images/{id}.jpg");
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
                EquipmentService.Delete(id);
            }
            catch (NotFoundException) { return HttpNotFound(); }
            catch (HasRelationsException) { return Content("Удаление невозможно."); }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
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

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}