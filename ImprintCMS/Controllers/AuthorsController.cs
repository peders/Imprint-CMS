﻿using ImprintCMS.Models;
using System.Linq;
using System.Web.Mvc;

namespace ImprintCMS.Controllers
{
    public class AuthorsController : SiteControllerBase
    {

        public ActionResult Index()
        {
            var vm = Repository.People.Where(p => p.IsVisible && p.HasPage).OrderBy(p => p.LastName).ThenBy(p => p.FirstName);
            return View(vm);
        }

        public ActionResult Details(int id)
        {
            var vm = Repository.GetPerson(id);
            if (vm == null) return HttpNotFound();
            if (!vm.IsVisible || !vm.HasPage) return HttpNotFound();
            ViewBag.OpenGraph = vm.OpenGraph(Config.Name, Request.RequestContext);
            return View(vm);
        }

    }
}
