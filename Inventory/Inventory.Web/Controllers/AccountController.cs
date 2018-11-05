using Inventory.BLL.DTO;
using Inventory.BLL.Interfaces;
using Inventory.BLL.Infrastructure;
using Inventory.Web.Models.Account;
using Inventory.Web.Util;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using System.Web;
using System.Security.Claims;
using Microsoft.AspNet.Identity;

namespace Inventory.Web.Controllers
{
    public class AccountController : Controller
    {
        private IAccountService AccountService;
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return System.Web.HttpContext.Current.GetOwinContext().Authentication;
            }
        }

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
        [ValidateAntiForgeryToken]
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
                    UserDTO userDTO = new UserDTO
                    {
                        UserName = model.UserName,
                        Email = model.Email,
                        Password = model.Password,
                        Role = model.Role
                    };
                    await AccountService.CreateUser(userDTO);

                    return RedirectToAction("Index", "Home");
                }
                catch (UserAlreadyExistsException)
                {
                    ModelState.AddModelError("UserName", $"Пользователь с логином {model.UserName} уже существует.");
                }
                catch (InsecurePasswordException)
                {
                    ModelState.AddModelError("Password", "Пароль должен содержать 8 знаков, включая строчные буквы, цифры и специальный символ.");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Произошла ошибка. Попробуйте еще раз.");
                }
            }

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> Login([Bind(Include = "UserName,Password,RememberMe")]LoginModel model)
        {
            if (ModelState.IsValid)
            {
                UserDTO userDTO = new UserDTO
                {
                    UserName = model.UserName,
                    Password = model.Password
                };

                ClaimsIdentity claim = await AccountService.AuthenticateUser(userDTO);
                if (claim == null)
                {
                    ModelState.AddModelError("", "Неверный логин или пароль.");
                }
                else
                {
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe
                    }, claim);

                    return RedirectToRoute(new
                    {
                        controller = "Home",
                        action = "Index"
                    });
                }
            }
            return View(model);
        }

        public async Task<ActionResult> ChangeEmail()
        {
            UserDTO userDTO = await AccountService.GetUser(User.Identity.GetUserId());
            if (userDTO == null)
                return HttpNotFound();

            ChangeEmailModel model = new ChangeEmailModel
            {
                Email = userDTO.Email
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeEmail([Bind(Include = "UserName,Email")] ChangeEmailModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    UserDTO userDTO = new UserDTO
                    {
                        Id = Guid.Parse(User.Identity.GetUserId()),
                        Email = model.Email
                    };
                    await AccountService.UpdateEmail(userDTO);
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Что-то пошло не так. Попытайтесь еще раз.");
                }
            }

            return View(model);
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ChangePasswordDTO changePasswordDTO = new ChangePasswordDTO
                    {
                        UserId = User.Identity.GetUserId(),
                        OldPassword = model.OldPassword,
                        NewPassword = model.NewPassword
                    };
                    await AccountService.UpdatePassword(changePasswordDTO);

                    return RedirectToRoute(new
                    {
                        controller = "Home",
                        action = "Index"
                    });
                }
                catch (OldPasswordIsWrongException)
                {
                    ModelState.AddModelError("OldPassword", "Пароль неверн.");
                }
                catch (InsecurePasswordException)
                {
                    ModelState.AddModelError("NewPassword", "Пароль должен содержать 8 знаков, включая строчные буквы, цифры и специальный символ.");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Что-то пошло не так. Попытайтесь еще раз.");
                }
            }

            return View(model);
        }

        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToRoute(new
            {
                controller = "Home",
                action = "Index"
            });
        }
    }
}