using AutoMapper;
using Inventory.BLL.DTO;
using Inventory.BLL.Infrastructure;
using Inventory.BLL.Interfaces;
using Inventory.Web.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;

namespace Inventory.Web.Controllers
{
    public class ComponentTypeController : Controller
    {
		private IComponentTypeService ComponentTypeService;
		MapperConfiguration config;
		public ComponentTypeController(IComponentTypeService componentTypeService)
		{
			ComponentTypeService = componentTypeService;
		}

        public ActionResult Index()
        {
			IEnumerable<ComponentTypeDTO> componentTypeDTOs = ComponentTypeService
				.GetAll();
			config = new MapperConfiguration(cfg => cfg.CreateMap<ComponentTypeDTO, ComponentTypeVM>());

			IEnumerable<ComponentTypeVM> componentTypeVMs = config
				.CreateMapper()
				.Map<IEnumerable<ComponentTypeVM>>(componentTypeDTOs);

            return View(componentTypeVMs);
        }

		public ActionResult Details(Guid id)
		{
			if (id == null)
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			ComponentTypeDTO componentTypeDTO = ComponentTypeService.Get(id);
			if (componentTypeDTO == null)
				return HttpNotFound();

			config = new MapperConfiguration(cfg => cfg.CreateMap<ComponentTypeDTO, ComponentTypeVM>());

			ComponentTypeVM componentTypeVM = config.CreateMapper().Map<ComponentTypeVM>(componentTypeDTO);

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
				config = new MapperConfiguration(cfg => cfg.CreateMap<ComponentTypeVM, ComponentTypeDTO>());
				ComponentTypeDTO componentTypeDTO = config.CreateMapper().Map<ComponentTypeDTO>(componentTypeVM);

				ComponentTypeService.Add(componentTypeDTO);

				return RedirectToAction("index");
			}

			return View();
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(Guid id)
		{
			try { ComponentTypeService.Delete(id); }
			catch (NotFoundException) { return HttpNotFound(); }

			return RedirectToAction("Index");
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}
	}
}