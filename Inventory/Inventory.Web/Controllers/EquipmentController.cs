using AutoMapper;
using Inventory.BLL.DTO;
using Inventory.BLL.Interfaces;
using Inventory.Web.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;

namespace Inventory.Web.Controllers
{
    public class EquipmentController : Controller
    {
        private IEquipmentService EquipmentService;

        public EquipmentController(IEquipmentService equipmentService)
        {
            EquipmentService = equipmentService;
        }

        public ActionResult Index()
        {
            IEnumerable<EquipmentDTO> equipmentDTOs = EquipmentService.GetAll();

            var config = new MapperConfiguration(cfg => cfg.CreateMap<EquipmentDTO, EquipmentVM>());
            IEnumerable<EquipmentVM> equipmentVMs = config.CreateMapper().Map<IEnumerable<EquipmentDTO>, List<EquipmentVM>>(equipmentDTOs);

            return View(equipmentVMs);
        }

        public ActionResult Details(Guid id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            EquipmentDTO equipmentDTO = EquipmentService.Get(id);
            if (equipmentDTO == null)
                return HttpNotFound();

            var config = new MapperConfiguration(cfg => cfg.CreateMap<EquipmentDTO, EquipmentVM>());
            EquipmentVM equipmentVM = config.CreateMapper().Map<EquipmentDTO, EquipmentVM>(equipmentDTO);

            var employee = EquipmentService.GetEquipmentOwner(id);
            ViewBag.Employee = employee;

            return View(equipmentVM);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}