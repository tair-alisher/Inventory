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
    public class EquipmentTypeController : BaseController
    {
        public EquipmentTypeController(IEquipmentTypeService equipmentTypeService) : base(equipmentTypeService) { }

        [Authorize(Roles = "admin, manager")]
        [OutputCache(Duration = 30, Location = OutputCacheLocation.Downstream)]
        public ActionResult AjaxEquipmentTypeList(int? page)
        {
            IEnumerable<EquipmentTypeDTO> equipmentTypeDTOs = EquipmentTypeService.GetListOrderedByName().ToList();
            IEnumerable<EquipmentTypeVM> equipmentTypeVMs = Mapper.Map<IEnumerable<EquipmentTypeVM>>(equipmentTypeDTOs);

            return PartialView(equipmentTypeVMs.ToPagedList(page ?? 1, PageSize));
        }

        [Authorize(Roles = "admin, manager")]
        [OutputCache(Duration = 30, Location = OutputCacheLocation.Downstream)]
        public ActionResult Index(int? page)
        {
            IEnumerable<EquipmentTypeDTO> equipmentTypeDTOs = EquipmentTypeService.GetListOrderedByName().ToList();
            IEnumerable<EquipmentTypeVM> equipmentTypeVMs = Mapper.Map<IEnumerable<EquipmentTypeVM>>(equipmentTypeDTOs);

            return View(equipmentTypeVMs.ToPagedList(page ?? 1, PageSize));
        }

        [Authorize(Roles = "admin, manager")]
        public ActionResult Details(Guid? id)
        {
            try
            {
                EquipmentTypeDTO equipmentTypeDTO = EquipmentTypeService.Get(id);
                EquipmentTypeVM equipmentTypeVM = Mapper.Map<EquipmentTypeVM>(equipmentTypeDTO);

                return View(equipmentTypeVM);
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

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin, manager")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name")] EquipmentTypeVM equipmentTypeVM)
        {
            if (ModelState.IsValid)
            {
                EquipmentTypeDTO equipmentTypeDTO = Mapper.Map<EquipmentTypeDTO>(equipmentTypeVM);
                EquipmentTypeService.Add(equipmentTypeDTO);

                return RedirectToAction("Index");
            }

            return View();
        }

        [Authorize(Roles = "admin, manager")]
        public ActionResult Edit(Guid? id)
        {
            try
            {
                EquipmentTypeDTO equipmentTypeDTO = EquipmentTypeService.Get(id);
                EquipmentTypeVM equipmentTypeVM = Mapper.Map<EquipmentTypeVM>(equipmentTypeDTO);

                return View(equipmentTypeVM);
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
        public ActionResult Edit([Bind(Include = "Id,Name")] EquipmentTypeVM equipmentTypeVM)
        {
            if (ModelState.IsValid)
            {
                EquipmentTypeDTO equipmentTypeDTO = Mapper.Map<EquipmentTypeDTO>(equipmentTypeVM);
                EquipmentTypeService.Update(equipmentTypeDTO);

                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id)
        {
            try { EquipmentTypeService.Delete(id); }
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