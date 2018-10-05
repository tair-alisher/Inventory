using AutoMapper;
using Inventory.BLL.DTO;
using Inventory.BLL.Interfaces;
using Inventory.Web.Models;
using System.Collections.Generic;
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

            IEnumerable<EquipmentVM> equipments = config.CreateMapper().Map<IEnumerable<EquipmentDTO>, List<EquipmentVM>>(equipmentDTOs);

            return View(equipments);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}