using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Inventory.Web.Models;
using PagedList.Mvc;
using PagedList;
using System.Net;
using Inventory.BLL.Interfaces;
using Inventory.BLL.Services;
using Inventory.BLL.DTO;
using Inventory.Web.Util;

namespace Inventory.Web.Controllers
{
    public class SearchController : Controller
    {
        private IStatusTypeService StatusTypeService;
        public SearchController(IStatusTypeService statusTypeService)
        {
            StatusTypeService = statusTypeService;
        }

        // Forms not found partial view
        public ActionResult NotFoundResult()
        {
            return PartialView("~/Views/Error/NotFoundError.cshtml");
        }

        public ActionResult AdminSearch(string title, string type)
        {
            string view = "~/Views/Search/";
            string[] words = title.ToLower().Split(' ');

            // returns not found if input string is empty
            if (title.Trim().Length <= 0)
                return RedirectToAction("NotFoundResult");

            if (type == "statusType")
            {
                List<StatusTypeVM> statusTypes = BuildDepartmentSearchQuery(words).ToList();
                BindSearchResults(statusTypes, ref view, "StatusTypes.cshtml");
            }


            return PartialView(view);
        }

        private IEnumerable<StatusTypeVM> BuildDepartmentSearchQuery(params string[] words)
        {
            IEnumerable<StatusTypeDTO> statusTypeDTOs = StatusTypeService.GetAll()
                .ToList()
                .Where(d => words.All(d.Name.ToLower().Contains));

            IEnumerable<StatusTypeVM> statusTypeVMs = WebStatusTypeMapper
               .DtoToVm(statusTypeDTOs);

            return statusTypeVMs;
        }

        // Binds entity search results and entity view
        private void BindSearchResults<T>(List<T> items, ref string view, string entityView)
        {
            if (items.Count <= 0)
            {
                view += "NotFound.cshtml";
            }
            else
            {
                ViewBag.Items = items;
                view += entityView;
            }
        }
    }
}