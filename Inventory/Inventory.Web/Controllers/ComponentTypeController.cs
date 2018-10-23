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
    public class ComponentTypeController : Controller
    {
        private IComponentTypeService ComponentTypeService;
        public ComponentTypeController(IComponentTypeService componentTypeService)
        {
            ComponentTypeService = componentTypeService;
        }

        public ActionResult Index()
        {
            IEnumerable<ComponentTypeDTO> componentTypeDTOs = ComponentTypeService
                .GetAll()
                .ToList();
            IEnumerable<ComponentTypeVM> componentTypeVMs = WebComponentTypeMapper
                .DtoToVm(componentTypeDTOs);

            return View(componentTypeVMs);
        }

        public ActionResult Details(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ComponentTypeDTO componentTypeDTO = ComponentTypeService.Get((Guid)id);
            if (componentTypeDTO == null)
                return HttpNotFound();

            ComponentTypeVM componentTypeVM = WebComponentTypeMapper
                .DtoToVm(componentTypeDTO);

            return View(componentTypeVM);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name")] ComponentTypeVM componentTypeVM)
        {
            if (ModelState.IsValid)
            {
                ComponentTypeDTO componentTypeDTO = WebComponentTypeMapper
                    .VmToDto(componentTypeVM);
                ComponentTypeService
                    .Add(componentTypeDTO);

                return RedirectToAction("Index");
            }

            return View();
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ComponentTypeDTO componentTypeDTO = ComponentTypeService.Get((Guid)id);
            if (componentTypeDTO == null)
                return HttpNotFound();

            ComponentTypeVM componentTypeVM = WebComponentTypeMapper.DtoToVm(componentTypeDTO);

            return View(componentTypeVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] ComponentTypeVM componentTypeVM)
        {
            if (ModelState.IsValid)
            {
                ComponentTypeDTO componentTypeDTO = WebComponentTypeMapper.VmToDto(componentTypeVM);
                ComponentTypeService.Update(componentTypeDTO);

                return RedirectToAction("Index");
            }

            return View();
        }

        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            try { ComponentTypeService.Delete(id); }
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