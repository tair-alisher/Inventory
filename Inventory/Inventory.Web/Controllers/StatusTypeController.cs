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
    public class StatusTypeController : Controller
    {
        private IStatusTypeService StatusTypeService;
        public StatusTypeController(IStatusTypeService statusTypeService)
        {
            StatusTypeService = statusTypeService;
        }

        // GET: StatusType
        public ActionResult Index()
        {
            IEnumerable<StatusTypeDTO> statusTypeDTOs = StatusTypeService
                .GetAll()
                .ToList();
            IEnumerable<StatusTypeVM> statusTypeVMs = WebStatusTypeMapper
                .DtoToVm(statusTypeDTOs);
            return View(statusTypeVMs);
        }

        public ActionResult Details(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            StatusTypeDTO statusTypeDTO = StatusTypeService.Get((Guid)id);
            if (statusTypeDTO == null)
                return HttpNotFound();

            StatusTypeVM statusTypeVM = WebStatusTypeMapper
                .DtoToVm(statusTypeDTO);
            return View(statusTypeVM);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name")] StatusTypeVM statusTypeVM)
        {
            if (ModelState.IsValid)
            {
                StatusTypeDTO statusTypeDTO = WebStatusTypeMapper
                    .VmToDto(statusTypeVM);
                StatusTypeService
                    .Add(statusTypeDTO);
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            StatusTypeDTO statusTypeDTO = StatusTypeService.Get((Guid)id);
            if (statusTypeDTO == null)
                return HttpNotFound();

            StatusTypeVM statusTypeVM = WebStatusTypeMapper.DtoToVm(statusTypeDTO);
            return View(statusTypeVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] StatusTypeVM statusTypeVM)
        {
            if (ModelState.IsValid)
            {
                StatusTypeDTO statusTypeDTO = WebStatusTypeMapper.VmToDto(statusTypeVM);
                StatusTypeService.Update(statusTypeDTO);

                return RedirectToAction("Index");
            }

            return View();
        }

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StatusTypeDTO statusTypeDTO = StatusTypeService.Get((Guid)id);
            if (statusTypeDTO == null)
                return HttpNotFound();

            StatusTypeVM statusTypeVM = WebStatusTypeMapper.DtoToVm(statusTypeDTO);
            return View(statusTypeVM);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            try
            {
                StatusTypeService.Delete(id);
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