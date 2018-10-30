using Inventory.BLL.DTO;
using Inventory.BLL.Infrastructure;
using Inventory.BLL.Interfaces;
using Inventory.Web.Models;
using Inventory.Web.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Inventory.Web.Controllers
{
    public class RepairPlaceController : Controller
    {
        private IRepairPlaceService RepairPlaceService;
        public RepairPlaceController(IRepairPlaceService repairPlaceService)
        {
            RepairPlaceService = repairPlaceService;
        }

        // GET: StatusType
        public ActionResult Index()
        {
            IEnumerable<RepairPlaceDTO> repairPlaceDTOs = RepairPlaceService
                .GetAll()
                .ToList();
            IEnumerable<RepairPlaceVM> repairPlaceVMs = WebRepairPlaceMapper
                .DtoToVm(repairPlaceDTOs);
            return View(repairPlaceVMs);
        }

        public ActionResult Details(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            RepairPlaceDTO repairPlaceDTO = RepairPlaceService.Get((Guid)id);
            if (repairPlaceDTO == null)
                return HttpNotFound();

            RepairPlaceVM repairPlaceVM = WebRepairPlaceMapper
                .DtoToVm(repairPlaceDTO);
            return View(repairPlaceVM);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name")] RepairPlaceVM repairPlaceVM)
        {
            if (ModelState.IsValid)
            {
                RepairPlaceDTO repairPlaceDTO = WebRepairPlaceMapper
                    .VmToDto(repairPlaceVM);
                RepairPlaceService
                    .Add(repairPlaceDTO);
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            RepairPlaceDTO repairPlaceDTO = RepairPlaceService.Get((Guid)id);
            if (repairPlaceDTO == null)
                return HttpNotFound();

            RepairPlaceVM repairPlaceVM = WebRepairPlaceMapper.DtoToVm(repairPlaceDTO);
            return View(repairPlaceVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] RepairPlaceVM repairPlaceVM)
        {
            if (ModelState.IsValid)
            {
                RepairPlaceDTO repairPlaceDTO = WebRepairPlaceMapper.VmToDto(repairPlaceVM);
                RepairPlaceService.Update(repairPlaceDTO);

                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id)
        {
            try
            {
                RepairPlaceService.Delete(id);
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

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}