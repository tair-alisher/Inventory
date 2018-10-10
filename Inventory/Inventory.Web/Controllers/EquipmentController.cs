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
        private IEquipmentTypeService EquipmentTypeService;
		private IEquipmentEmployeeService EquipmentEmployeeService;
        private IEmployeeService EmployeeService;
        MapperConfiguration config;

        public EquipmentController
            (IEquipmentService equipmentService,
            IEmployeeService employeeService,
			IEquipmentEmployeeService equipmentEmployeeService,
            IEquipmentTypeService equipmentTypeService)
        {
            EquipmentService = equipmentService;
            EmployeeService = employeeService;
			EquipmentEmployeeService = equipmentEmployeeService;
            EquipmentTypeService = equipmentTypeService;
        }

        public ActionResult Index()
        {
            IEnumerable<EquipmentDTO> equipmentDTOs = EquipmentService.GetAll();

            config = new MapperConfiguration(cfg => cfg.CreateMap<EquipmentDTO, EquipmentVM>());
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

            config = new MapperConfiguration(cfg => cfg.CreateMap<EquipmentDTO, EquipmentVM>());
            EquipmentVM equipmentVM = config.CreateMapper().Map<EquipmentDTO, EquipmentVM>(equipmentDTO);

            var employee = EquipmentService.GetEquipmentOwner(id);
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

				EquipmentEmployeeDTO equipmentEmployeeRelation = new EquipmentEmployeeDTO
				{
					EquipmentId = createdEquipmentId,
					EmployeeId = int.Parse(EmployeeId)
				};
				EquipmentEmployeeService.Add(equipmentEmployeeRelation);

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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            try
            {
                EquipmentService.Delete(id);
            } catch (Exception ex)
            {
                return Content(ex.Message);
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}