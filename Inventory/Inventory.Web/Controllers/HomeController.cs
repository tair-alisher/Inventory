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
    public class HomeController : Controller
    {
        private IEquipmentService EquipmentService;

        public HomeController(IEquipmentService equipmentService)
        {
            EquipmentService = equipmentService;
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            IEnumerable<DivisionEquipmentDTO> structuredEquipment = EquipmentService
                .GetEquipmentByStructure()
                .ToList();

            return View(structuredEquipment);
        }
    }
}