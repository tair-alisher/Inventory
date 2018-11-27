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

        [Authorize(Roles = "admin, manager, user")]
        [OutputCache(Duration = 30, Location = OutputCacheLocation.Downstream)]
        public ActionResult AjaxEquipmentList(int? page)
        {
            IEnumerable<EquipmentDTO> equipmentDTOs = EquipmentService.GetAll().ToList();
            IEnumerable<EquipmentVM> equipmentVMs = Mapper.Map<IEnumerable<EquipmentVM>>(equipmentDTOs);

            return PartialView(equipmentVMs.ToPagedList(page ?? 1, PageSize));
        }

        [Authorize(Roles = "admin, manager, user")]
        [OutputCache(Duration = 30, Location = OutputCacheLocation.Downstream)]
        public ActionResult Index(int? page)
        {
            IEnumerable<EquipmentDTO> equipmentDTOs = EquipmentService.GetAll().ToList();
            IEnumerable<EquipmentVM> equipmentVMs = Mapper.Map<IEnumerable<EquipmentVM>>(equipmentDTOs);

            return View(equipmentVMs.ToPagedList(page ?? 1, PageSize));
        }

        [Authorize(Roles = "admin, manager")]
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

        [Authorize(Roles = "admin, manager, user")]
        public ActionResult Create()
        {
            ViewBag.EquipmentTypeId = GetEquipmentTypeIdSelectList();

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin, manager, user")]
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

        [Authorize(Roles = "admin, manager")]
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
        [Authorize(Roles = "admin, manager")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,EquipmentTypeId,InventNumber,QRCode,Price,Supplier")] EquipmentVM equipmentVM)
        {
            if (ModelState.IsValid)
            {
                EquipmentDTO equipmentDTO = Mapper.Map<EquipmentDTO>(equipmentVM);
                EquipmentService.Update(equipmentDTO);

                return RedirectToAction("Index");
            }
            else
                ModelState.AddModelError(null, "Что-то пошло не так. Не удалось сохранить изменения.");

            ViewBag.EquipmentTypeId = GetEquipmentTypeIdSelectList(equipmentVM.EquipmentTypeId);

            return View(equipmentVM);
        }

        [Authorize(Roles = "admin, manager, user")]
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

        [Authorize(Roles = "admin, manager")]
        public ActionResult OwnerInfo(Guid? equipmentId, int employeeId)
        {
            try
            {
                OwnerInfoDTO ownerInfoDTO = EquipmentService.GetOwnerInfo(equipmentId, employeeId);
                OwnerInfoVM ownerInfoVM = Mapper.Map<OwnerInfoVM>(ownerInfoDTO);

                ViewBag.EquipmentId = equipmentId;

                return View(ownerInfoVM);
            }
            catch (ArgumentNullException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [Authorize(Roles = "admin, manager, user")]
        public ActionResult Components(Guid? equipmentId)
        {
            try
            {
                IEnumerable<ComponentDTO> components = EquipmentService.GetComponents(equipmentId).ToList();
                IEnumerable<ComponentVM> componentVMs = Mapper.Map<IEnumerable<ComponentVM>>(components);

                ViewBag.EquipmentId = equipmentId;

                return View(componentVMs);
            }
            catch (ArgumentNullException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
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
        [Authorize(Roles = "admin, manager")]
        [ValidateAntiForgeryToken]
        public ActionResult FindEmployees(string name)
        {
            IEnumerable<OwnerInfoDTO> ownerDTOList = EmployeeService.ValidateNameAndGetEmployeesByName(name).ToList();
            IEnumerable<OwnerInfoVM> ownerVMList = Mapper.Map<IEnumerable<OwnerInfoVM>>(ownerDTOList);

            return PartialView(ownerVMList.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}