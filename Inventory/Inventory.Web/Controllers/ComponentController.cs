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
    public class ComponentController : BaseController
    {
        public ComponentController(
            IComponentService compService,
            IComponentTypeService compTypeService,
            IEquipmentService equipService) : base(compService, compTypeService, equipService) { }

        [Authorize(Roles = "admin")]
        [OutputCache(Duration = 30, Location = OutputCacheLocation.Downstream)]
        public ActionResult AjaxComponentList(int? page)
        {
            int pageNumber = (page ?? 1);

            ViewBag.ComponentTypeId = GetComponentTypeIdSelectList();
            ViewBag.ModelName = GetModelNameSelectList();
            ViewBag.Name = GetComponentNameSelectList();

            FilterParamsDTO parameters = new FilterParamsDTO
            {
                ComponentTypeId = Request.QueryString["componentTypeId"],
                ModelName = Request.QueryString["modelName"],
                Name = Request.QueryString["Name"]
            };

            IEnumerable<ComponentDTO> filteredComponentDTOList = ComponentService.GetFilteredList(parameters).ToList();
            IEnumerable<ComponentVM> filteredComponentVMList = Mapper.Map<IEnumerable<ComponentVM>>(filteredComponentDTOList);

            return View(filteredComponentVMList.ToPagedList(pageNumber, PageSize));

        }

        [Authorize(Roles = "admin")]
        [OutputCache(Duration = 30, Location = OutputCacheLocation.Downstream)]
        public ActionResult Index(int? page)
        {
            int pageNumber = (page ?? 1);

            IEnumerable<ComponentDTO> componentDTOs = ComponentService
                .GetAll()
                .ToList();
            IEnumerable<ComponentVM> componentVMs = Mapper.Map<IEnumerable<ComponentVM>>(componentDTOs);

            ViewBag.ComponentTypeId = GetComponentTypeIdSelectList();
            ViewBag.ModelName = GetModelNameSelectList();
            ViewBag.Name = GetComponentNameSelectList();

            FilterParamsDTO parameters = new FilterParamsDTO
            {
                ComponentTypeId = Request.QueryString["componentTypeId"],
                ModelName = Request.QueryString["modelName"],
                Name = Request.QueryString["Name"]
            };

            IEnumerable<ComponentDTO> filteredComponentDTOList = ComponentService.GetFilteredList(parameters).ToList();
            IEnumerable<ComponentVM> filteredComponentVMList = Mapper.Map<IEnumerable<ComponentVM>>(filteredComponentDTOList);

            return View(filteredComponentVMList.ToPagedList(pageNumber, PageSize));
        }

        [Authorize(Roles = "admin")]
        public ActionResult Details(Guid? id)
        {
            try
            {
                ComponentDTO componentDTO = ComponentService.Get((Guid)id);
                ComponentVM componentVM = Mapper.Map<ComponentVM>(componentDTO);

                return View(componentVM);
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
            ViewBag.ComponentTypeId = GetComponentTypeIdSelectList();

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ComponentTypeId,ModelName,Name,Description,Price,InventNumber,Supplier")] ComponentVM componentVM)
        {
            if (ModelState.IsValid)
            {
                ComponentDTO componentDTO = Mapper.Map<ComponentDTO>(componentVM);
                ComponentService.Add(componentDTO);

                return RedirectToAction("Index");
            }
            ViewBag.ComponentTypeId = GetComponentTypeIdSelectList();

            return View(componentVM);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Edit(Guid? id)
        {
            try
            {
                ComponentDTO componentDTO = ComponentService.Get(id);
                ComponentVM componentVM = Mapper.Map<ComponentVM>(componentDTO);
                ViewBag.ComponentTypeId = GetComponentTypeIdSelectList(componentVM.ComponentTypeId);

                return View(componentVM);
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
        public ActionResult Edit([Bind(Include = "Id,ComponentTypeId,ModelName,Name,Description,Price,InventNumber,Supplier")] ComponentVM componentVM)
        {
            if (ModelState.IsValid)
            {
                ComponentDTO componentDTO = Mapper.Map<ComponentDTO>(componentVM);
                ComponentService.Update(componentDTO);

                return RedirectToAction("Index");
            }
            
            ViewBag.ComponentTypeId = GetComponentTypeIdSelectList(componentVM.ComponentTypeId);

            return View(componentVM);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id)
        {
            try
            {
                ComponentService.Delete(id);
            }
            catch (NotFoundException)
            {
                return HttpNotFound();
            }
            catch (HasRelationsException)
            {
                return Content("Удаление невозможно.");
            }

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

            List<ComponentVM> componentVMs = Mapper.Map<IEnumerable<ComponentVM>>(componentDTOs).ToList();

            return PartialView(componentVMs);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}