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
            ViewBag.EquipmentId = new SelectList(
                EquipmentService.GetAll(),
                "Id",
                "InventNumber");
            ViewBag.ComponentTypeId = new SelectList(
                ComponentTypeService.GetAll(),
                "Id",
                "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ComponentTypeId,ModelName,Name,Description,Price,InventNumber,Supplier")] ComponentVM componentVM, Guid? EquipmentId)
        {
            if (ModelState.IsValid)
            {
                ComponentDTO componentDTO = WebComponentMapper.VmToDto(componentVM);
                Guid componentId = ComponentService.AddAndGetId(componentDTO);

                if (EquipmentId != null)
                    EquipmentComponentRelationService.Create(componentId, (Guid)EquipmentId);

                return RedirectToAction("Index");
            }

            ViewBag.EquipmentId = new SelectList(
                EquipmentService.GetAll(),
                "Id",
                "InventNumber");
            ViewBag.ComponentTypeId = new SelectList(
                ComponentTypeService.GetAll(),
                "Id",
                "Name");

            return View(componentVM);
        }

        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            try { ComponentService.Delete(id); }
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