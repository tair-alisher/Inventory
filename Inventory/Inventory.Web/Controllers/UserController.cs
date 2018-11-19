using Inventory.BLL.DTO;
using Inventory.BLL.Interfaces;
using Inventory.Web.Models.User;
using Inventory.Web.Util;
using PagedList;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;

namespace Inventory.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService UserService;
        private readonly IAccountService AccountService;
        
        public UserController(IUserService userService, IAccountService accountService)
        {
            UserService = userService;
            AccountService = accountService;
        }

        [Authorize(Roles = "admin")]
        [OutputCache(Duration = 30, Location = OutputCacheLocation.Downstream)]
        public ActionResult Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            var userDTOs = UserService.GetAllUsers().ToList();
            var userVMs = WebUserMapper.DtoToVm(userDTOs).ToPagedList(pageNumber, pageSize);

            return View(userVMs);
        }

        [Authorize(Roles = "admin")]
        public async Task<ActionResult> ChangeRole(string userId)
        {
            if (userId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var role = await UserService.GetUserRole(userId);
            ChangeRoleModel model = new ChangeRoleModel
            {
                UserId = userId,
                OldRole = role,
                Role = role
            };

            var userRole = UserService.GetUserRole(userId);
            var roles = AccountService.GetAllRoles().ToList();

            ViewBag.Role = new SelectList(
                roles,
                "Name",
                "Description",
                model.Role);

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeRole([Bind(Include = "UserId,OldRole,Role")] ChangeRoleModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ChangeRoleDTO changeRoleDTO = new ChangeRoleDTO
                    {
                        UserId = model.UserId,
                        OldRole = model.OldRole,
                        Role = model.Role
                    };
                    await UserService.ChangeUserRole(changeRoleDTO);
                    TempData["success"] = "Изменения сохранены.";
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Произошла ошибка. Попробуйте еще раз либо обратитесь к администратору.");
                }
            }

            var roles = AccountService.GetAllRoles().ToList();
            ViewBag.Role = new SelectList(
                roles,
                "Name",
                "Description",
                model.Role);

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            try
            {
                await UserService.DeleteUser(id);
            }
            catch (Exception)
            {
                TempData["fail"] = "Произошла ошибка.";
            }

            return RedirectToAction("Index");
        }
    }
}