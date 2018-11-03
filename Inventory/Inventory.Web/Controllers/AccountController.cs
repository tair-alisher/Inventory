using Inventory.BLL.DTO;
using Inventory.BLL.Interfaces;
using Inventory.BLL.Infrastructure;
using Inventory.Web.Models.Account;
using Inventory.Web.Util;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Inventory.Web.Controllers
{
    public class AccountController : Controller
    {
        private IAccountService AccountService;

        public AccountController(IAccountService accountService)
        {
            AccountService = accountService;
        }

        public ActionResult Index()
        {
            var userDTOs = AccountService.GetAllUsers().ToList();
            var userVMs = WebUserMapper.DtoToVm(userDTOs).ToList();

            return View(userVMs);
        }

        public ActionResult Register()
        {
            ViewBag.Role = new SelectList(
                AccountService.GetAllRoles().ToList(),
                "Name",
                "Description");

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            ViewBag.Role = new SelectList(
                AccountService.GetAllRoles().ToList(),
                "Name",
                "Description");

            if (ModelState.IsValid)
            {
                try
                {
                    bool success = await AccountService.CreateUser(new UserDTO { UserName = model.UserName, Email = model.Email, Password = model.Password, Role = model.Role });
                    return RedirectToAction("Index", "Home");
                }
                catch (UserAlreadyExistsException)
                {
                    ModelState.AddModelError("UserName", $"Пользователь с логином {model.UserName} уже существует.");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Произошла ошибка. Попробуйте еще раз.");
                }
            }

            return View(model);
        }
    }
}