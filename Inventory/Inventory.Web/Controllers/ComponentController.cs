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
    public class ComponentController : Controller
    {
        private IComponentService ComponentService;
        private IComponentTypeService ComponentTypeService;
        private IEquipmentService EquipmentService;
        private IEquipmentComponentRelationService EquipmentComponentRelationService;
        public ComponentController(
            IComponentService componentService,
            IComponentTypeService componentTypeService,
            IEquipmentService equipmentService,
            IEquipmentComponentRelationService equipmentComponentRelationService
            )
        {
            ComponentService = componentService;
            ComponentTypeService = componentTypeService;
            EquipmentService = equipmentService;
            EquipmentComponentRelationService = equipmentComponentRelationService;
        }

        public ActionResult Index()
        {
            IEnumerable<ComponentDTO> componentDTOs = ComponentService
                .GetAll()
                .ToList();
            IEnumerable<ComponentVM> componentVMs = WebComponentMapper.DtoToVm(componentDTOs);

            return View(componentVMs.ToList());
        }

        public ActionResult Details(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ComponentDTO componentDTO = ComponentService.Get((Guid)id);
            if (componentDTO == null)
                return HttpNotFound();

            ComponentVM componentVM = WebComponentMapper.DtoToVm(componentDTO);

            return View(componentVM);
        }

        public ActionResult Create()
        {
            ViewBag.ComponentTypeId = new SelectList(
                ComponentTypeService.GetAll(),
                "Id",
                "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ComponentTypeId,ModelName,Name,Description,Price,InventNumber,Supplier")] ComponentVM componentVM)
        {
            if (ModelState.IsValid)
            {
                ComponentDTO componentDTO = WebComponentMapper.VmToDto(componentVM);
                ComponentService.Add(componentDTO);

                return RedirectToAction("Index");
            }

            ViewBag.ComponentTypeId = new SelectList(
                ComponentTypeService.GetAll(),
                "Id",
                "Name");

            return View(componentVM);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ComponentDTO componentDTO = ComponentService.Get((Guid)id);
            if (componentDTO == null)
                return HttpNotFound();

            ComponentVM componentVM = WebComponentMapper.DtoToVm(componentDTO);
            ViewBag.ComponentTypeId = new SelectList(
                ComponentTypeService.GetAll(),
                "Id",
                "Name",
                componentVM.ComponentTypeId);

            return View(componentVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ComponentTypeId,ModelName,Name,Description,Price,InventNumber,Supplier")] ComponentVM componentVM)
        {
            if (ModelState.IsValid)
            {
                ComponentDTO componentDTO = WebComponentMapper.VmToDto(componentVM);
                ComponentService.Update(componentDTO);

                return RedirectToAction("Index");
            }

            ViewBag.ComponentTypeId = new SelectList(
                ComponentTypeService.GetAll(),
                "Id",
                "Name",
                componentVM.ComponentTypeId);

            return View(componentVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id)
        {
            try { ComponentService.Delete(id); }
            catch (NotFoundException) { return HttpNotFound(); }
            catch (HasRelationsException) { return Content("Удаление невозможно."); }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FindComponents(string value, string type)
        {
            value = value.Trim().ToLower();

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