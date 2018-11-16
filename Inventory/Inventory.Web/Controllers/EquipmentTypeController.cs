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
using System.Web.Mvc;
using System.Web.UI;

namespace Inventory.Web.Controllers
{
    public class EquipmentTypeController : Controller
    {
        private IEquipmentTypeService EquipmentTypeService;
        public EquipmentTypeController(IEquipmentTypeService equipmentTypeService)
        {
            EquipmentTypeService = equipmentTypeService;
        }

        [OutputCache(Duration = 30, Location = OutputCacheLocation.Downstream)]
        public ActionResult AjaxEquipmentTypeList(int? page)
        {
            int pageSize = 1;
            int pageNumber = (page ?? 1);

            IEnumerable<EquipmentTypeDTO> equipmentTypeDTOs = EquipmentTypeService
              .GetAll()
              .ToList();
            IEnumerable<EquipmentTypeVM> equipmentTypeVMs = WebEquipmentTypeMapper
                .DtoToVm(equipmentTypeDTOs);

            return PartialView(equipmentTypeVMs.OrderBy(s => s.Name).ToPagedList(pageNumber, pageSize));
        }

        [OutputCache(Duration = 30, Location = OutputCacheLocation.Downstream)]
        public ActionResult Index(int? page)
        {
            int pageSize = 1;
            int pageNumber = (page ?? 1);

            IEnumerable<EquipmentTypeDTO> equipmentTypeDTOs = EquipmentTypeService
                .GetAll()
                .ToList();
            IEnumerable<EquipmentTypeVM> equipmentTypeVMs = WebEquipmentTypeMapper
                .DtoToVm(equipmentTypeDTOs);

            return View(equipmentTypeVMs.OrderBy(s => s.Name).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Details(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            EquipmentTypeDTO equipmentTypeDTO = EquipmentTypeService.Get((Guid)id);
            if (equipmentTypeDTO == null)
                return HttpNotFound();

            EquipmentTypeVM equipmentTypeVM = WebEquipmentTypeMapper
                .DtoToVm(equipmentTypeDTO);

            return View(equipmentTypeVM);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name")] EquipmentTypeVM equipmentTypeVM)
        {
            if (ModelState.IsValid)
            {
                EquipmentTypeDTO equipmentTypeDTO = WebEquipmentTypeMapper
                    .VmToDto(equipmentTypeVM);
                EquipmentTypeService
                    .Add(equipmentTypeDTO);

                return RedirectToAction("Index");
            }

            return View();
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            EquipmentTypeDTO equipmentTypeDTO = EquipmentTypeService.Get((Guid)id);
            if (equipmentTypeDTO == null)
                return HttpNotFound();

            EquipmentTypeVM equipmentTypeVM = WebEquipmentTypeMapper.DtoToVm(equipmentTypeDTO);

            return View(equipmentTypeVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] EquipmentTypeVM equipmentTypeVM)
        {
            if (ModelState.IsValid)
            {
                EquipmentTypeDTO equipmentTypeDTO = WebEquipmentTypeMapper.VmToDto(equipmentTypeVM);
                EquipmentTypeService.Update(equipmentTypeDTO);

                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpPost]
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