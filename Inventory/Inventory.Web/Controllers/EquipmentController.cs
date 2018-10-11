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
    public class EquipmentController : Controller
    {
        private IEquipmentService EquipmentService;
        private IEquipmentTypeService EquipmentTypeService;
		private IEquipmentEmployeeRelationService EquipmentEmployeeRelationService;
        private IEmployeeService EmployeeService;
        MapperConfiguration config;

        public EquipmentController
            (IEquipmentService equipmentService,
            IEmployeeService employeeService,
			IEquipmentEmployeeRelationService equipmentEmployeeRelationService,
            IEquipmentTypeService equipmentTypeService)
        {
            EquipmentService = equipmentService;
            EmployeeService = employeeService;
			EquipmentEmployeeRelationService = equipmentEmployeeRelationService;
            EquipmentTypeService = equipmentTypeService;
        }

        public ActionResult Index()
        {
            IEnumerable<EquipmentDTO> equipmentDTOs = EquipmentService.GetAll();

            config = new MapperConfiguration(cfg => cfg.CreateMap<EquipmentDTO, EquipmentVM>());
            IEnumerable<EquipmentVM> equipmentVMs = config.CreateMapper().Map<IEnumerable<EquipmentDTO>, List<EquipmentVM>>(equipmentDTOs);

            return View(equipmentVMs);
        }

        public ActionResult Details(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            EquipmentDTO equipmentDTO = EquipmentService.Get((Guid)id);
            if (equipmentDTO == null)
                return HttpNotFound();

            config = new MapperConfiguration(cfg => cfg.CreateMap<EquipmentDTO, EquipmentVM>());
            EquipmentVM equipmentVM = config.CreateMapper().Map<EquipmentDTO, EquipmentVM>(equipmentDTO);

            var employee = EquipmentService.GetEquipmentOwner((Guid)id);
            ViewBag.Employee = employee;

            return View(equipmentVM);
        }

        public ActionResult Create()
        {
            ViewBag.EmployeeId = new SelectList(
                EmployeeService.GetAll(),
                "EmployeeId",
                "EmployeeFullName"
                );
            ViewBag.EquipmentTypeId = new SelectList(
                EquipmentTypeService.GetAll(),
                "Id",
                "Name"
                );

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="EquipmentTypeId,InventNumber,QRCode,Price,Supplier")] EquipmentVM equipmentVM, string EmployeeId)
        {
            if (ModelState.IsValid)
            {
                config = new MapperConfiguration(cfg => cfg.CreateMap<EquipmentVM, EquipmentDTO>());
                EquipmentDTO equipmentDTO = config.CreateMapper().Map<EquipmentDTO>(equipmentVM);

                Guid createdEquipmentId = EquipmentService.AddAndGetId(equipmentDTO);

				EquipmentEmployeeRelationDTO equipmentEmployeeRelation = new EquipmentEmployeeRelationDTO
				{
					EquipmentId = createdEquipmentId,
					EmployeeId = int.Parse(EmployeeId)
				};
				EquipmentEmployeeRelationService.Add(equipmentEmployeeRelation);

                return RedirectToAction("Index");
            }

            ViewBag.EmployeeId = new SelectList(
                    EmployeeService.GetAll(),
                    "EmployeeId",
                    "EmployeeFullName",
                    equipmentVM.EquipmentTypeId
                );
            ViewBag.EquipmentTypeId = new SelectList(
                    EquipmentTypeService.GetAll(),
                    "Id",
                    "Name",
                    EmployeeId
                );

            return View(equipmentVM);
        }

		public ActionResult Edit(Guid id)
		{

			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit()
		{
			return View();
		}

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            try { EquipmentService.Delete(id); }
			catch (NotFoundException) { return HttpNotFound(); }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}