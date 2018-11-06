using Inventory.BLL.Interfaces;
using Inventory.Web.Util;
using System.Linq;
using System.Web.Mvc;

namespace Inventory.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService UserService;
        
        public UserController(IUserService userService)
        {
            UserService = userService;
        }

        public ActionResult Index()
        {
            var userDTOs = UserService.GetAllUsers().ToList();
            var userVMs = WebUserMapper.DtoToVm(userDTOs).ToList();

            return View(userVMs);
        }
    }
}