using IEqEmpRelServ = Inventory.BLL.Interfaces.IEquipmentEmployeeRelationService;
using IEqComRelServ = Inventory.BLL.Interfaces.IEquipmentComponentRelationService;
using System.Web.Mvc;
using System;
using System.Net;
using Inventory.Web.Models;
using Inventory.Web.Util;
using Inventory.BLL.DTO;

namespace Inventory.Web.Controllers
{
    public class RelationController : Controller
    {
        private IEqEmpRelServ EqEmpService;
        private IEqComRelServ EqComService;

        public RelationController(IEqEmpRelServ eqEmpService, IEqComRelServ eqComService)
        {
            EqEmpService = eqEmpService;
            EqComService = eqComService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateOwnerHistory(Guid? equipmentId)
        {
            if (equipmentId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            string[] employeeIds = Request.Form.GetValues("employeeId[]") ?? new string[0];
            if (employeeIds.Length <= 0)
                EqEmpService.DeleteRelationsByEquipmentId((Guid)equipmentId);
            else
            {
                try
                {
                    EqEmpService.UpdateEquipmentRelations((Guid)equipmentId, employeeIds);
                    if (Request.Form["ownerId"] != null)
                        EqEmpService.ResetOwner((Guid)equipmentId, int.Parse(Request.Form["ownerId"]));
                    else
                        EqEmpService.UnsetOwner((Guid)equipmentId);
                }
                catch
                {
                    EqEmpService.DeleteRelationsByEquipmentId((Guid)equipmentId);
                }
            }

            return RedirectToRoute(new
            {
                controller = "Equipment",
                action = "OwnerHistory",
                equipmentId
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateComponents(Guid? equipmentId)
        {
            if (equipmentId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            string[] componentIds = Request.Form.GetValues("componentId[]") ?? new string[0];
            if (componentIds.Length <= 0)
                EqComService.DeleteRelationsByEquipmentId((Guid)equipmentId);
            else
            {
                try
                {
                    EqComService.UpdateEquipmentRelations((Guid)equipmentId, componentIds);
                }
                catch
                {
                    EqComService.DeleteRelationsByEquipmentId((Guid)equipmentId);
                }
            }

            return RedirectToRoute(new
            {
                controller = "Equipment",
                action = "Components",
                equipmentId
            });
        }

        public ActionResult EditEquipmentEmployeeRelation()
        {
            Guid equipmentId;
            int employeeId;
            try
            {
                equipmentId = Guid.Parse(Request.QueryString["equipmentId"]);
                employeeId = int.Parse(Request.QueryString["employeeId"]);
            }
            catch (ArgumentNullException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            EquipmentEmployeeRelationDTO relationDTO;
            try
            {
                relationDTO = EqEmpService.GetByEquipmentAndEmployee(equipmentId, employeeId);
            }
            catch (DllNotFoundException)
            {
                return HttpNotFound();
            }

            EquipmentEmployeeRelationVM relationVM = WebEquipmentEmployeeMapper.DtoToVm(relationDTO);

            return View(relationVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEquipmentEmployeeRelation([Bind(Include = "Id,EquipmentId,CreatedAt,UpdatedAt")] EquipmentEmployeeRelationVM relationVM)
        {
            if (ModelState.IsValid)
            {
                EquipmentEmployeeRelationDTO relationDTO = WebEquipmentEmployeeMapper
                    .VmToDto(relationVM);
                EqEmpService.UpdateDates(relationDTO);
            }
            else
                ModelState.AddModelError(null, "Что-то пошло не так. Не удалось сохранить изменения");

            return View(relationVM);
        }
    }
}