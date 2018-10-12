using Inventory.BLL.DTO;
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
        public ComponentController(
            IComponentService componentService,
            IComponentTypeService componentTypeService
            )
        {
            ComponentService = componentService;
            ComponentTypeService = componentTypeService;
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
                "Name"
                );

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ComponentTypeId,ModelName,Name,Description,Price,InventNumber,Supplier")
        {
            return View();
        }
    }
}