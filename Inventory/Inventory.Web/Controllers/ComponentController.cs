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
    public class ComponentController : Controller
    {
        private IComponentService ComponentService;
        private IComponentTypeService ComponentTypeService;
        private IEquipmentService EquipmentService;
        public ComponentController(
            IComponentService componentService,
            IComponentTypeService componentTypeService,
            IEquipmentService equipmentService
            )
        {
            ComponentService = componentService;
            ComponentTypeService = componentTypeService;
            EquipmentService = equipmentService;
        }

        [Authorize(Roles = "admin")]
        [OutputCache(Duration = 30, Location = OutputCacheLocation.Downstream)]
        public ActionResult AjaxComponentList(int? page, string componentTypeId, string modelName, string name)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            IEnumerable<ComponentDTO> componentDTOs = ComponentService
                .GetAll()
                .ToList();
            IEnumerable<ComponentVM> componentVMs = WebComponentMapper.DtoToVm(componentDTOs);

            ViewBag.ComponentTypeId = new SelectList(ComponentTypeService.GetAll(), "Id", "Name");
            ViewBag.ModelName = new SelectList(ComponentService.GetAll(), "ModelName", "ModelName");
            ViewBag.Name = new SelectList(ComponentService.GetAll(), "Name", "Name");

            var filteredComponents = (!String.IsNullOrEmpty(componentTypeId)) || (!String.IsNullOrEmpty(modelName)) || (!String.IsNullOrEmpty(name))
            ? ComponentService.Filter(pageNumber, pageSize, componentDTOs, componentTypeId, modelName, name).OrderBy(x => x.ComponentType.Name)
            : null;

            return filteredComponents == null ? View(componentVMs.ToPagedList(pageNumber, pageSize)) : View(WebComponentMapper.DtoToVm(filteredComponents).ToPagedList(pageNumber, pageSize));

        }

        [Authorize(Roles = "admin")]
        [OutputCache(Duration = 30, Location = OutputCacheLocation.Downstream)]
        public ActionResult Index(int? page, string componentTypeId, string modelName, string name)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            IEnumerable<ComponentDTO> componentDTOs = ComponentService
                .GetAll()
                .ToList();
            IEnumerable<ComponentVM> componentVMs = WebComponentMapper.DtoToVm(componentDTOs);

            ViewBag.ComponentTypeId = new SelectList( ComponentTypeService.GetAll(), "Id", "Name");
            ViewBag.ModelName = new SelectList(ComponentService.GetAll(), "ModelName", "ModelName");
            ViewBag.Name = new SelectList(ComponentService.GetAll(), "Name", "Name");

            var filteredComponents = (!String.IsNullOrEmpty(componentTypeId)) || (!String.IsNullOrEmpty(modelName)) || (!String.IsNullOrEmpty(name))
            ? ComponentService.Filter(pageNumber, pageSize, componentDTOs, componentTypeId, modelName, name).OrderBy(x => x.ComponentType.Name)
            : null;

            return filteredComponents == null ? View(componentVMs.ToPagedList(pageNumber, pageSize)) : View(WebComponentMapper.DtoToVm(filteredComponents).ToPagedList(pageNumber, pageSize));

            //return View(componentVMs.ToPagedList(pageNumber, pageSize));
        }

        [Authorize(Roles = "admin")]
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

        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            ViewBag.ComponentTypeId = new SelectList(
                ComponentTypeService.GetAll(),
                "Id",
                "Name");

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
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

        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id)
        {
            try { ComponentService.Delete(id); }
            catch (NotFoundException) { return HttpNotFound(); }
            catch (HasRelationsException) { return Content("Удаление невозможно."); }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
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