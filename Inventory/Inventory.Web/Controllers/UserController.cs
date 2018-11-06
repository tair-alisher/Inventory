using Inventory.BLL.Interfaces;
using Inventory.Web.Models.User;
using Inventory.Web.Util;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

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

        public ActionResult Index()
        {
            var userDTOs = UserService.GetAllUsers().ToList();
            var userVMs = WebUserMapper.DtoToVm(userDTOs).ToList();

            return View(userVMs);
        }

        public async Task<ActionResult> ChangeRole(string userId)
        {
            if (userId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ChangeRoleModel model = new ChangeRoleModel
            {
                UserId = userId,
                Role = await UserService.GetUserRole(userId)
            };

            var userRole = UserService.GetUserRole(userId.ToString());
            var roles = AccountService.GetAllRoles().ToList();

            ViewBag.Role = new SelectList(
                roles,
                "Name",
                "Description",
                model.Role);

            return View(model);
        }

        public ActionResult ChangePassword(Guid userId)
        {
            return View();
        }
    }
}