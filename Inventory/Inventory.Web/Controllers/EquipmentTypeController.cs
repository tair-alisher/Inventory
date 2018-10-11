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
    public class EquipmentTypeController : Controller
    {
		private IEquipmentTypeService EquipmentTypeService;
		MapperConfiguration config;
		public EquipmentTypeController(IEquipmentTypeService equipmentTypeService)
		{
			EquipmentTypeService = equipmentTypeService;
		}

        public ActionResult Index()
        {
			IEnumerable<EquipmentTypeDTO> equipmentTypeDTOs = EquipmentTypeService
				.GetAll();
			config = new MapperConfiguration(cfg => cfg.CreateMap<EquipmentTypeDTO, EquipmentTypeVM>());

			IEnumerable<EquipmentTypeVM> equipmentTypeVMs = config
				.CreateMapper()
				.Map<IEnumerable<EquipmentTypeVM>>(equipmentTypeDTOs);

            return View(equipmentTypeVMs);
        }

		public ActionResult Details(Guid? id)
		{
			if (id == null)
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			EquipmentTypeDTO equipmentTypeDTO = EquipmentTypeService.Get((Guid)id);
			if (equipmentTypeDTO == null)
				return HttpNotFound();

			config = new MapperConfiguration(cfg => cfg.CreateMap<EquipmentTypeDTO, EquipmentTypeVM>());
			EquipmentTypeVM equipmentTypeVM = config.CreateMapper().Map<EquipmentTypeVM>(equipmentTypeDTO);

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
				config = new MapperConfiguration(cfg => cfg.CreateMap<EquipmentTypeVM, EquipmentTypeDTO>());
				EquipmentTypeDTO equipmentTypeDTO = config.CreateMapper().Map<EquipmentTypeDTO>(equipmentTypeVM);

				EquipmentTypeService.Add(equipmentTypeDTO);

				return RedirectToAction("Index");
			}

			return View();
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(Guid id)
		{
			try { EquipmentTypeService.Delete(id); }
			catch (NotFoundException) { return HttpNotFound(); }

			return RedirectToAction("Index");
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}
	}
}